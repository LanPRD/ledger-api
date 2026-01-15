using FinancialLedger.Domain.Enums;

namespace FinancialLedger.Domain.Entities;

public class LedgerEntry {
  public long Id { get; set; }
  public LedgerEntryType Type { get; set; }
  public decimal Amount { get; set; }
  public string? Description { get; set; }
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

  // FK
  public long AccountId { get; set; }
  public Account Account { get; set; } = default!;
}
