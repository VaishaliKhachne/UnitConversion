using UnitConversion.Domain;

namespace UnitConversion.Application.Converters;

/// <summary>Base unit: kilogram.</summary>
public sealed class WeightConverter : LinearUnitConverterBase
{
    public override ConversionCategory Category => ConversionCategory.Weight;

    protected override IReadOnlyDictionary<string, decimal> FactorsToBase { get; } =
        new Dictionary<string, decimal>
        {
            ["kg"] = 1m,
            ["g"] = 0.001m,
            ["lb"] = 0.45359237m,
            ["oz"] = 0.028349523125m,
        };
}