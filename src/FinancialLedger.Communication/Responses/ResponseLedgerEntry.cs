using FinancialLedger.Communication.Enums;

namespace FinancialLedger.Communication.Responses;

public class ResponseLedgerEntry {
  public long Id { get; set; }
  public LedgerEntryType Type { get; set; }
  public decimal Amount { get; set; }
  public string? Description { get; set; }
  public DateTimeOffset CreatedAt { get; set; }
}
