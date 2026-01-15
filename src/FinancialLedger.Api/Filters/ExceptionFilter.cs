using FinancialLedger.Communication.Response;
using FinancialLedger.Exception;
using FinancialLedger.Exception.ExceptionBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FinancialLedger.Api.Filters;

public class ExceptionFilter : IExceptionFilter {
  public void OnException(ExceptionContext context) {
    if (context.Exception is FinancialLedgerException) {
      this.HandleProjectException(context);
    } else {
      this.ThrownUnknowException(context);
    }
  }

  private void HandleProjectException(ExceptionContext context) {
    var appException = (FinancialLedgerException)context.Exception;
    var errorResponse = new ResponseError {
      ErrorTitle = appException.GetErrorTitle(),
      ErrorMessage = appException.GetErrorDescription(),
      TraceId = context.HttpContext.TraceIdentifier
    };

    context.HttpContext.Response.StatusCode = appException.StatusCode;
    context.Result = new ObjectResult(errorResponse);
  }

  private void ThrownUnknowException(ExceptionContext context) {
    var errorResponse = new ResponseError {
      ErrorTitle = ResourceErrorMessages.UNKNOWN_ERROR,
      ErrorMessage = ResourceErrorMessages.UNKNOWN_ERROR,
      TraceId = context.HttpContext.TraceIdentifier
    };

    context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
    context.Result = new ObjectResult(errorResponse);
  }
}
