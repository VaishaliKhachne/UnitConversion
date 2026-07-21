using System.ComponentModel.DataAnnotations;

namespace UnitConversion.Api.Contracts;

public sealed class ConversionRequest
{
    [Required]
    public string From { get; set; } = string.Empty;

    [Required]
    public string To { get; set; } = string.Empty;
    public decimal Value { get; set; }
}