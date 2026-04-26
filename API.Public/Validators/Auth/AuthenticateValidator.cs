using API.Public.DTOs;
using API.Public.Validators._Base;
using Domain.Utils;
using FluentValidation;

namespace API.Public.Validators;

public class AuthenticateValidator : BaseValidator<AuthenticateDTO>
{
    public AuthenticateValidator()
    {
        RuleFor(m => m.Identifier)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .Must(v => IsEmail(v) || IsCpf(v)).WithMessage("INVALID_EMAIL_OR_DOCUMENT");

        RuleFor(m => m.Password)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .Length(6, 50).WithMessage("INVALID_LENGHT");
    }

    private static bool IsEmail(string value) =>
        !string.IsNullOrWhiteSpace(value) &&
        new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(value);

    private static bool IsCpf(string value) =>
        !string.IsNullOrWhiteSpace(value) &&
        StringUtil.IsValidCPF(StringUtil.Slugify(value));
}