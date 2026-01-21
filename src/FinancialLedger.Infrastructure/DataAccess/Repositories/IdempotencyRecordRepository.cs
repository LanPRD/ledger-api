using FinancialLedger.Domain.Entities;
using FinancialLedger.Domain.Repositories.IdempotencyRecords;
using Microsoft.EntityFrameworkCore;

namespace FinancialLedger.Infrastructure.DataAccess.Repositories;

internal class IdempotencyRecordRepository : IIdempotencyRecordWriteOnlyRepository {
  private readonly ApplicationDbContext _dbContext;

  public IdempotencyRecordRepository(ApplicationDbContext dbContext) {
    this._dbContext = dbContext;
  }

  public Task TryAdd(IdempotencyRecord record) {
    _dbContext.IdempotencyRecords.Add(record);
    return Task.CompletedTask;
  }
}
