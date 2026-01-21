using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FinancialLedger.Application.UseCases.Account.CreateEntriesByAccountId;
using FinancialLedger.Communication.Enums;
using FinancialLedger.Domain.Entities;
using FinancialLedger.Domain.Repositories;
using FinancialLedger.Domain.Repositories.Account;
using FinancialLedger.Domain.Repositories.AccountBalance;
using FinancialLedger.Domain.Repositories.IdempotencyRecords;
using FinancialLedger.Domain.Repositories.LedgerEntries;
using FinancialLedger.Exception;
using FinancialLedger.Exception.ExceptionBase;
using FluentAssertions;
using Moq;
using System.Globalization;

namespace UseCases.Test.Account.CreateEntriesByAccountId;

public class CreateEntryByAccountIdUseCaseTest {
  private Mock<IUnitOfWork> _unitOfWorkMock = default!;
  private Mock<ILedgerEntriesWriteOnlyRepository> _ledgerEntriesWriteOnlyMock = default!;
  private Mock<IAccountBalanceUpdateOnlyRepository> _accountBalanceUpdateOnlyMock = default!;
  private Mock<IIdempotencyRecordWriteOnlyRepository> _idempotencyWriteOnlyMock = default!;
  private Mock<IAccountReadOnlyRepository> _accountReadOnlyMock = default!;

  [Fact]
  public async Task Success_Debit() {
    // arrange
    var request = RequestCreateEntryBuilder.Build();
    request.Type = LedgerEntryType.DEBIT;

    long accountId = 1;
    var account = AccountBuilder.Build();
    account.Id = accountId;

    var useCase = this.CreateUseCase(account);

    // act
    var result = await useCase.Execute(accountId, request);

    // assert
    result.Should().NotBeNull();
    result.Type.Should().Be(request.Type);
    result.Amount.Should().Be(request.Amount);
    result.Description.Should().Be(request.Description);

    this._idempotencyWriteOnlyMock.Verify(
      repo => repo.TryAdd(It.Is<IdempotencyRecord>(
        i => i.AccountId == accountId && i.IdempotencyKey == request.IdempotencyKey
      )),
      Times.Once
    );

    this._unitOfWorkMock.Verify(uow => uow.Flush(), Times.Once);

    this._accountBalanceUpdateOnlyMock.Verify(
      repo => repo.TryDebit(accountId, request.Amount),
      Times.Once
    );

    this._ledgerEntriesWriteOnlyMock.Verify(
      repo => repo.Add(It.Is<LedgerEntry>(
        l => l.AccountId == accountId && l.Amount == request.Amount
      )),
      Times.Once
    );
  }

  [Fact]
  public async Task Success_Credit() {
    // arrange
    var request = RequestCreateEntryBuilder.Build();
    request.Type = LedgerEntryType.CREDIT;

    long accountId = 1;
    var account = AccountBuilder.Build();
    account.Id = accountId;

    var useCase = this.CreateUseCase(account);

    // act
    var result = await useCase.Execute(accountId, request);

    // assert
    result.Should().NotBeNull();
    result.Type.Should().Be(LedgerEntryType.CREDIT);

    this._accountBalanceUpdateOnlyMock.Verify(
      repo => repo.Credit(accountId, request.Amount),
      Times.Once
    );

    this._accountBalanceUpdateOnlyMock.Verify(
      repo => repo.TryDebit(It.IsAny<long>(), It.IsAny<decimal>()),
      Times.Never
    );
  }

  [Fact]
  public async Task Error_InsufficientBalance() {
    // arrange
    var request = RequestCreateEntryBuilder.Build();
    request.Type = LedgerEntryType.DEBIT;

    long accountId = 1;
    var account = AccountBuilder.Build();
    account.Id = accountId;

    var useCase = this.CreateUseCase(account);

    this._accountBalanceUpdateOnlyMock
      .Setup(repo => repo.TryDebit(It.IsAny<long>(), It.IsAny<decimal>()))
      .ReturnsAsync(false);

    // act
    var act = async () => await useCase.Execute(accountId, request);

    // assert
    await act.Should()
      .ThrowAsync<ValidationException>()
      .WithMessage(ResourceErrorMessages.INSUFFICIENT_BALANCE);
  }

  [Fact]
  public async Task Error_AccountNotFound() {
    // arrange
    var request = RequestCreateEntryBuilder.Build();
    long accountId = 99999;

    var useCase = this.CreateUseCase(accountExists: false);

    // act
    var act = async () => await useCase.Execute(accountId, request);

    // assert
    var expectedMessage = ResourceErrorMessages.ResourceManager.GetString(
      "ACCOUNT_NOT_FOUND",
      new CultureInfo("pt-BR")
    );

    await act.Should()
      .ThrowAsync<NotFoundException>()
      .WithMessage(expectedMessage);
  }

  private CreateEntryByAccountIdUseCase CreateUseCase(
    FinancialLedger.Domain.Entities.Account? account = null,
    bool accountExists = true
  ) {
    var mapper = MapperBuilder.Build();

    var accountReadOnlyBuilder = new AccountReadOnlyRepositoryBuilder();

    if (accountExists && account != null) {
      accountReadOnlyBuilder.GetById(account);
    } else if (!accountExists) {
      accountReadOnlyBuilder.GetByIdReturnsNull(99999);
    }

    this._unitOfWorkMock = UnitOfWorkBuilder.Build();
    this._ledgerEntriesWriteOnlyMock = LedgerEntriesWriteOnlyRepositoryBuilder.Build();
    this._accountBalanceUpdateOnlyMock = AccountBalanceUpdateOnlyRepositoryBuilder.Build();
    this._accountReadOnlyMock = accountReadOnlyBuilder.Build();
    this._idempotencyWriteOnlyMock = IdempotencyRecordWriteOnlyRepositoryBuilder.Build();

    return new CreateEntryByAccountIdUseCase(
      mapper,
      this._unitOfWorkMock.Object,
      this._ledgerEntriesWriteOnlyMock.Object,
      this._accountBalanceUpdateOnlyMock.Object,
      this._idempotencyWriteOnlyMock.Object,
      this._accountReadOnlyMock.Object
    );
  }
}