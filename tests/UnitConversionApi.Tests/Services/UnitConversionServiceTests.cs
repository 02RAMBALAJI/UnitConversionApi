using Xunit;
using FluentAssertions;
using UnitConversionApi.Models;
using UnitConversionApi.Services;

namespace UnitConversionApi.Tests.Services;

/// <summary>
/// Unit tests for UnitConversionService covering all three categories
/// and common error conditions.
/// </summary>
public class UnitConversionServiceTests
{
    // Use the real registry — it is pure, deterministic, and has no I/O.
    private readonly UnitConversionService _sut;

    public UnitConversionServiceTests()
    {
        var registry = new UnitRegistryService();
        _sut = new UnitConversionService(registry);
    }

    // ──────────────────────────────────────────────────────────────────────────
    // Length
    // ──────────────────────────────────────────────────────────────────────────

    [Theory]
    [InlineData(1,     "meter",      "foot",        3.28083989501312)]
    [InlineData(1,     "kilometer",  "meter",       1_000.0)]
    [InlineData(1,     "mile",       "kilometer",   1.609344)]
    [InlineData(100,   "centimeter", "meter",       1.0)]
    [InlineData(1,     "inch",       "centimeter",  2.54)]
    [InlineData(1,     "yard",       "foot",        3.0)]
    [InlineData(1,     "meter",      "meter",       1.0)]  // same unit → identity
    [InlineData(0,     "meter",      "foot",        0.0)]  // zero is always zero
    [InlineData(1,     "nautical_mile", "meter",    1_852.0)]
    public void Convert_Length_ShouldReturnCorrectResult(
        double input, string from, string to, double expected)
    {
        var request = new ConversionRequest { Value = input, FromUnit = from, ToUnit = to };

        var result = _sut.Convert(request);

        result.OutputValue.Should().BeApproximately(expected, precision: 1e-6);
        result.Category.Should().Be("length");
        result.InputValue.Should().Be(input);
    }

    [Fact]
    public void Convert_Length_NegativeValue_ShouldWork()
    {
        var request = new ConversionRequest { Value = -10, FromUnit = "meter", ToUnit = "foot" };
        var result = _sut.Convert(request);
        result.OutputValue.Should().BeApproximately(-32.8083989501312, precision: 1e-6);
    }

    // ──────────────────────────────────────────────────────────────────────────
    // Temperature
    // ──────────────────────────────────────────────────────────────────────────

    [Theory]
    [InlineData(0,      "celsius",     "fahrenheit",  32.0)]
    [InlineData(100,    "celsius",     "fahrenheit",  212.0)]
    [InlineData(32,     "fahrenheit",  "celsius",     0.0)]
    [InlineData(212,    "fahrenheit",  "celsius",     100.0)]
    [InlineData(0,      "celsius",     "kelvin",      273.15)]
    [InlineData(273.15, "kelvin",      "celsius",     0.0)]
    [InlineData(0,      "kelvin",      "celsius",     -273.15)]
    [InlineData(-40,    "celsius",     "fahrenheit",  -40.0)]  // magical equality point
    [InlineData(-40,    "fahrenheit",  "celsius",     -40.0)]
    [InlineData(37,     "celsius",     "fahrenheit",  98.6)]   // body temperature
    public void Convert_Temperature_ShouldReturnCorrectResult(
        double input, string from, string to, double expected)
    {
        var request = new ConversionRequest { Value = input, FromUnit = from, ToUnit = to };

        var result = _sut.Convert(request);

        result.OutputValue.Should().BeApproximately(expected, precision: 1e-6);
        result.Category.Should().Be("temperature");
    }

    [Fact]
    public void Convert_Temperature_SameUnit_ShouldBeIdentity()
    {
        var request = new ConversionRequest { Value = 100, FromUnit = "celsius", ToUnit = "celsius" };
        var result = _sut.Convert(request);
        result.OutputValue.Should().BeApproximately(100, precision: 1e-9);
    }

    // ──────────────────────────────────────────────────────────────────────────
    // Weight / Mass
    // ──────────────────────────────────────────────────────────────────────────

    [Theory]
    [InlineData(1,     "kilogram", "pound",     2.20462262)]
    [InlineData(1,     "pound",    "kilogram",  0.45359237)]
    [InlineData(1_000, "gram",     "kilogram",  1.0)]
    [InlineData(1,     "ton",      "kilogram",  1_000.0)]
    [InlineData(16,    "ounce",    "pound",     1.0)]
    [InlineData(1,     "stone",    "pound",     14.0)]
    public void Convert_Weight_ShouldReturnCorrectResult(
        double input, string from, string to, double expected)
    {
        var request = new ConversionRequest { Value = input, FromUnit = from, ToUnit = to };

        var result = _sut.Convert(request);

        result.OutputValue.Should().BeApproximately(expected, precision: 1e-4);
        result.Category.Should().Be("weight");
    }

    // ──────────────────────────────────────────────────────────────────────────
    // Case-insensitivity
    // ──────────────────────────────────────────────────────────────────────────

    [Theory]
    [InlineData("Meter",  "FOOT")]
    [InlineData("METER",  "foot")]
    [InlineData("mEtEr",  "Foot")]
    public void Convert_UnitIds_ShouldBeCaseInsensitive(string from, string to)
    {
        var request = new ConversionRequest { Value = 1, FromUnit = from, ToUnit = to };

        var act = () => _sut.Convert(request);

        act.Should().NotThrow();
    }

    // ──────────────────────────────────────────────────────────────────────────
    // Error cases
    // ──────────────────────────────────────────────────────────────────────────

    [Fact]
    public void Convert_UnknownFromUnit_ShouldThrowArgumentException()
    {
        var request = new ConversionRequest { Value = 1, FromUnit = "lightyear", ToUnit = "meter" };

        var act = () => _sut.Convert(request);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*lightyear*");
    }

    [Fact]
    public void Convert_UnknownToUnit_ShouldThrowArgumentException()
    {
        var request = new ConversionRequest { Value = 1, FromUnit = "meter", ToUnit = "fathom" };

        var act = () => _sut.Convert(request);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*fathom*");
    }

    [Fact]
    public void Convert_CrossCategory_ShouldThrowInvalidOperationException()
    {
        var request = new ConversionRequest { Value = 1, FromUnit = "meter", ToUnit = "kilogram" };

        var act = () => _sut.Convert(request);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*same category*");
    }

    [Fact]
    public void Convert_TemperatureToCrossCategory_ShouldThrowInvalidOperationException()
    {
        var request = new ConversionRequest { Value = 100, FromUnit = "celsius", ToUnit = "meter" };

        var act = () => _sut.Convert(request);

        act.Should().Throw<InvalidOperationException>();
    }

    // ──────────────────────────────────────────────────────────────────────────
    // Response shape
    // ──────────────────────────────────────────────────────────────────────────

    [Fact]
    public void Convert_ResponseShape_ShouldBeFullyPopulated()
    {
        var request = new ConversionRequest { Value = 5, FromUnit = "kilometer", ToUnit = "mile" };

        var result = _sut.Convert(request);

        result.InputValue.Should().Be(5);
        result.FromUnit.Should().Be("Kilometer");
        result.ToUnit.Should().Be("Mile");
        result.Category.Should().Be("length");
        result.OutputValue.Should().BeGreaterThan(0);
    }
}
