using sharedModels.Response.Enums;

namespace sharedModels.Response;

/// <summary>
/// Generic Response Model
/// </summary>
public class GenericResponse
{
    /// <summary>
    /// Status Response in String, can be as one of the following:
    /// - OK,
    /// - ERRORUPDATING,
    /// - ERRORINSERTING,
    /// - ERRORFETCHING,
    /// - INTERNALERROR
    /// </summary>
    public ResponseStatusEnum ResponseStatus { get; set; }
    /// <summary>
    /// Property to check if request was successfull
    /// </summary>
    public bool IsSuccess { get; set; }
    /// <summary>
    /// Message with information
    /// </summary>
    public string Message { get; set; } = default!;
}
