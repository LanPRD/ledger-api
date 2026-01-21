using FinancialLedger.Exception;
using FinancialLedger.Exception.ExceptionBase;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FinancialLedger.Infrastructure;

public static class DbExceptionMapper {
  public static System.Exception MapToApplicationException(DbUpdateException dbException) {
    if (dbException.InnerException is PostgresException pgEx) {
      if (pgEx.SqlState == PostgresErrorCodes.UniqueViolation &&
          (pgEx.ConstraintName?.Contains("IdempotencyKey") == true ||
           pgEx.MessageText.Contains("IdempotencyKey"))) {
        return new ConflictException(ResourceErrorMessages.ENTRY_ALREADY_CREATED);
      }
    }

    if (dbException.InnerException?.GetType().Name == "SqliteException") {
      if (IsSqliteUniqueConstraintViolation(dbException.InnerException)) {
        return new ConflictException(ResourceErrorMessages.ENTRY_ALREADY_CREATED);
      }
    }

    return dbException;
  }

  private static bool IsSqliteUniqueConstraintViolation(System.Exception sqliteException) {
    var errorCodeProperty = sqliteException.GetType().GetProperty("SqliteErrorCode");
    if (errorCodeProperty == null)
      return false;

    var errorCode = (int?)errorCodeProperty.GetValue(sqliteException);
    var message = sqliteException.Message;

    return errorCode == 19 && message.Contains("IdempotencyKey");
  }
}