using AutoMapper;
using FinancialLedger.Communication.Response;
using FinancialLedger.Domain.Repositories;
using FinancialLedger.Domain.Repositories.Account;

namespace FinancialLedger.Application.UseCases.Account.Create;

public class CreateAccountUseCase : ICreateAccountUseCase {
  private readonly IMapper _mapper;
  private readonly IUnitOfWork _unitOfWork;
  private readonly IAccountWriteOnlyRepository _repository;

  public CreateAccountUseCase(IUnitOfWork unitOfWork, IAccountWriteOnlyRepository repository, IMapper mapper) {
    this._mapper = mapper;
    this._unitOfWork = unitOfWork;
    this._repository = repository;
  }

  public async Task<ResponseAccount> Execute() {
    var entity = new Domain.Entities.Account { };
    await this._repository.Add(entity);
    await this._unitOfWork.Commit();

    return this._mapper.Map<ResponseAccount>(entity);
  }
}
