using AutoMapper;
using FinancialLedger.Communication.Response;

namespace FinancialLedger.Application.AutoMapper;

public class AutoMapping : Profile {
  public AutoMapping() {
    this.EntityToResponse();
  }

  private void EntityToResponse() {
    this.CreateMap<Domain.Entities.Account, ResponseAccount>();
  }
}
