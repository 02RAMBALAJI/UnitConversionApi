using Microsoft.AspNetCore.Mvc;
using UnitConversionApi.Models;
using UnitConversionApi.Services;

namespace UnitConversionApi.Controllers;

/// <summary>
/// Exposes the unit registry — useful for callers to discover supported units.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UnitsController : ControllerBase
{
    private readonly IUnitRegistryService _registry;

    public UnitsController(IUnitRegistryService registry)
    {
        _registry = registry;
    }

    /// <summary>
    /// Returns all available units across all categories.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<Unit>>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(ApiResponse<IReadOnlyList<Unit>>.Ok(_registry.GetAllUnits()));
    }

    /// <summary>
    /// Returns all unit categories, each containing their member units.
    /// </summary>
    [HttpGet("categories")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<UnitCategory>>), StatusCodes.Status200OK)]
    public IActionResult GetCategories()
    {
        return Ok(ApiResponse<IReadOnlyList<UnitCategory>>.Ok(_registry.GetCategories()));
    }

    /// <summary>
    /// Returns a single unit by its ID.
    /// </summary>
    /// <param name="id">The unit ID (e.g. "meter", "celsius", "kilogram").</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<Unit>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public IActionResult GetById(string id)
    {
        var unit = _registry.GetUnit(id);
        if (unit is null)
            return NotFound(ApiResponse<object>.Fail(
                $"Unit '{id}' not found. Call GET /api/units for a list of valid IDs."));

        return Ok(ApiResponse<Unit>.Ok(unit));
    }
}
