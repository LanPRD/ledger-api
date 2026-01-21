using FinancialLedger.Domain.Entities;
using FinancialLedger.Domain.Repositories.AccountBalance;
using Microsoft.EntityFrameworkCore;

namespace FinancialLedger.Infrastructure.DataAccess.Repositories;

internal class AccountBalanceRepository : IAccountBalanceUpdateOnlyRepository {
  private readonly ApplicationDbContext _dbContext;

  public AccountBalanceRepository(ApplicationDbContext dbContext) {
    this._dbContext = dbContext;
  }

  public async Task<bool> Credit(long accountId, decimal amount) {
    var now = DateTimeOffset.UtcNow;

    var rows = await this.GetTable()
      .Where(ab => ab.AccountId == accountId && ab.Balance >= amount)
      .ExecuteUpdateAsync(setters =>
        setters
          .SetProperty(ab => ab.Balance, ab => ab.Balance + amount)
          .SetProperty(ab => ab.UpdatedAt, _ => now)
      );

    return rows == 1;
  }

  public async Task<bool> TryDebit(long accountId, decimal amount) {
    var now = DateTimeOffset.UtcNow;

    var rows = await this.GetTable()
      .Where(ab => ab.AccountId == accountId && ab.Balance >= amount)
      .ExecuteUpdateAsync(setters =>
        setters
          .SetProperty(ab => ab.Balance, ab => ab.Balance - amount)
          .SetProperty(ab => ab.UpdatedAt, _ => now)
      );

    return rows == 1;
  }

  private DbSet<AccountBalance> GetTable() {
    return this._dbContext.AccountBalance;
  }
}
