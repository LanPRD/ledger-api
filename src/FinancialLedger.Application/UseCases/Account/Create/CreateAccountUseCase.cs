using AutoMapper;
using FinancialLedger.Communication.Response;
using FinancialLedger.Domain.Repositories;
using FinancialLedger.Domain.Repositories.Account;

namespace FinancialLedger.Application.UseCases.Account.Create;

public class CreateAccountUseCase : ICreateAccountUseCase {
  private readonly IMapper _mapper;
  private readonly IUnitOfWork _unitOfWork;
  private readonly IAccountWriteOnlyRepository _accountWriteRepository;

  public CreateAccountUseCase(IUnitOfWork unitOfWork, IAccountWriteOnlyRepository accountWriteRepository, IMapper mapper) {
    this._mapper = mapper;
    this._unitOfWork = unitOfWork;
    this._accountWriteRepository = accountWriteRepository;
  }

  public async Task<ResponseAccount> Execute() {
    var account = new Domain.Entities.Account { };
    var accountBalance = new Domain.Entities.AccountBalance {
      Balance = 0,
    };

    account.AccountBalance = accountBalance;

    await this._accountWriteRepository.Add(account);
    await this._unitOfWork.Commit();

    return this._mapper.Map<ResponseAccount>(account);
  }
}
