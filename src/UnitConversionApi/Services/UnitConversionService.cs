using UnitConversionApi.Data;
using UnitConversionApi.Models;

namespace UnitConversionApi.Services;

/// <summary>
/// Converts values between units using the base-unit strategy for linear categories
/// and dedicated formula handling for non-linear categories (temperature).
/// </summary>
public class UnitConversionService : IUnitConversionService
{
    private readonly IUnitRegistryService _registry;

    public UnitConversionService(IUnitRegistryService registry)
    {
        _registry = registry;
    }

    /// <inheritdoc />
    public ConversionResponse Convert(ConversionRequest request)
    {
        var fromUnit = _registry.GetUnit(request.FromUnit)
            ?? throw new ArgumentException($"Unknown unit: '{request.FromUnit}'. " +
               "Call GET /api/units for a list of valid unit IDs.");

        var toUnit = _registry.GetUnit(request.ToUnit)
            ?? throw new ArgumentException($"Unknown unit: '{request.ToUnit}'. " +
               "Call GET /api/units for a list of valid unit IDs.");

        if (!string.Equals(fromUnit.Category, toUnit.Category, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Cannot convert '{fromUnit.Name}' ({fromUnit.Category}) " +
                $"to '{toUnit.Name}' ({toUnit.Category}). " +
                "Both units must belong to the same category.");
        }

        double result = fromUnit.Category.ToLowerInvariant() switch
        {
            "length"      => ConvertLength(request.Value, fromUnit.Id, toUnit.Id),
            "temperature" => ConvertTemperature(request.Value, fromUnit.Id, toUnit.Id),
            "weight"      => ConvertWeight(request.Value, fromUnit.Id, toUnit.Id),
            var cat       => throw new NotSupportedException(
                                 $"Category '{cat}' is not yet supported.")
        };

        return new ConversionResponse
        {
            InputValue  = request.Value,
            FromUnit    = fromUnit.Name,
            OutputValue = Math.Round(result, 10),
            ToUnit      = toUnit.Name,
            Category    = fromUnit.Category
        };
    }

    // ──────────────────────────────────────────────────────────────────────────
    // Private helpers
    // ──────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Converts using the two-step base-unit strategy:
    ///   value (from) → meters → value (to)
    /// </summary>
    private static double ConvertLength(double value, string from, string to)
    {
        var toMeter = UnitDefinitions.LengthToMeter;

        if (!toMeter.TryGetValue(from, out double fromFactor))
            throw new ArgumentException($"No length conversion factor for '{from}'.");
        if (!toMeter.TryGetValue(to, out double toFactor))
            throw new ArgumentException($"No length conversion factor for '{to}'.");

        return value * fromFactor / toFactor;
    }

    /// <summary>
    /// Temperature conversion uses affine (offset + scale) formulas.
    /// Strategy: convert to Celsius first, then to the target unit.
    /// </summary>
    private static double ConvertTemperature(double value, string from, string to)
    {
        // Step 1 — normalise to Celsius
        double celsius = from.ToLowerInvariant() switch
        {
            "celsius"     => value,
            "fahrenheit"  => (value - 32.0) * 5.0 / 9.0,
            "kelvin"      => value - 273.15,
            _             => throw new ArgumentException($"Unknown temperature unit: '{from}'.")
        };

        // Step 2 — convert from Celsius to target
        return to.ToLowerInvariant() switch
        {
            "celsius"     => celsius,
            "fahrenheit"  => celsius * 9.0 / 5.0 + 32.0,
            "kelvin"      => celsius + 273.15,
            _             => throw new ArgumentException($"Unknown temperature unit: '{to}'.")
        };
    }

    /// <summary>
    /// Converts using the two-step base-unit strategy:
    ///   value (from) → kilograms → value (to)
    /// </summary>
    private static double ConvertWeight(double value, string from, string to)
    {
        var toKg = UnitDefinitions.WeightToKilogram;

        if (!toKg.TryGetValue(from, out double fromFactor))
            throw new ArgumentException($"No weight conversion factor for '{from}'.");
        if (!toKg.TryGetValue(to, out double toFactor))
            throw new ArgumentException($"No weight conversion factor for '{to}'.");

        return value * fromFactor / toFactor;
    }
}
