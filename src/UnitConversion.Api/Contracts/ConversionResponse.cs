namespace UnitConversion.Api.Contracts;

public sealed class ConversionResponse
{
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public decimal InputValue { get; set; }
    public decimal ConvertedValue { get; set; }
}