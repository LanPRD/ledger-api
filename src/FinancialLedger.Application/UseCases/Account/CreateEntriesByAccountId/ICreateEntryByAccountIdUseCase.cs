using FinancialLedger.Communication.Requests;
using FinancialLedger.Communication.Responses;

namespace FinancialLedger.Application.UseCases.Account.CreateEntriesByAccountId;

public interface ICreateEntryByAccountIdUseCase {
  public Task<ResponseLedgerEntry> Execute(long accountId, RequestCreateEntry request);
}
