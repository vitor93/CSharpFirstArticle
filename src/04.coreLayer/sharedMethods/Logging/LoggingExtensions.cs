namespace sharedMethods.Logging;

public static class LoggingExtensions
{
    public static string GetGenericError(this Exception exception, string methodName)
    {
        return $"Error in method {methodName} with error: {exception.GetBaseException().Message}";
    }
}
