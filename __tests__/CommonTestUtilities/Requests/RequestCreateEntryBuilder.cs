using Bogus;
using FinancialLedger.Communication.Enums;
using FinancialLedger.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestCreateEntryBuilder {
  public static RequestCreateEntry Build() {
    return new Faker<RequestCreateEntry>()
      .RuleFor(entry => entry.Description, faker => faker.Commerce.ProductDescription())
      .RuleFor(entry => entry.Amount, faker => faker.Finance.Amount())
      .RuleFor(entry => entry.IdempotencyKey, _ => Guid.NewGuid())
      .RuleFor(entry => entry.Type, faker => faker.PickRandom<LedgerEntryType>());
  }
}
