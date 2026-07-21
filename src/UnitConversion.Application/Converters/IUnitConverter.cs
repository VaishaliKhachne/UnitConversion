using UnitConversion.Domain;

namespace UnitConversion.Application.Converters;

public interface IUnitConverter
{
    ConversionCategory Category { get; }

    /// <summary>Converts a value from one unit code to another within this converter's category.</summary>
    decimal Convert(string fromUnitCode, string toUnitCode, decimal value);
}