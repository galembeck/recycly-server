using API.Public.DTOs.CooperativeAuth;
using API.Public.Validators._Base;
using FluentValidation;

namespace API.Public.Validators.CooperativeAuth;

public class CooperativePasswordRecoveryVerifyValidator : BaseValidator<CooperativePasswordRecoveryVerifyDTO>
{
    public CooperativePasswordRecoveryVerifyValidator()
    {
        RuleFor(m => m.Email)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .EmailAddress().WithMessage("INVALID_EMAIL");

        RuleFor(m => m.Token)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .Length(6, 10).WithMessage("INVALID_LENGHT");
    }
}
