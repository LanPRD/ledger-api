using FinancialLedger.Domain.Repositories;
using FinancialLedger.Domain.Repositories.Account;
using FinancialLedger.Infrastructure.DataAccess;
using FinancialLedger.Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialLedger.Infrastructure;

public static class DependencyInjectionExtension {
  public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
    DependencyInjectionExtension.AddRepositories(services);
    DependencyInjectionExtension.AddDbContext(services, configuration);
  }

  private static void AddDbContext(IServiceCollection services, IConfiguration configuration) {
    var connectionString = configuration.GetConnectionString("Connection");
    services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
  }

  private static void AddRepositories(IServiceCollection services) {
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped<IAccountReadOnlyRepository, AccountRepository>();
    services.AddScoped<IAccountWriteOnlyRepository, AccountRepository>();
  }
}
