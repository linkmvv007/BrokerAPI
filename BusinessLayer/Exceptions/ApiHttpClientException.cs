namespace BusinessLayer.Exceptions;

/// <summary>
/// Exception for API errors 
/// </summary>
public sealed class ApiHttpClientException : ApplicationException
{
    internal const string ErrorNoDataRates = "No data rates from API";

    /// <summary>
    /// Initialize a new instance of <see cref="ApiHttpClientException"/>
    /// </summary>
    /// <param name="message"></param>
    public ApiHttpClientException(string? message)
         : base(message)
    {
    }

    internal static string Error_FromAPI(DateTime dt) => $"API Layer returned an error for date: {dt}";
}
