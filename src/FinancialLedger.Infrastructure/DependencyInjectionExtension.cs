using FinancialLedger.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialLedger.Infrastructure;

public static class DependencyInjectionExtension {
  public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
    DependencyInjectionExtension.AddDbContext(services, configuration);
  }

  private static void AddDbContext(IServiceCollection services, IConfiguration configuration) {
    var connectionString = configuration.GetConnectionString("Connection");
    services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
  }
}
