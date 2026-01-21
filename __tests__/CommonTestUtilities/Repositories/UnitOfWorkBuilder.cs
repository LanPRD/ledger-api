using FinancialLedger.Domain.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories;

public class UnitOfWorkBuilder {
  public static Mock<IUnitOfWork> Build() {
    var mock = new Mock<IUnitOfWork>();

    mock.Setup(uow => uow.ExecuteInTransaction(It.IsAny<Func<Task>>()))
        .Returns<Func<Task>>(async func => await func());

    return mock;
  }
}
