using API.Public.DTOs.CooperativeAuth;
using API.Public.Validators._Base;
using FluentValidation;

namespace API.Public.Validators.CooperativeAuth;

public class CooperativePasswordResetValidator : BaseValidator<CooperativePasswordResetDTO>
{
    public CooperativePasswordResetValidator()
    {
        RuleFor(m => m.Email)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .EmailAddress().WithMessage("INVALID_EMAIL");

        RuleFor(m => m.Token)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .Length(6, 10).WithMessage("INVALID_LENGHT");

        RuleFor(m => m.NewPassword)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .Length(6, 30).WithMessage("INVALID_LENGHT");
    }
}
