using FinancialLedger.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class AccountBuilder {
  public static Account Build() {
    return new Account {
      Id = 1,
      CreatedAt = DateTimeOffset.UtcNow,
      AccountBalance = new AccountBalance {
        AccountId = 1,
        Balance = 1000.00m,
        UpdatedAt = DateTimeOffset.UtcNow
      }
    };
  }

  public static Account BuildWithBalance(decimal balance) {
    return new Account {
      Id = 1,
      CreatedAt = DateTimeOffset.UtcNow,
      AccountBalance = new AccountBalance {
        AccountId = 1,
        Balance = balance,
        UpdatedAt = DateTimeOffset.UtcNow
      }
    };
  }
}