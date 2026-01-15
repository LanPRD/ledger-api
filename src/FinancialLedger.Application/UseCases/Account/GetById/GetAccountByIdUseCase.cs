
using AutoMapper;
using FinancialLedger.Communication.Response;
using FinancialLedger.Domain.Repositories.Account;
using FinancialLedger.Exception;
using FinancialLedger.Exception.ExceptionBase;

namespace FinancialLedger.Application.UseCases.Account.GetById;

internal class GetAccountByIdUseCase : IGetAccountByIdUseCase {
  private readonly IMapper _mapper;
  private readonly IAccountReadOnlyRepository _repository;

  public GetAccountByIdUseCase(IMapper mapper, IAccountReadOnlyRepository repository) {
    this._mapper = mapper;
    this._repository = repository;
  }

  public async Task<ResponseAccount> Execute(long id) {
    var result = await this._repository.GetById(id);

    if (result == null) {
      throw new NotFoundException(ResourceErrorMessages.ACCOUNT_NOT_FOUND);
    }

    return this._mapper.Map<ResponseAccount>(result);
  }
}
