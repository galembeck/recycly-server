using API.Public.DTOs;
using API.Public.Validators._Base;
using FluentValidation;

namespace API.Public.Validators;

public class CreateMaterialValidator : BaseValidator<MaterialDTO>
{
    public CreateMaterialValidator()
    {
        RuleFor(m => m.Name)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .Length(2, 150).WithMessage("INVALID_LENGHT");

        RuleFor(m => m.Color)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .Matches(@"^#([0-9A-Fa-f]{3}|[0-9A-Fa-f]{6})$").WithMessage("INVALID_COLOR_FORMAT");
    }
}
