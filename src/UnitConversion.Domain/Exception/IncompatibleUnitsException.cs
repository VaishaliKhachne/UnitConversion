namespace UnitConversion.Domain.Exceptions;

public sealed class IncompatibleUnitsException : Exception
{
    public IncompatibleUnitsException(string fromUnit, string toUnit)
        : base($"Cannot convert between '{fromUnit}' and '{toUnit}' — they belong to different categories.")
    {
    }
}