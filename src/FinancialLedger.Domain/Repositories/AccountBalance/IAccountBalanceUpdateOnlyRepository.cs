
namespace FinancialLedger.Domain.Repositories.AccountBalance;

public interface IAccountBalanceUpdateOnlyRepository {
  public Task<bool> TryDebit(long accountId, decimal amount);
  public Task<bool> Credit(long accountId, decimal amount);
}
