using System.Net;

namespace FinancialLedger.Exception.ExceptionBase;

public class NotFoundException : FinancialLedgerException {
  public override int StatusCode => (int)HttpStatusCode.NotFound;

  public NotFoundException(string message) : base(message) { }

  public override string GetErrorTitle() => ResourceErrorMessages.NOT_FOUND_ERROR_TITLE;
  public override string GetErrorDescription() => this.Message;
}
