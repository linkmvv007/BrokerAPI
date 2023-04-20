namespace BusinessLayer.Interfaces;

public interface IRedisSettings
{
    /// <summary>
    /// Redis connection string 
    /// </summary>
    string Url { get; set; }
    /// <summary>
    /// Database number
    /// </summary>
    string Prefix { get; set; }
}
