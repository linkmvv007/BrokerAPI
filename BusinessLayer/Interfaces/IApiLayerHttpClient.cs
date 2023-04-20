using DataLayer.ApiLayer;

namespace BusinessLayer.Interfaces;

/// <summary>
/// Interface for interacting with the service APILayer
/// <see cref="ApiLayerHttpClient"/>
/// </summary>
public interface IApiLayerHttpClient
{
    /// <summary>
    /// Requests courses for a date
    /// </summary>
    /// <param name="date">exchange date</param>
    /// <returns></returns>
    Task<ExchangeRates?> GetExchangeRatesAsync(DateTime date);
}
