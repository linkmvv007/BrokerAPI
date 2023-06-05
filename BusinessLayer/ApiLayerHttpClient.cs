﻿using BusinessLayer.Interfaces;
using DataLayer.ApiLayer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace BusinessLayer;

/// <summary>
/// API Layer Http client 
/// </summary>
public class ApiLayerHttpClient : IApiLayerHttpClient
{
    private const string USD = "USD";
    private readonly HttpClient _httpClient;
    private readonly IApiLayerSettings _settings;
    private readonly IDistributedCache _cache;
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<ApiLayerHttpClient> _logger;

    const string ExchangeTemplateUrl = "{0}fixer/{1}?base=USD&symbols={2}";

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
        IDistributedCache cache)
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

        var cacheData = await _cache.GetAsync(cacheKey);
        if (cacheData is not null)
        {
            return JsonSerializer.Deserialize<ExchangeRates>(cacheData);
        }

        var uri = new Uri(string.Format(ExchangeTemplateUrl, _settings.Url, dateKey, Consts.UsdExchangeMoney));

        ExchangeRates? ret;
        try
        {
            ret = await _httpClient.GetFromJsonAsync<ExchangeRates>(uri);

            if (ret is null || !ret.Success || ret.Rates is null)
            {
                _logger.LogError($"API Layer returned an error for date : {date}");
                throw new ApplicationException($"API Layer returned an error for date : {date}");
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, $"Error API Layer For date: {date}");
            throw;
        }

        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(ret), Consts.GetDistributedCacheEntryOptions);

        return ret;
    }
}