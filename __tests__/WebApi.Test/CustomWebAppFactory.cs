using CommonTestUtilities.Entities;
using CommonTestUtilities.Resources;
using FinancialLedger.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;

public class CustomWebAppFactory : WebApplicationFactory<Program> {
  public AccountIdentityManager Account { get; private set; } = default!;
  private SqliteConnection _connection = default!;

  protected override void ConfigureWebHost(IWebHostBuilder builder) {
    builder.UseEnvironment("Test").ConfigureServices(services => {
      // Cria conexão SQLite InMemory
      this._connection = new SqliteConnection("DataSource=:memory:");
      this._connection.Open();

      // Adiciona SQLite InMemory (único provider registrado)
      services.AddDbContext<ApplicationDbContext>(options => {
        options.UseSqlite(this._connection);
      });

      var sp = services.BuildServiceProvider();
      using var scope = sp.CreateScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

      // Cria o schema
      dbContext.Database.EnsureCreated();

      this.StartDatabase(dbContext);
    });
  }

  private void StartDatabase(ApplicationDbContext dbContext) {
    var account = AccountBuilder.Build();

    dbContext.Accounts.Add(account);
    dbContext.SaveChanges();

    this.Account = new AccountIdentityManager(account);
  }

  protected override void Dispose(bool disposing) {
    if (disposing) {
      this._connection?.Close();
      this._connection?.Dispose();
    }
    base.Dispose(disposing);
  }
}