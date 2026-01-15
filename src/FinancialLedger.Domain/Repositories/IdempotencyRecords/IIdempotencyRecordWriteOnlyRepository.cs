using FinancialLedger.Domain.Entities;

namespace FinancialLedger.Domain.Repositories.IdempotencyRecords;

public interface IIdempotencyRecordWriteOnlyRepository {
  public Task TryAdd(IdempotencyRecord idempotencyRecord);
}
