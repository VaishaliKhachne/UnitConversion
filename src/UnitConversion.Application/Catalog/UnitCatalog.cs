using UnitConversion.Domain;

namespace UnitConversion.Application.Catalog;

/// <summary>
/// Single source of truth for which units exist. In a future version this
/// would be backed by a database/repository instead of an in-memory list —
/// nothing outside this class needs to know that.
/// </summary>
public static class UnitCatalog
{
    public static readonly IReadOnlyList<UnitDefinition> All = new List<UnitDefinition>
    {
        // Length (base unit: meter)
        new("m",  "Meter",     ConversionCategory.Length),
        new("km", "Kilometer", ConversionCategory.Length),
        new("cm", "Centimeter",ConversionCategory.Length),
        new("mm", "Millimeter",ConversionCategory.Length),
        new("ft", "Foot",      ConversionCategory.Length),
        new("in", "Inch",      ConversionCategory.Length),
        new("mi", "Mile",      ConversionCategory.Length),
        new("yd", "Yard",      ConversionCategory.Length),

        // Weight (base unit: kilogram)
        new("kg", "Kilogram",  ConversionCategory.Weight),
        new("g",  "Gram",      ConversionCategory.Weight),
        new("lb", "Pound",     ConversionCategory.Weight),
        new("oz", "Ounce",     ConversionCategory.Weight),

        // Temperature
        new("c", "Celsius",    ConversionCategory.Temperature),
        new("f", "Fahrenheit", ConversionCategory.Temperature),
        new("k", "Kelvin",     ConversionCategory.Temperature),
    };

    public static UnitDefinition? Find(string code) =>
        All.FirstOrDefault(u => string.Equals(u.Code, code, StringComparison.OrdinalIgnoreCase));

    public static IEnumerable<UnitDefinition> ByCategory(ConversionCategory category) =>
        All.Where(u => u.Category == category);
}