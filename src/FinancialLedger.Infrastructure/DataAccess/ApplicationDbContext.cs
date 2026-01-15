using FinancialLedger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialLedger.Infrastructure.DataAccess;

public class ApplicationDbContext : DbContext {
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

  public DbSet<Account> Accounts => Set<Account>();
}
