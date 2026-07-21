using UnitConversion.Application.Catalog;
using UnitConversion.Application.Converters;
using UnitConversion.Domain.Exceptions;

namespace UnitConversion.Application.Services;

public sealed class ConversionService : IConversionService
{
    private readonly IReadOnlyCollection<IUnitConverter> _converters;

    // All registered converters are injected — adding a new category means
    // registering one more IUnitConverter in DI, nothing here changes.
    public ConversionService(IEnumerable<IUnitConverter> converters)
    {
        _converters = converters.ToList();
    }

    public decimal Convert(string fromUnitCode, string toUnitCode, decimal value)
    {
        var fromUnit = UnitCatalog.Find(fromUnitCode) ?? throw new UnknownUnitException(fromUnitCode);
        var toUnit = UnitCatalog.Find(toUnitCode) ?? throw new UnknownUnitException(toUnitCode);

        if (fromUnit.Category != toUnit.Category)
            throw new IncompatibleUnitsException(fromUnitCode, toUnitCode);

        var converter = _converters.FirstOrDefault(c => c.Category == fromUnit.Category)
            ?? throw new InvalidOperationException($"No converter registered for category '{fromUnit.Category}'.");

        return converter.Convert(fromUnitCode, toUnitCode, value);
    }
}