using FinancialLedger.Communication.Response;

namespace FinancialLedger.Application.UseCases.Account.GetById;

public interface IGetAccountByIdUseCase {
  public Task<ResponseAccount> Execute(long id);
}
