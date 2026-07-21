using UnitConversion.Domain;

namespace UnitConversion.Application.Converters;

/// <summary>Base unit: meter.</summary>
public sealed class LengthConverter : LinearUnitConverterBase
{
    public override ConversionCategory Category => ConversionCategory.Length;

    protected override IReadOnlyDictionary<string, decimal> FactorsToBase { get; } =
        new Dictionary<string, decimal>
        {
            ["m"] = 1m,
            ["km"] = 1000m,
            ["cm"] = 0.01m,
            ["mm"] = 0.001m,
            ["ft"] = 0.3048m,
            ["in"] = 0.0254m,
            ["mi"] = 1609.344m,
            ["yd"] = 0.9144m,
        };
}