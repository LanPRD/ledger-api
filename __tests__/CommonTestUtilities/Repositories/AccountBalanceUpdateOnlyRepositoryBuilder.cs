using FinancialLedger.Domain.Repositories.AccountBalance;
using Moq;

namespace CommonTestUtilities.Repositories;

public class AccountBalanceUpdateOnlyRepositoryBuilder {
  public static Mock<IAccountBalanceUpdateOnlyRepository> Build() {
    var mock = new Mock<IAccountBalanceUpdateOnlyRepository>();

    mock.Setup(repo => repo.TryDebit(It.IsAny<long>(), It.IsAny<decimal>()))
        .ReturnsAsync(true);

    mock.Setup(repo => repo.Credit(It.IsAny<long>(), It.IsAny<decimal>()))
        .ReturnsAsync(true);

    return mock;
  }
}
