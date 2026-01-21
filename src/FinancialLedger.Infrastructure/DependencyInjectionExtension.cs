using FinancialLedger.Domain.Repositories;
using FinancialLedger.Domain.Repositories.Account;
using FinancialLedger.Domain.Repositories.AccountBalance;
using FinancialLedger.Domain.Repositories.IdempotencyRecords;
using FinancialLedger.Domain.Repositories.LedgerEntries;
using FinancialLedger.Infrastructure.DataAccess;
using FinancialLedger.Infrastructure.DataAccess.Repositories;
using FinancialLedger.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialLedger.Infrastructure;

public static class DependencyInjectionExtension {
  public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
    DependencyInjectionExtension.AddRepositories(services);

    if (!configuration.IsTestEnvironment()) {
      DependencyInjectionExtension.AddDbContext(services, configuration);
    }
  }

  private static void AddDbContext(IServiceCollection services, IConfiguration configuration) {
    var connectionString = configuration.GetConnectionString("Connection");
    services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
  }

  private static void AddRepositories(IServiceCollection services) {
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped<IAccountReadOnlyRepository, AccountsRepository>();
    services.AddScoped<IAccountWriteOnlyRepository, AccountsRepository>();
    services.AddScoped<ILedgerEntriesWriteOnlyRepository, LedgerEntriesRespository>();
    services.AddScoped<IAccountBalanceUpdateOnlyRepository, AccountBalanceRepository>();
    services.AddScoped<IIdempotencyRecordWriteOnlyRepository, IdempotencyRecordRepository>();
  }
}
