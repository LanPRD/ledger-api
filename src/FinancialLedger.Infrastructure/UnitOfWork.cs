using FinancialLedger.Domain.Repositories;
using FinancialLedger.Infrastructure.DataAccess;

namespace FinancialLedger.Infrastructure;

internal class UnitOfWork : IUnitOfWork {
  private readonly ApplicationDbContext _dbContext;

  public UnitOfWork(ApplicationDbContext dbContext) {
    this._dbContext = dbContext;
  }

  public async Task Commit() => await this._dbContext.SaveChangesAsync();
}
