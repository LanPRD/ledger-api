using FinancialLedger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancialLedger.Infrastructure.DataAccess;

public class ApplicationDbContext : DbContext {
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

  public DbSet<Account> Accounts => Set<Account>();
  public DbSet<LedgerEntry> LedgerEntries => Set<LedgerEntry>();
  public DbSet<AccountBalance> AccountBalance => Set<AccountBalance>();
  public DbSet<IdempotencyRecord> IdempotencyRecords => Set<IdempotencyRecord>();

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<AccountBalance>(entity => {
      entity.HasKey(ab => ab.AccountId);
      entity.HasOne(ab => ab.Account).WithOne(a => a.AccountBalance).HasForeignKey<AccountBalance>(ab => ab.AccountId);
      entity.Property(ab => ab.Balance).IsRequired();
      entity.Property(ab => ab.UpdatedAt).IsRequired();
    });


    modelBuilder.Entity<IdempotencyRecord>(entity => {
      entity.HasKey(ir => ir.Id);
      entity.HasOne(ir => ir.Account).WithMany().HasForeignKey(ir => ir.AccountId);
      entity.HasOne(ir => ir.LedgerEntry).WithOne().HasForeignKey<IdempotencyRecord>(ir => ir.LedgerEntryId);
      entity.HasIndex(ir => new { ir.AccountId, ir.IdempotencyKey }).IsUnique();
    });
  }
}
