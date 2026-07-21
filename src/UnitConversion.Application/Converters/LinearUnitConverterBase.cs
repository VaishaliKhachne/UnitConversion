using UnitConversion.Domain;
using UnitConversion.Domain.Exceptions;

namespace UnitConversion.Application.Converters;

/// <summary>
/// Base for any category where conversion is a pure multiplicative factor
/// against a common base unit (e.g. meter for length, kilogram for weight).
/// </summary>
public abstract class LinearUnitConverterBase : IUnitConverter
{
    public abstract ConversionCategory Category { get; }

    /// <summary>Maps each unit code to "how many base units does 1 of this unit equal".</summary>
    protected abstract IReadOnlyDictionary<string, decimal> FactorsToBase { get; }

    public decimal Convert(string fromUnitCode, string toUnitCode, decimal value)
    {
        var fromFactor = GetFactor(fromUnitCode);
        var toFactor = GetFactor(toUnitCode);

        var valueInBase = value * fromFactor;
        return valueInBase / toFactor;
    }

    private decimal GetFactor(string unitCode)
    {
        if (FactorsToBase.TryGetValue(unitCode.ToLowerInvariant(), out var factor))
            return factor;

        throw new UnknownUnitException(unitCode);
    }
}