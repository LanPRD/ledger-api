using FinancialLedger.Domain.Entities;
using FinancialLedger.Domain.Repositories.Account;
using Microsoft.EntityFrameworkCore;

namespace FinancialLedger.Infrastructure.DataAccess.Repositories;

internal class AccountsRepository : IAccountWriteOnlyRepository, IAccountReadOnlyRepository {
  private readonly ApplicationDbContext _dbContext;

  public AccountsRepository(ApplicationDbContext dbContext) {
    this._dbContext = dbContext;
  }

  public async Task Add(Account account) {
    await this._dbContext.Accounts.AddAsync(account);
  }

  public async Task Delete(long id) {
    var result = await this._dbContext.Accounts.FindAsync(id);
    this._dbContext.Remove(result!);
  }

  public async Task<Account?> GetById(long id) {
    return await this._dbContext.Accounts.AsNoTracking().FirstOrDefaultAsync(account => account.Id == id);
  }
}
