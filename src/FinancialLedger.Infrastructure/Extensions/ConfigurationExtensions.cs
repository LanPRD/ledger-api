using Microsoft.Extensions.Configuration;

namespace FinancialLedger.Infrastructure.Extensions;

public static class ConfigurationExtensions {
  public static bool IsTestEnvironment(this IConfiguration configuration) {
    return configuration.GetValue<bool>("InMemoryTest");
  }
}