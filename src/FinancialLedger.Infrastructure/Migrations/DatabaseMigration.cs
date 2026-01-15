using FinancialLedger.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialLedger.Infrastructure.Migrations;

public static class DatabaseMigration {
  public async static Task ApplyMigrations(IServiceProvider serviceProvider) {
    var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();
  }
}
