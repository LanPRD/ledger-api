using System.Net;

namespace FinancialLedger.Exception.ExceptionBase;

public class ValidationException : FinancialLedgerException {
  public override int StatusCode => (int)HttpStatusCode.BadRequest;

  public ValidationException(string errorMessage) : base(errorMessage) { }


  public override string GetErrorTitle() => ResourceErrorMessages.VALIDATION_ERROR_TITLE;
  public override string GetErrorDescription() => this.Message;
}
