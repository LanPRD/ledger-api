using FinancialLedger.Communication.Enums;

namespace FinancialLedger.Communication.Requests;

public class RequestCreateEntry {
  public LedgerEntryType Type { get; set; }
  public decimal Amount { get; set; }
  public string? Description { get; set; }
  public Guid IdempotencyKey { get; set; }
}
