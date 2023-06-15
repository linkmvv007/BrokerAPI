using System.Text.Json;
using System.Text.Json.Serialization;

namespace BusinessLayer.Helpers;

/// <summary>
///     Helper to work with json
/// </summary>
public static class JsonHelper
{
    static readonly JsonSerializerOptions options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    static JsonHelper()
    {
        options.Converters.Add(new JsonStringEnumConverter());
    }

    /// <summary>
    ///     Serialize from model T to JSON in string format
    /// </summary>
    public static string SerializeToString<T>(this T obj) where T : class
    {
        return JsonSerializer.Serialize(obj, options);
    }
}