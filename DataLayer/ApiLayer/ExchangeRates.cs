using System.Text.Json.Serialization;

namespace DataLayer.ApiLayer;

/// <summary>
/// Data structure for exchange rates from API Layer 
/// </summary>
public sealed class ExchangeRates
{
    /// <summary>
    /// Query execution result
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    /// <summary>
    /// Request time
    /// </summary>
    [JsonPropertyName("timestamp")]
    public int Timestamp { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("historical")]
    public bool Historical { get; set; }

    /// <summary>
    /// Rates against that currency
    /// </summary>
    [JsonPropertyName("base")]
    public required string Base { get; set; }

    /// <summary>
    /// Date of exchange rates
    /// </summary>
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    /// <summary>
    /// Exchange rates
    /// </summary>
    [JsonPropertyName("rates")]
    public required Rates Rates { get; init; }
}
