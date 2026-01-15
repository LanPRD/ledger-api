using FinancialLedger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialLedger.Infrastructure.DataAccess;

public class ApplicationDbContext : DbContext {
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

  public DbSet<Account> Accounts => Set<Account>();
  public DbSet<LedgerEntry> LedgerEntrys => Set<LedgerEntry>();
  public DbSet<IdempotencyRecord> IdempotencyRecords => Set<IdempotencyRecord>();

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<AccountBalance>().HasNoKey().ToTable("AccountBalance");
  }
}
