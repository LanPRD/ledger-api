using FinancialLedger.Domain.Entities;
using FinancialLedger.Domain.Repositories.Account;
using Moq;

namespace CommonTestUtilities.Repositories;

public class AccountReadOnlyRepositoryBuilder {
  private readonly Mock<IAccountReadOnlyRepository> _repository;

  public AccountReadOnlyRepositoryBuilder() {
    this._repository = new Mock<IAccountReadOnlyRepository>();
  }

  public AccountReadOnlyRepositoryBuilder GetById(Account account) {
    this._repository.Setup(repo => repo.GetById(account.Id))
               .ReturnsAsync(account);
    return this;
  }

  public AccountReadOnlyRepositoryBuilder GetByIdReturnsNull(long accountId) {
    this._repository.Setup(repo => repo.GetById(accountId))
               .ReturnsAsync((Account?)null);
    return this;
  }

  public Mock<IAccountReadOnlyRepository> Build() => this._repository;
}