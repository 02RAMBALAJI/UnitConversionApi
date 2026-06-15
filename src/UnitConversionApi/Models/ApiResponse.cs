namespace UnitConversionApi.Models;

/// <summary>
/// Uniform envelope for all API responses.
/// </summary>
/// <typeparam name="T">The type of the data payload.</typeparam>
public class ApiResponse<T>
{
    /// <summary>Indicates whether the request succeeded.</summary>
    public bool Success { get; set; }

    /// <summary>The response payload on success; null on failure.</summary>
    public T? Data { get; set; }

    /// <summary>A human-readable error message on failure; null on success.</summary>
    public string? Error { get; set; }

    /// <summary>Creates a successful response wrapping the given data.</summary>
    public static ApiResponse<T> Ok(T data) =>
        new() { Success = true, Data = data };

    /// <summary>Creates a failed response with the given error message.</summary>
    public static ApiResponse<T> Fail(string error) =>
        new() { Success = false, Error = error };
}
