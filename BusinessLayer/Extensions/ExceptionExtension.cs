using System.Text;

namespace BusinessLayer.Extensions;

/// <summary>
///     Extension for working with exceptions
/// </summary>
public static class ExceptionExtension
{
    /// <summary>
    ///     Full message of exception
    /// </summary>
    public static string FullMessage(this Exception exception, bool needStackTrace = true)
    {
        var message = new StringBuilder();

        if (needStackTrace)
            message.AppendLine(exception.StackTrace);

        message.AppendLine(exception.Message);

        while (exception.InnerException != null)
        {
            exception = exception.InnerException;
            message.AppendLine(exception.Message);
        }

        return message.ToString();
    }
}
