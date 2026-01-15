using System.Net;

namespace FinancialLedger.Exception.ExceptionBase;

public abstract class FinancialLedgerException : SystemException {
  public abstract int StatusCode { get; }

  protected FinancialLedgerException(string message) : base(message) { }

  public abstract string GetErrorTitle();
  public abstract string GetErrorDescription();
}
