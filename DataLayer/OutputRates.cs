using DataLayer.ApiLayer;

namespace DataLayer;

/// <summary>
/// Part of the data for <see cref="OutputExchangeRates"/>
/// </summary>
public sealed class OutputRates : Rates
{
    /// <summary>
    /// Trading date 
    /// </summary>
    public DateTime Date { get; set; }
}
