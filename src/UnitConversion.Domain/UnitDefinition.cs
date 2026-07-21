namespace UnitConversion.Domain;

/// <summary>
/// Describes a single unit of measurement within a category.
/// Code is the canonical, case-insensitive identifier used in API calls (e.g. "m", "ft", "c").
/// </summary>
public sealed record UnitDefinition(string Code, string DisplayName, ConversionCategory Category);