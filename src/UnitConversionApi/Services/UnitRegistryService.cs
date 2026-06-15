using UnitConversionApi.Data;
using UnitConversionApi.Models;

namespace UnitConversionApi.Services;

/// <summary>
/// In-memory implementation of IUnitRegistryService backed by hardcoded UnitDefinitions.
/// Registered as a singleton — data never changes at runtime.
/// </summary>
public class UnitRegistryService : IUnitRegistryService
{
    private readonly Dictionary<string, Unit> _unitLookup;
    private readonly List<UnitCategory> _categories;

    public UnitRegistryService()
    {
        _unitLookup = UnitDefinitions.All
            .ToDictionary(u => u.Id, u => u, StringComparer.OrdinalIgnoreCase);

        _categories = UnitDefinitions.All
            .GroupBy(u => u.Category, StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key)
            .Select(g => new UnitCategory
            {
                Id    = g.Key,
                Name  = char.ToUpperInvariant(g.Key[0]) + g.Key[1..],
                Units = g.OrderBy(u => u.Name).ToList()
            })
            .ToList();
    }

    /// <inheritdoc />
    public Unit? GetUnit(string unitId) =>
        _unitLookup.TryGetValue(unitId, out var unit) ? unit : null;

    /// <inheritdoc />
    public IReadOnlyList<Unit> GetAllUnits() => UnitDefinitions.All;

    /// <inheritdoc />
    public IReadOnlyList<UnitCategory> GetCategories() => _categories;

    /// <inheritdoc />
    public bool UnitExists(string unitId) => _unitLookup.ContainsKey(unitId);
}
