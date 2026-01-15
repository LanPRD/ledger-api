using FinancialLedger.Communication.Requests;
using FinancialLedger.Exception;
using FluentValidation;

namespace FinancialLedger.Application.UseCases;

public class EntryValidator : AbstractValidator<RequestCreateEntry> {
  public EntryValidator() {
    this.RuleFor(entry => entry.Amount).GreaterThan(0).WithMessage(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO);
    this.RuleFor(entry => entry.Type).IsInEnum().WithMessage(ResourceErrorMessages.INVALID_ENTRY_TYPE);
    this.RuleFor(entry => entry.IdempotencyKey).NotEmpty().WithMessage(ResourceErrorMessages.IDEMPOTENCY_KEY_REQUIRED);
  }
}
