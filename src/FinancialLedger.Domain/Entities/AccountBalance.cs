namespace FinancialLedger.Domain.Entities;

public class AccountBalance {
  public decimal Balance { get; set; }
  public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

  // FK
  public long AccountId { get; set; }
  public Account Account { get; set; } = default!;
}
