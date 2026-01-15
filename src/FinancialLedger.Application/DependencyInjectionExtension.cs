using FinancialLedger.Application.AutoMapper;
using FinancialLedger.Application.UseCases.Account.Create;
using FinancialLedger.Application.UseCases.Account.CreateEntriesByAccountId;
using FinancialLedger.Application.UseCases.Account.GetById;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialLedger.Application;

public static class DependencyInjectionExtension {
  public static void AddApplication(this IServiceCollection services) {
    DependencyInjectionExtension.AddUseCases(services);
    DependencyInjectionExtension.AddAutoMapper(services);
  }

  private static void AddUseCases(IServiceCollection services) {
    services.AddScoped<ICreateAccountUseCase, CreateAccountUseCase>();
    services.AddScoped<IGetAccountByIdUseCase, GetAccountByIdUseCase>();
    services.AddScoped<ICreateEntryByAccountIdUseCase, CreateEntryByAccountIdUseCase>();
  }

  private static void AddAutoMapper(IServiceCollection services) {
    services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapping>());
  }
}
