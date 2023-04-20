namespace BusinessLayer.Interfaces;

/// <summary>
/// Settings for the Api Layer service.
/// </summary>
public interface IApiLayerSettings
{
    /// <summary>
    /// Api url to access exchange rates
    /// </summary>
    string Url { get; set; }
    /// <summary>
    /// Api user key 
    /// </summary>
    string ApiKey { get; set; }
}
