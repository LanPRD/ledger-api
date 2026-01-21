using FinancialLedger.Infrastructure.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Test;

public class AppClassFixture : IClassFixture<CustomWebAppFactory> {
  private readonly HttpClient _client;
  protected readonly CustomWebAppFactory _factory;

  public AppClassFixture(CustomWebAppFactory factory) {
    this._client = factory.CreateClient();
    this._factory = factory;
  }

  protected async Task<HttpResponseMessage> DoPost(
    string uri,
    object request,
    string culture = "en"
  ) {
    this.ChangeRequestCulture(culture);
    return await this._client.PostAsJsonAsync(uri, request);
  }

  private void ChangeRequestCulture(string culture) {
    if (string.IsNullOrWhiteSpace(culture)) {
      return;
    }

    this._client.DefaultRequestHeaders.AcceptLanguage.Clear();
    this._client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
  }

  protected T GetService<T>() where T : notnull {
    return this._factory.Services.GetRequiredService<T>();
  }

  protected void CleanDatabase() {
    using var scope = this._factory.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    dbContext.LedgerEntries.RemoveRange(dbContext.LedgerEntries);
    dbContext.IdempotencyRecords.RemoveRange(dbContext.IdempotencyRecords);

    foreach (var balance in dbContext.AccountBalance) {
      balance.Balance = 1000.00m;
      balance.UpdatedAt = DateTimeOffset.UtcNow;
    }

    dbContext.SaveChanges();
  }
}
