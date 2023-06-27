using BusinessLayer.Cache.Interfaces;
using BusinessLayer.Exceptions;
using BusinessLayer.Interfaces;
using DataLayer.ApiLayer;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace BusinessLayer;

/// <summary>
/// API Layer Http client 
/// </summary>
public class ApiLayerHttpClient : IApiLayerHttpClient
{
    private const string USD = "USD";
    private readonly HttpClient _httpClient;
    private readonly IApiLayerSettings _settings;
    private readonly ICacheService _cache;
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<ApiLayerHttpClient> _logger;

    private const string ExchangeTemplateUrl = "{0}fixer/{1}?base=USD&symbols={2}";

    /// <summary>
    /// Initialize a new instance of <see cref="ApiLayerHttpClient"/>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="httpClient"></param>
    /// <param name="settings"> api layers settings</param>
    /// <param name="cache">redis cache</param>
    public ApiLayerHttpClient(ILogger<ApiLayerHttpClient> logger,
        HttpClient httpClient,
        IApiLayerSettings settings,
        ICacheService cache)
    {
        _logger = logger;
        _httpClient = httpClient;
        _settings = settings;

        _httpClient.BaseAddress = new Uri(_settings.Url);
        _httpClient.DefaultRequestHeaders.Add("apikey", _settings.ApiKey);

        _cache = cache;
    }

    /// <summary>
    /// Request to the service to obtain data on US dollar exchange rates
    /// </summary>
    /// <param name="date">course date</param>
    /// <returns>exchange rates</returns>
    /// <exception cref="ApplicationException">Invalid data format or receive error</exception>
    public async Task<ExchangeRates?> GetExchangeRatesAsync(DateTime date)
    {
        var dateKey = date.ToString("yyyy-MM-dd");
        var cacheKey = Consts.GetCacheKey(USD, dateKey);

        return await _cache.GetAsync<ExchangeRates>(
            cacheKey,
            async () =>
            {
                var uri = new Uri(string.Format(ExchangeTemplateUrl, _settings.Url, dateKey, Consts.UsdExchangeMoney));
                try
                {
                    var ret = await _httpClient.GetFromJsonAsync<ExchangeRates>(uri);

                    if (ret is null or { Success: false } or { Rates: null })
                    {
                        _logger.LogError(ApiHttpClientException.Error_FromAPI(date));
                        throw new ApiHttpClientException(ApiHttpClientException.Error_FromAPI(date));
                    }

                    return ret;
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, $"Error API Layer For date: {date}");
                    throw;
                }
            });
    }
}
