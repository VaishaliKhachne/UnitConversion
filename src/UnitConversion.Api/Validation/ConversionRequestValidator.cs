using FluentValidation;
using UnitConversion.Api.Contracts;

namespace UnitConversion.Api.Validation;

public sealed class ConversionRequestValidator : AbstractValidator<ConversionRequest>
{
    public ConversionRequestValidator()
    {
        RuleFor(x => x.From).NotEmpty().WithMessage("'from' unit is required.");
        RuleFor(x => x.To).NotEmpty().WithMessage("'to' unit is required.");
        //RuleFor(x => x.Value).InclusiveBetween(-1_000_000_000m, 1_000_000_000m).WithMessage("Value must be within a reasonable numeric range.");
    }
}