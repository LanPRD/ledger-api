using FinancialLedger.Communication.Response;

namespace FinancialLedger.Application.UseCases.Account.Create;

public interface ICreateAccountUseCase {
  public Task<ResponseAccount> Execute();
}
