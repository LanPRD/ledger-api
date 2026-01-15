namespace FinancialLedger.Domain.Entities;

public class IdempotencyRecord {
  public long Id { get; set; }
  public Guid IdempotencyKey { get; set; }
  public long LedgerEntryId { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

  // FK
  public long AccountId { get; set; }
  public Account Account { get; set; } = default!;
}
