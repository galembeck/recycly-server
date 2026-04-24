using API.Public.DTOs.CooperativeAuth;
using API.Public.Validators._Base;
using FluentValidation;

namespace API.Public.Validators.CooperativeAuth;

public class CooperativePasswordRecoveryRequestValidator : BaseValidator<CooperativePasswordRecoveryRequestDTO>
{
    public CooperativePasswordRecoveryRequestValidator()
    {
        RuleFor(m => m.Email)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .EmailAddress().WithMessage("INVALID_EMAIL");
    }
}
