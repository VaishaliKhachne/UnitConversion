namespace UnitConversion.Domain.Exceptions;

public sealed class UnknownUnitException : Exception
{
    public string UnitCode { get; }

    public UnknownUnitException(string unitCode)
        : base($"Unit '{unitCode}' is not recognized.")
    {
        UnitCode = unitCode;
    }
}