using FinancialLedger.Communication.Enums;
using FinancialLedger.Communication.Requests;
using FinancialLedger.Communication.Response;
using FinancialLedger.Communication.Responses;
using FinancialLedger.Exception;
using FinancialLedger.Infrastructure.DataAccess;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;

namespace WebApi.Test.Account;

public class CreateLedgerEntryTest : AppClassFixture {
  private const string URI = "api/account";
  private readonly long _id;
  private readonly decimal _initialBalance;

  public CreateLedgerEntryTest(CustomWebAppFactory factory) : base(factory) {
    this._id = factory.Account.Id;
    this._initialBalance = factory.Account.InitialBalance;

    this.CleanDatabase();
  }

  [Fact]
  public async Task Success_CreateDebitEntry() {
    // Arrange
    var accountId = this._id;
    var request = new RequestCreateEntry {
      Type = LedgerEntryType.DEBIT,
      Amount = 100.00m,
      Description = "Test debit",
      IdempotencyKey = Guid.NewGuid()
    };

    // Act
    var result = await this.DoPost(
      $"{URI}/{accountId}/entries",
      request
    );

    // Assert
    var response = await result.Content.ReadFromJsonAsync<ResponseLedgerEntry>();

    response.Should().NotBeNull();
    response!.Type.Should().Be(LedgerEntryType.DEBIT);
    response.Amount.Should().Be(100.00m);
    response.Description.Should().Be("Test debit");
  }

  [Fact]
  public async Task Idempotency_SameKey_ShouldReturnConflict() {
    // Arrange
    var idempotencyKey = Guid.NewGuid();
    var request = new RequestCreateEntry {
      Type = LedgerEntryType.DEBIT,
      Amount = 50.00m,
      Description = "Idempotency test",
      IdempotencyKey = idempotencyKey
    };

    // Act
    var firstResult = await this.DoPost($"{URI}/{this._id}/entries", request);
    var secondResult = await this.DoPost($"{URI}/{this._id}/entries", request);

    // Assert
    firstResult.StatusCode.Should().Be(HttpStatusCode.Created);
    secondResult.StatusCode.Should().Be(HttpStatusCode.Conflict);

    using var scope = this.GetService<IServiceProvider>().CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    var entries = dbContext.LedgerEntries
      .Where(e => e.AccountId == this._id && e.Description == "Idempotency test")
      .ToList();

    entries.Should().HaveCount(1);
    entries[0].Amount.Should().Be(50.00m);
  }

  [Fact]
  public async Task Idempotency_DifferentKeys_BothSucceed() {
    // Arrange
    var request1 = new RequestCreateEntry {
      Type = LedgerEntryType.DEBIT,
      Amount = 30.00m,
      Description = "First request",
      IdempotencyKey = Guid.NewGuid()
    };

    var request2 = new RequestCreateEntry {
      Type = LedgerEntryType.DEBIT,
      Amount = 40.00m,
      Description = "Second request",
      IdempotencyKey = Guid.NewGuid()
    };

    // Act
    var result1 = await this.DoPost($"{URI}/{this._id}/entries", request1);
    var result2 = await this.DoPost($"{URI}/{this._id}/entries", request2);

    // Assert
    result1.StatusCode.Should().Be(HttpStatusCode.Created);
    result2.StatusCode.Should().Be(HttpStatusCode.Created);

    // Verifica que AMBAS as entradas existem
    using var scope = this.GetService<IServiceProvider>().CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    var entries = dbContext.LedgerEntries
      .Where(e => e.AccountId == this._id && (e.Description == "First request" || e.Description == "Second request"))
      .ToList();

    entries.Should().HaveCount(2);
  }

  [Fact]
  public async Task Calculate_Balance_Correctly() {
    // Arrange
    var initialBalance = this._initialBalance;
    decimal debit1 = 30.00m;
    decimal credit1 = 60.00m;
    decimal debit2 = 100.00m;

    var request1 = new RequestCreateEntry {
      Type = LedgerEntryType.DEBIT,
      Amount = debit1,
      Description = "First request",
      IdempotencyKey = Guid.NewGuid()
    };

    var request2 = new RequestCreateEntry {
      Type = LedgerEntryType.CREDIT,
      Amount = credit1,
      Description = "Second request",
      IdempotencyKey = Guid.NewGuid()
    };

    var request3 = new RequestCreateEntry {
      Type = LedgerEntryType.DEBIT,
      Amount = debit2,
      Description = "Second request",
      IdempotencyKey = Guid.NewGuid()
    };

    // Act
    var result1 = await this.DoPost($"{URI}/{this._id}/entries", request1);
    var result2 = await this.DoPost($"{URI}/{this._id}/entries", request2);
    var result3 = await this.DoPost($"{URI}/{this._id}/entries", request3);

    // Assert
    result1.StatusCode.Should().Be(HttpStatusCode.Created);
    result2.StatusCode.Should().Be(HttpStatusCode.Created);
    result3.StatusCode.Should().Be(HttpStatusCode.Created);

    using var scope = this.GetService<IServiceProvider>().CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    var accountBalance = dbContext.AccountBalance.First(ab => ab.AccountId == this._id);
    var expectedBalance = initialBalance - debit1 + credit1 - debit2;

    accountBalance.Balance.Should().Be(expectedBalance);
  }

  [Fact]
  public async Task Error_InsufficientBalance() {
    // Arrange
    var request = new RequestCreateEntry {
      Type = LedgerEntryType.DEBIT,
      Amount = 99999.00m,
      Description = "Overdraft attempt",
      IdempotencyKey = Guid.NewGuid()
    };

    // Act
    var result = await this.DoPost($"{URI}/{this._id}/entries", request);

    // Assert
    result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    var error = await result.Content.ReadFromJsonAsync<ResponseError>();
    var expectedMessage = ResourceErrorMessages.ResourceManager.GetString(
      "INSUFFICIENT_BALANCE",
      new CultureInfo("en")
    );

    error!.ErrorMessage.Should().Be(expectedMessage);
  }

  [Fact]
  public async Task Error_InvalidAmount_Zero() {
    // Arrange
    var request = new RequestCreateEntry {
      Type = LedgerEntryType.DEBIT,
      Amount = 0m,
      Description = "Invalid",
      IdempotencyKey = Guid.NewGuid()
    };

    // Act
    var result = await this.DoPost($"{URI}/{this._id}/entries", request);

    // Assert
    result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
  }

  [Fact]
  public async Task Error_InvalidAmount_Negative() {
    // Arrange
    var request = new RequestCreateEntry {
      Type = LedgerEntryType.DEBIT,
      Amount = -10m,
      Description = "Negative",
      IdempotencyKey = Guid.NewGuid()
    };

    // Act
    var result = await this.DoPost($"{URI}/{this._id}/entries", request);

    // Assert
    result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
  }

  [Fact]
  public async Task Error_AccountNotFound() {
    // Arrange
    var request = new RequestCreateEntry {
      Type = LedgerEntryType.DEBIT,
      Amount = 10m,
      Description = "Test",
      IdempotencyKey = Guid.NewGuid()
    };

    // Act
    var result = await this.DoPost($"{URI}/99999/entries", request);

    // Assert
    result.StatusCode.Should().Be(HttpStatusCode.NotFound);
  }

  [Fact]
  public async Task Success_MultipleCredits_IncreasesBalance() {
    // Arrange
    var initialBalance = this._initialBalance;

    var request1 = new RequestCreateEntry {
      Type = LedgerEntryType.CREDIT,
      Amount = 100m,
      Description = "Credit 1",
      IdempotencyKey = Guid.NewGuid()
    };

    var request2 = new RequestCreateEntry {
      Type = LedgerEntryType.CREDIT,
      Amount = 200m,
      Description = "Credit 2",
      IdempotencyKey = Guid.NewGuid()
    };

    // Act
    await this.DoPost($"{URI}/{this._id}/entries", request1);
    await this.DoPost($"{URI}/{this._id}/entries", request2);

    // Assert
    using var scope = this.GetService<IServiceProvider>().CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    var balance = dbContext.AccountBalance.First(ab => ab.AccountId == this._id);
    balance.Balance.Should().Be(initialBalance + 300m);
  }

  [Fact]
  public async Task Error_EmptyIdempotencyKey() {
    var request = new RequestCreateEntry {
      Type = LedgerEntryType.DEBIT,
      Amount = 10m,
      Description = "Test",
      IdempotencyKey = Guid.Empty
    };

    var result = await this.DoPost($"{URI}/{this._id}/entries", request);

    result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
  }

  [Fact]
  public async Task Success_DebitExactBalance_BalanceZero() {
    var currentBalance = this._initialBalance;

    var request = new RequestCreateEntry {
      Type = LedgerEntryType.DEBIT,
      Amount = currentBalance,
      Description = "Empty account",
      IdempotencyKey = Guid.NewGuid()
    };

    var result = await this.DoPost($"{URI}/{this._id}/entries", request);

    result.StatusCode.Should().Be(HttpStatusCode.Created);

    using var scope = this.GetService<IServiceProvider>().CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    var balance = dbContext.AccountBalance.First(ab => ab.AccountId == this._id);
    balance.Balance.Should().Be(0m);
  }
}
