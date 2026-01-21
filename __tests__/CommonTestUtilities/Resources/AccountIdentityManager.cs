using FinancialLedger.Domain.Entities;

namespace CommonTestUtilities.Resources;

public class AccountIdentityManager {
  public long Id { get; }
  public decimal InitialBalance { get; }

  public AccountIdentityManager(Account account) {
    this.Id = account.Id;
    this.InitialBalance = account.AccountBalance.Balance;
  }
}
