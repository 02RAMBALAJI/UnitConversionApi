using Microsoft.AspNetCore.Mvc;
using UnitConversionApi.Models;
using UnitConversionApi.Services;

namespace UnitConversionApi.Controllers;

/// <summary>
/// Performs unit conversions.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ConversionsController : ControllerBase
{
    private readonly IUnitConversionService _conversionService;

    public ConversionsController(IUnitConversionService conversionService)
    {
        _conversionService = conversionService;
    }

    /// <summary>
    /// Convert a value from one unit to another (POST).
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/conversions
    ///     {
    ///         "value": 100,
    ///         "fromUnit": "meter",
    ///         "toUnit": "foot"
    ///     }
    ///
    /// </remarks>
    /// <param name="request">Conversion parameters.</param>
    /// <returns>The converted value with metadata.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ConversionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
    public IActionResult Convert([FromBody] ConversionRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);

            return BadRequest(ApiResponse<object>.Fail(
                string.Join(" | ", errors)));
        }

        var result = _conversionService.Convert(request);
        return Ok(ApiResponse<ConversionResponse>.Ok(result));
    }

    /// <summary>
    /// Convert a value from one unit to another (GET — handy for quick browser tests).
    /// </summary>
    /// <param name="value">The numerical value to convert.</param>
    /// <param name="from">The source unit ID (e.g. "celsius").</param>
    /// <param name="to">The target unit ID (e.g. "fahrenheit").</param>
    /// <returns>The converted value with metadata.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<ConversionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
    public IActionResult ConvertGet(
        [FromQuery] double value,
        [FromQuery] string from,
        [FromQuery] string to)
    {
        var request = new ConversionRequest
        {
            Value    = value,
            FromUnit = from,
            ToUnit   = to
        };

        var result = _conversionService.Convert(request);
        return Ok(ApiResponse<ConversionResponse>.Ok(result));
    }
}
