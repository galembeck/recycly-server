using API.Public.DTOs.CooperativeAuth;
using API.Public.Validators._Base;
using FluentValidation;

namespace API.Public.Validators.CooperativeAuth;

public class CooperativeRegisterValidator : BaseValidator<CooperativeRegisterDTO>
{
    public CooperativeRegisterValidator()
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
