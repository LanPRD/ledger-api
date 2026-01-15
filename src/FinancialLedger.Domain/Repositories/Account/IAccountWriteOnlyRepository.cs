namespace FinancialLedger.Domain.Repositories.Account;

public interface IAccountWriteOnlyRepository {
  public Task Add(Entities.Account account);
  public Task Delete(long id);
}
