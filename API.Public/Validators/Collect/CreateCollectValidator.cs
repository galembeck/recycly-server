using API.Public.DTOs;
using API.Public.Validators._Base;
using FluentValidation;

namespace API.Public.Validators;

public class CreateCollectValidator : BaseValidator<CreateCollectDTO>
{
    public CreateCollectValidator()
    {
        RuleFor(m => m.CollectionPointId)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL");

        RuleFor(m => m.MaterialId)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL");

        RuleFor(m => m.WeightKg)
            .GreaterThan(0).WithMessage("WEIGHT_MUST_BE_POSITIVE");

        RuleFor(m => m.CollectedAt)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY");
    }
}
