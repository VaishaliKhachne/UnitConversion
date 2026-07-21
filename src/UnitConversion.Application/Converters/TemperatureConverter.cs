using UnitConversion.Domain;
using UnitConversion.Domain.Exceptions;

namespace UnitConversion.Application.Converters;

public sealed class TemperatureConverter : IUnitConverter
{
    public ConversionCategory Category => ConversionCategory.Temperature;

    public decimal Convert(string fromUnitCode, string toUnitCode, decimal value)
    {
        var celsius = ToCelsius(fromUnitCode, value);
        return FromCelsius(toUnitCode, celsius);
    }

    private static decimal ToCelsius(string unitCode, decimal value) => unitCode.ToLowerInvariant() switch
    {
        "c" => value,
        "f" => (value - 32m) * 5m / 9m,
        "k" => value - 273.15m,
        _ => throw new UnknownUnitException(unitCode)
    };

    private static decimal FromCelsius(string unitCode, decimal celsius) => unitCode.ToLowerInvariant() switch
    {
        "c" => celsius,
        "f" => celsius * 9m / 5m + 32m,
        "k" => celsius + 273.15m,
        _ => throw new UnknownUnitException(unitCode)
    };
}