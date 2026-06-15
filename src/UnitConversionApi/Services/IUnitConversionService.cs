using UnitConversionApi.Models;

namespace UnitConversionApi.Services;

/// <summary>
/// Converts a numerical value from one unit to another.
/// </summary>
public interface IUnitConversionService
{
    /// <summary>
    /// Performs the conversion described by <paramref name="request"/>.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Thrown when an unknown unit ID is supplied.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when <see cref="ConversionRequest.FromUnit"/> and
    /// <see cref="ConversionRequest.ToUnit"/> belong to different categories.
    /// </exception>
    ConversionResponse Convert(ConversionRequest request);
}
