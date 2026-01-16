using AutoMapper;
using FinancialLedger.Communication.Requests;
using FinancialLedger.Communication.Response;
using FinancialLedger.Communication.Responses;
using FinancialLedger.Domain.Entities;

namespace FinancialLedger.Application.AutoMapper;

public class AutoMapping : Profile {
  public AutoMapping() {
    this.RequestToEntity();
    this.EntityToResponse();
  }

  private void RequestToEntity() {
    this.CreateMap<RequestCreateEntry, LedgerEntry>();
  }

  private void EntityToResponse() {
    this.CreateMap<Account, ResponseAccount>();
    this.CreateMap<LedgerEntry, ResponseLedgerEntry>();
  }
}
