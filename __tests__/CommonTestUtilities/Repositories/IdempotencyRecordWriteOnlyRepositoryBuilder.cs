using FinancialLedger.Domain.Repositories.IdempotencyRecords;
using Moq;

namespace CommonTestUtilities.Repositories;

public class IdempotencyRecordWriteOnlyRepositoryBuilder {
  public static Mock<IIdempotencyRecordWriteOnlyRepository> Build() {
    var mock = new Mock<IIdempotencyRecordWriteOnlyRepository>();
    return mock;
  }
}
