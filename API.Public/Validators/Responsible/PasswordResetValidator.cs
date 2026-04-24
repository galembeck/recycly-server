using API.Public.DTOs.Auth;
using API.Public.Validators._Base;
using FluentValidation;

namespace API.Public.Validators.Responsible;

public class PasswordResetValidator : BaseValidator<PasswordResetDTO>
{
    public PasswordResetValidator()
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
