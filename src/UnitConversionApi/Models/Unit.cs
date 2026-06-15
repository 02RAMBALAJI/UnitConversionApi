namespace UnitConversionApi.Models;

/// <summary>
/// Describes a single unit of measurement.
/// </summary>
public class Unit
{
    /// <summary>Unique lowercase identifier used in API requests (e.g. "meter", "celsius").</summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>Human-readable name (e.g. "Meter", "Celsius").</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Standard symbol (e.g. "m", "°C", "kg").</summary>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>Measurement category this unit belongs to (e.g. "length", "temperature", "weight").</summary>
    public string Category { get; set; } = string.Empty;
}
