using System.Net;

namespace FinancialLedger.Exception.ExceptionBase;

public class ConflictException : FinancialLedgerException {
  public override int StatusCode => (int)HttpStatusCode.Conflict;

  public ConflictException(string message) : base(message) { }


  public override string GetErrorTitle() => ResourceErrorMessages.NOT_FOUND_ERROR_TITLE;
  public override string GetErrorDescription() => this.Message;
}
