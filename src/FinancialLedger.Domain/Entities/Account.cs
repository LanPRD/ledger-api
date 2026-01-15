namespace FinancialLedger.Domain.Entities;

public class Account {
  public long Id { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
