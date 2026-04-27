using API.Public.DTOs;
using API.Public.Validators._Base;
using FluentValidation;

namespace API.Public.Validators;

public class CreateCollectionPointValidator : BaseValidator<CreateCollectionPointDTO>
{
    public CreateCollectionPointValidator()
    {
        RuleFor(m => m.Name)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .Length(2, 150).WithMessage("INVALID_LENGHT");

        RuleFor(m => m.ZipCode)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .Length(8, 9).WithMessage("INVALID_ZIPCODE");

        RuleFor(m => m.Address)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY");

        RuleFor(m => m.Number)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY");

        RuleFor(m => m.Neighborhood)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY");

        RuleFor(m => m.City)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY");

        RuleFor(m => m.State)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .Length(2, 2).WithMessage("INVALID_STATE");

        RuleFor(m => m.Phone)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .MinimumLength(10).WithMessage("INVALID_PHONE");

        RuleFor(m => m.OpeningTime)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .Matches(@"^\d{2}:\d{2}$").WithMessage("INVALID_TIME_FORMAT");

        RuleFor(m => m.ClosingTime)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .Matches(@"^\d{2}:\d{2}$").WithMessage("INVALID_TIME_FORMAT");

        RuleFor(m => m.MaterialIds)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .Must(ids => ids.Count > 0).WithMessage("AT_LEAST_ONE_MATERIAL_REQUIRED");
    }
}
