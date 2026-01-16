using AutoMapper;
using FinancialLedger.Communication.Enums;
using FinancialLedger.Communication.Requests;
using FinancialLedger.Communication.Responses;
using FinancialLedger.Domain.Entities;
using FinancialLedger.Domain.Repositories;
using FinancialLedger.Domain.Repositories.AccountBalance;
using FinancialLedger.Domain.Repositories.IdempotencyRecords;
using FinancialLedger.Domain.Repositories.LedgerEntries;
using FinancialLedger.Exception;
using FinancialLedger.Exception.ExceptionBase;

namespace FinancialLedger.Application.UseCases.Account.CreateEntriesByAccountId;

public class CreateEntryByAccountIdUseCase : ICreateEntryByAccountIdUseCase {
  private readonly IMapper _mapper;
  private readonly IUnitOfWork _unitOfWork;
  private readonly ILedgerEntriesWriteOnlyRepository _ledgerEntriesWriteRepository;
  private readonly IAccountBalanceUpdateOnlyRepository _accountBalanceUpdateRepository;
  private readonly IIdempotencyRecordWriteOnlyRepository _idempotencyRecordWriteRepository;

  public CreateEntryByAccountIdUseCase(
    IMapper mapper,
    IUnitOfWork unitOfWork,
    ILedgerEntriesWriteOnlyRepository ledgerEntriesWriteRepository,
    IAccountBalanceUpdateOnlyRepository accountBalanceUpdateRepository,
    IIdempotencyRecordWriteOnlyRepository idempotencyRecordWriteRepository
  ) {
    this._mapper = mapper;
    this._unitOfWork = unitOfWork;
    this._ledgerEntriesWriteRepository = ledgerEntriesWriteRepository;
    this._accountBalanceUpdateRepository = accountBalanceUpdateRepository;
    this._idempotencyRecordWriteRepository = idempotencyRecordWriteRepository;
  }

  public async Task<ResponseLedgerEntry> Execute(long accountId, RequestCreateEntry request) {
    this.ValidateRequest(request);

    LedgerEntry? ledger = null;

    await this._unitOfWork.ExecuteInTransaction(async () => {
      ledger = await this.ExecuteInTransaction(accountId, request);
    });

    return this._mapper.Map<ResponseLedgerEntry>(ledger!);
  }

  private void ValidateRequest(RequestCreateEntry request) {
    var validator = new EntryValidator();
    var result = validator.Validate(request);

    if (!result.IsValid) {
      var errorMessage = result.Errors.Select(error => error.ErrorMessage).ToList();
      throw new ValidationException(errorMessage[0]);
    }
  }

  private async Task<LedgerEntry> ExecuteInTransaction(long accountId, RequestCreateEntry request) {
    var idempotency = new IdempotencyRecord {
      AccountId = accountId,
      IdempotencyKey = request.IdempotencyKey
    };

    await _idempotencyRecordWriteRepository.TryAdd(idempotency);
    await _unitOfWork.Flush();

    var success = false;

    if (request.Type == LedgerEntryType.DEBIT) {
      success = await _accountBalanceUpdateRepository.TryDebit(accountId, request.Amount);
    } else {
      success = await _accountBalanceUpdateRepository.Credit(accountId, request.Amount);
    }

    if (!success) {
      throw new ValidationException(ResourceErrorMessages.INSUFFICIENT_BALANCE);
    }

    var ledger = _mapper.Map<LedgerEntry>(request);
    ledger.AccountId = accountId;

    await _ledgerEntriesWriteRepository.Add(ledger);

    idempotency.LedgerEntry = ledger;

    return ledger;
  }
}
