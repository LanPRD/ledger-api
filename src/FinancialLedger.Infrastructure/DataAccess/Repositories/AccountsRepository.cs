using FinancialLedger.Domain.Entities;
using FinancialLedger.Domain.Repositories.Account;
using Microsoft.EntityFrameworkCore;

namespace FinancialLedger.Infrastructure.DataAccess.Repositories;

internal class AccountsRepository : IAccountWriteOnlyRepository, IAccountReadOnlyRepository {
  private readonly ApplicationDbContext _dbContext;

  public AccountsRepository(ApplicationDbContext dbContext) {
    this._dbContext = dbContext;
  }

  public Task Add(Account account) {
    this._dbContext.Accounts.Add(account);
    return Task.CompletedTask;
  }

  public async Task Delete(long id) {
    var result = await this._dbContext.Accounts.FindAsync(id);
    this._dbContext.Remove(result!);
  }

  public async Task<Account?> GetById(long id) {
    return await this._dbContext.Accounts.AsNoTracking().FirstOrDefaultAsync(account => account.Id == id);
  }
}
