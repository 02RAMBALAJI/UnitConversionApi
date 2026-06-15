namespace UnitConversionApi.Models;

/// <summary>
/// A grouping of related units (e.g. all length units).
/// </summary>
public class UnitCategory
{
    /// <summary>Unique lowercase category identifier (e.g. "length").</summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>Human-readable category name (e.g. "Length").</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>All units that belong to this category.</summary>
    public List<Unit> Units { get; set; } = new();
}
