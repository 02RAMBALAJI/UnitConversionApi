using UnitConversionApi.Models;

namespace UnitConversionApi.Data;

/// <summary>
/// Central store of all unit definitions and conversion factors.
/// 
/// Design note: Data is hardcoded as per requirements. To scale to hundreds of units,
/// replace this class with a database-backed implementation of IUnitRegistryService
/// without changing any other code.
/// 
/// Conversion strategy:
///   - Length  : convert via the base unit "meter"   (factor = unit / meter)
///   - Weight  : convert via the base unit "kilogram" (factor = unit / kilogram)
///   - Temperature: uses affine formulas (not a simple factor), handled in UnitConversionService
/// </summary>
public static class UnitDefinitions
{
    public static readonly IReadOnlyList<Unit> All = new List<Unit>
    {
        // ── Length ──────────────────────────────────────────────────
        new() { Id = "meter",       Name = "Meter",       Symbol = "m",   Category = "length" },
        new() { Id = "kilometer",   Name = "Kilometer",   Symbol = "km",  Category = "length" },
        new() { Id = "centimeter",  Name = "Centimeter",  Symbol = "cm",  Category = "length" },
        new() { Id = "millimeter",  Name = "Millimeter",  Symbol = "mm",  Category = "length" },
        new() { Id = "mile",        Name = "Mile",        Symbol = "mi",  Category = "length" },
        new() { Id = "yard",        Name = "Yard",        Symbol = "yd",  Category = "length" },
        new() { Id = "foot",        Name = "Foot",        Symbol = "ft",  Category = "length" },
        new() { Id = "inch",        Name = "Inch",        Symbol = "in",  Category = "length" },
        new() { Id = "nautical_mile", Name = "Nautical Mile", Symbol = "nmi", Category = "length" },

        // ── Temperature ──────────────────────────────────────────────
        new() { Id = "celsius",     Name = "Celsius",     Symbol = "°C",  Category = "temperature" },
        new() { Id = "fahrenheit",  Name = "Fahrenheit",  Symbol = "°F",  Category = "temperature" },
        new() { Id = "kelvin",      Name = "Kelvin",      Symbol = "K",   Category = "temperature" },

        // ── Weight / Mass ────────────────────────────────────────────
        new() { Id = "kilogram",    Name = "Kilogram",    Symbol = "kg",  Category = "weight" },
        new() { Id = "gram",        Name = "Gram",        Symbol = "g",   Category = "weight" },
        new() { Id = "milligram",   Name = "Milligram",   Symbol = "mg",  Category = "weight" },
        new() { Id = "pound",       Name = "Pound",       Symbol = "lb",  Category = "weight" },
        new() { Id = "ounce",       Name = "Ounce",       Symbol = "oz",  Category = "weight" },
        new() { Id = "ton",         Name = "Metric Ton",  Symbol = "t",   Category = "weight" },
        new() { Id = "stone",       Name = "Stone",       Symbol = "st",  Category = "weight" },
        new() { Id = "microgram",   Name = "Microgram",   Symbol = "µg",  Category = "weight" },
    };

    /// <summary>
    /// How many meters equal one unit of the keyed unit.
    /// e.g. LengthToMeter["kilometer"] = 1000 means 1 km = 1000 m.
    /// </summary>
    public static readonly IReadOnlyDictionary<string, double> LengthToMeter =
        new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
        {
            ["meter"]         = 1.0,
            ["kilometer"]     = 1_000.0,
            ["centimeter"]    = 0.01,
            ["millimeter"]    = 0.001,
            ["mile"]          = 1_609.344,
            ["yard"]          = 0.9144,
            ["foot"]          = 0.3048,
            ["inch"]          = 0.0254,
            ["nautical_mile"] = 1_852.0,
        };

    /// <summary>
    /// How many kilograms equal one unit of the keyed unit.
    /// e.g. WeightToKilogram["pound"] = 0.453592 means 1 lb = 0.453592 kg.
    /// </summary>
    public static readonly IReadOnlyDictionary<string, double> WeightToKilogram =
        new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
        {
            ["kilogram"]   = 1.0,
            ["gram"]       = 0.001,
            ["milligram"]  = 0.000_001,
            ["microgram"]  = 0.000_000_001,
            ["pound"]      = 0.453_592_37,
            ["ounce"]      = 0.028_349_523,
            ["ton"]        = 1_000.0,
            ["stone"]      = 6.350_293_18,
        };
}
