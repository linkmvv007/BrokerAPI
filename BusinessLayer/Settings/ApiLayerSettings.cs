using BusinessLayer.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BusinessLayer.Settings;
#pragma warning disable CS8601
/// <summary>
/// Api Layer Settings
/// </summary>
internal class ApiLayerSettings : IApiLayerSettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiLayerSettings"/> class.
    /// </summary>
    /// <param name="config"></param>
    public ApiLayerSettings(IConfiguration config)
    {
        Url = config["ApiLayer:Url"];
        ApiKey = config["ApiLayer:ApiKey"];
    }

    /// <summary>
    /// Base Api url to access exchange rates
    /// </summary>
    public required string Url { get; set; }

    /// <summary>
    /// Api Layer user key 
    /// </summary>
    public required string ApiKey { get; set; }
}
