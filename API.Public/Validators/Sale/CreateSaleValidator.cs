using API.Public.DTOs;
using API.Public.Validators._Base;
using FluentValidation;

namespace API.Public.Validators;

public class CreateSaleValidator : BaseValidator<CreateSaleDTO>
{
    public CreateSaleValidator()
    {
        RuleFor(x => x.BuyerName)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .MaximumLength(200).WithMessage("INVALID_LENGHT");

        RuleFor(x => x.MaterialIds)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .Must(ids => ids != null && ids.Count > 0).WithMessage("MUST_HAVE_AT_LEAST_ONE_MATERIAL");

        RuleFor(x => x.WeightKg)
            .GreaterThan(0).WithMessage("MUST_BE_GREATER_THAN_ZERO");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("MUST_BE_GREATER_THAN_ZERO");

        RuleFor(x => x.SoldAt)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY");
    }
}
