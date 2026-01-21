using FinancialLedger.Domain.Repositories;
using FinancialLedger.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace FinancialLedger.Infrastructure;

internal class UnitOfWork : IUnitOfWork {
  private readonly ApplicationDbContext _dbContext;

  public UnitOfWork(ApplicationDbContext dbContext) {
    this._dbContext = dbContext;
  }

  public async Task Commit() => await this._dbContext.SaveChangesAsync();

  public async Task Flush() {
    try {
      await _dbContext.SaveChangesAsync();
    } catch (DbUpdateException dbEx) {
      throw DbExceptionMapper.MapToApplicationException(dbEx);
    }
  }

  public async Task ExecuteInTransaction(Func<Task> action) {
    await using var tx = await this._dbContext.Database.BeginTransactionAsync();
    try {
      await action();
      await this._dbContext.SaveChangesAsync();
      await tx.CommitAsync();
    } catch (DbUpdateException dbEx) {
      await tx.RollbackAsync();
      throw DbExceptionMapper.MapToApplicationException(dbEx);
    } catch {
      await tx.RollbackAsync();
      throw;
    }
  }
}
