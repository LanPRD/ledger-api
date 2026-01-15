using FinancialLedger.Domain.Entities;

namespace FinancialLedger.Domain.Repositories.LedgerEntries;

public interface ILedgerEntriesWriteOnlyRepository {
  public Task Add(LedgerEntry ledgerEntry);
}
