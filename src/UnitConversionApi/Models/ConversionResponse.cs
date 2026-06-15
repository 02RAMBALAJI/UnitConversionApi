namespace UnitConversionApi.Models;

/// <summary>
/// Represents the result of a unit conversion.
/// </summary>
public class ConversionResponse
{
    /// <summary>The original input value.</summary>
    public double InputValue { get; set; }

    /// <summary>The name of the source unit.</summary>
    public string FromUnit { get; set; } = string.Empty;

    /// <summary>The converted output value.</summary>
    public double OutputValue { get; set; }

    /// <summary>The name of the target unit.</summary>
    public string ToUnit { get; set; } = string.Empty;

    /// <summary>The measurement category (e.g. "length", "temperature", "weight").</summary>
    public string Category { get; set; } = string.Empty;
}
