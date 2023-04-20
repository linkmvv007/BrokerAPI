using BusinessLayer.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BusinessLayer.Settings;

internal class RedisSettings : IRedisSettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RedisSettings"/> class.
    /// </summary>
    /// <param name="config"></param>
    public RedisSettings(IConfiguration config)
    {
        Url = config["Redis:Url"];
        Prefix = config["Redis:RedisPrefix"];
    }
    public string Url { get; set; }
    public string Prefix { get; set; }
}
