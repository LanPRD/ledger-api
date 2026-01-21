using FinancialLedger.Domain.Repositories.LedgerEntries;
using Moq;

namespace CommonTestUtilities.Repositories;

public class LedgerEntriesWriteOnlyRepositoryBuilder {
  public static Mock<ILedgerEntriesWriteOnlyRepository> Build() {
    var repository = new Mock<ILedgerEntriesWriteOnlyRepository>();
    return repository;
  }
}
