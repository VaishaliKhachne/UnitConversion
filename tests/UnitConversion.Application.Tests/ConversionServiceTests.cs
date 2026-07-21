using UnitConversion.Application.Converters;
using UnitConversion.Application.Services;
using UnitConversion.Domain.Exceptions;
using Xunit;

namespace UnitConversion.Application.Tests;

public class ConversionServiceTests
{
    private readonly IConversionService _sut;

    public ConversionServiceTests()
    {
        _sut = new ConversionService(new IUnitConverter[]
        {
            new LengthConverter(),
            new WeightConverter(),
            new TemperatureConverter()
        });
    }

    [Theory]
    [InlineData("m", "ft", 1, 3.28084)]
    [InlineData("km", "mi", 1, 0.621371)]
    public void Converts_length_correctly(string from, string to, decimal value, double expected)
    {
        var result = _sut.Convert(from, to, value);
        Assert.Equal(expected, (double)result, precision: 3);
    }

    [Theory]
    [InlineData("kg", "lb", 1, 2.20462)]
    public void Converts_weight_correctly(string from, string to, decimal value, double expected)
    {
        var result = _sut.Convert(from, to, value);
        Assert.Equal(expected, (double)result, precision: 3);
    }

    [Theory]
    [InlineData("c", "f", 0, 32)]
    [InlineData("c", "k", 0, 273.15)]
    [InlineData("f", "c", 212, 100)]
    public void Converts_temperature_correctly(string from, string to, decimal value, double expected)
    {
        var result = _sut.Convert(from, to, value);
        Assert.Equal(expected, (double)result, precision: 2);
    }

    [Fact]
    public void Throws_when_unit_unknown()
    {
        Assert.Throws<UnknownUnitException>(() => _sut.Convert("xx", "m", 1));
    }

    [Fact]
    public void Throws_when_categories_incompatible()
    {
        Assert.Throws<IncompatibleUnitsException>(() => _sut.Convert("m", "kg", 1));
    }
}