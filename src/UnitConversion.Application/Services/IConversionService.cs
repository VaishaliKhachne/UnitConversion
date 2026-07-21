namespace UnitConversion.Application.Services;

public interface IConversionService
{
    decimal Convert(string fromUnitCode, string toUnitCode, decimal value);
}