using API.Public.DTOs.Auth;
using API.Public.Validators._Base;
using FluentValidation;

namespace API.Public.Validators.Responsible;

public class PasswordRecoveryRequestValidator : BaseValidator<PasswordRecoveryRequestDTO>
{
    public PasswordRecoveryRequestValidator()
    {
        RuleFor(m => m.Email)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .EmailAddress().WithMessage("INVALID_EMAIL");
    }
}
