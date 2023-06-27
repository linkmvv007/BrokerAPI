using BusinessLayer.Contexts;
using BusinessLayer.Exceptions;
using BusinessLayer.Interfaces;
using DataLayer;
using DataLayer.ApiLayer;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Mediator;

/// <summary>
/// Query that get the exchange currency
/// </summary>
public class GetRatesQuery : IRequest<OutputExchangeRates>
{
    /// <summary>
    /// Input parameters
    /// </summary>
    public required BestContext Context { get; set; }
}

/// <summary>
/// Query  handler for <see cref="GetRatesQuery"/> 
/// </summary>
public class GetRatesHandler : IRequestHandler<GetRatesQuery, OutputExchangeRates>
{
    private readonly IApiLayerHttpClient _httpClient;
    private readonly IMediator _mediator;
    private readonly ILogger<GetRatesHandler> _logger;

    /// <summary>
    /// Initialize a new instance of <see cref="GetRatesHandler"/>
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="mediator"></param>
    /// <param name="logger"></param>
    public GetRatesHandler(
        IApiLayerHttpClient httpClient,
        IMediator mediator,
        ILogger<GetRatesHandler> logger)
    {
        _mediator = mediator;
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// GetRatesQuery request handler
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<OutputExchangeRates> Handle(GetRatesQuery request, CancellationToken cancellationToken)
    {
        var startDate = request.Context!.startDate;
        var endDate = request.Context!.endDate;
        var dollarAmount = request.Context!.moneyUsd;

        TimeSpan difference = endDate.Date.Subtract(startDate.Date);
        int days = difference.Days;

        var tasks = Enumerable.Range(0, days + 1)
            .Select(i => _httpClient.GetExchangeRatesAsync(startDate.Date.AddDays(i)))
            .ToList();

        var responses = await Task.WhenAll(tasks);

        ExchangeRates[] data = responses!.Where(x => x is not null).ToArray();

        if (data is not { Length: > 0 })
        {
            _logger.LogError(ApiHttpClientException.Error_NoDataRates);
            throw new ApiHttpClientException(ApiHttpClientException.Error_NoDataRates);
        }

        return await _mediator.Send(new CalculateBestRevenueQuery
        {
            ExchangeRates = data,
            DollarAmount = dollarAmount,
            EndDate = endDate,
            StartDate = startDate
        });
    }
}