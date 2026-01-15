using FinancialLedger.Domain.Entities;
using FinancialLedger.Domain.Repositories.LedgerEntries;

namespace FinancialLedger.Infrastructure.DataAccess.Repositories;

internal class LedgerEntriesRespository : ILedgerEntriesWriteOnlyRepository {
  private readonly ApplicationDbContext _dbContext;

  public LedgerEntriesRespository(ApplicationDbContext dbContext) {
    this._dbContext = dbContext;
  }

  public Task Add(LedgerEntry ledgerEntry) {
    this._dbContext.LedgerEntries.Add(ledgerEntry);
    return Task.CompletedTask;
  }
}
