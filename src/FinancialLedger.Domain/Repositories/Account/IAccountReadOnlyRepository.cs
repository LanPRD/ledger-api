namespace FinancialLedger.Domain.Repositories.Account;

public interface IAccountReadOnlyRepository {
  public Task<Entities.Account?> GetById(long id);
}
