using System.ComponentModel.DataAnnotations;

namespace UnitConversionApi.Models;

/// <summary>
/// Represents a request to convert a numerical value from one unit to another.
/// </summary>
public class ConversionRequest
{
    /// <summary>The numerical value to convert.</summary>
    /// <example>100</example>
    [Required]
    public double Value { get; set; }

    /// <summary>The source unit ID (e.g. "meter", "celsius", "kilogram").</summary>
    /// <example>meter</example>
    [Required]
    [MinLength(1)]
    public string FromUnit { get; set; } = string.Empty;

    /// <summary>The target unit ID (e.g. "foot", "fahrenheit", "pound").</summary>
    /// <example>foot</example>
    [Required]
    [MinLength(1)]
    public string ToUnit { get; set; } = string.Empty;
}
