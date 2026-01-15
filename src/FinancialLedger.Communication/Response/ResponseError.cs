namespace FinancialLedger.Communication.Response;

public class ResponseError {
  public string ErrorTitle { get; set; } = string.Empty;
  public string ErrorMessage { get; set; } = string.Empty;
  public string TraceId { get; set; } = string.Empty;
}
