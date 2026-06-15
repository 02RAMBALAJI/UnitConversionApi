using UnitConversionApi.Models;

namespace UnitConversionApi.Services;

/// <summary>
/// Provides access to the registry of available units and categories.
/// Implementations can be backed by hardcoded data, a database, or an external service.
/// </summary>
public interface IUnitRegistryService
{
    /// <summary>Returns a unit by its ID (case-insensitive), or null if not found.</summary>
    Unit? GetUnit(string unitId);

    /// <summary>Returns every registered unit.</summary>
    IReadOnlyList<Unit> GetAllUnits();

    /// <summary>Returns all categories, each containing their member units.</summary>
    IReadOnlyList<UnitCategory> GetCategories();

    /// <summary>Returns true if a unit with the given ID exists.</summary>
    bool UnitExists(string unitId);
}
