using API.Public.DTOs.Auth;
using API.Public.Validators._Base;
using FluentValidation;

namespace API.Public.Validators.Auth;

public class RegisterValidator : BaseValidator<RegisterDTO>
{
    public RegisterValidator()
    {
        RuleFor(m => m.Name)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .Length(2, 100).WithMessage("INVALID_LENGHT");

        RuleFor(m => m.Email)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .EmailAddress().WithMessage("INVALID_EMAIL");

        RuleFor(m => m.Cpf)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .Length(11, 14).WithMessage("INVALID_LENGHT");

        RuleFor(m => m.Password)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .Length(6, 30).WithMessage("INVALID_LENGHT");

        RuleFor(m => m.BirthDate)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .Must(d => d < DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("INVALID_BIRTH_DATE");

        RuleFor(m => m.Phones)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL");
    }
}
