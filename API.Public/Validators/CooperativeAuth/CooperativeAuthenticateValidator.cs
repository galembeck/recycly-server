using API.Public.DTOs.CooperativeAuth;
using API.Public.Validators._Base;
using FluentValidation;

namespace API.Public.Validators.CooperativeAuth;

public class CooperativeAuthenticateValidator : BaseValidator<CooperativeAuthenticateDTO>
{
    public CooperativeAuthenticateValidator()
    {
        RuleFor(m => m.Email)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .EmailAddress().WithMessage("INVALID_EMAIL");

        RuleFor(m => m.Password)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .Length(6, 30).WithMessage("INVALID_LENGHT");
    }
}
