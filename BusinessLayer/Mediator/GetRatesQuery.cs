using BusinessLayer.Contexts;
using BusinessLayer.Interfaces;
using DataLayer;
using MediatR;

namespace BusinessLayer.Mediator;

/// <summary>
/// Query that get the exchange currency
/// </summary>
public class GetRatesQuery : IRequest<OutputExchangeRates>
{
    /// <summary>
    /// Input parameters
    /// </summary>
    public BestContext Context { get; set; }
}

/// <summary>
/// Query  handler for <see cref="GetRatesQuery"/> 
/// </summary>
public class GetRatesHandler : IRequestHandler<GetRatesQuery, OutputExchangeRates>
{
    private readonly IApiLayerHttpClient _httpClient;
    private readonly IMediator _mediator;


    /// <summary>
    /// Initialize a new instance of <see cref="GetRatesHandler"/>
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="mediator"></param>
    public GetRatesHandler(IApiLayerHttpClient httpClient, IMediator mediator)
    {
        _mediator = mediator;
        _httpClient = httpClient;
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

        return await _mediator.Send(new CalculateBestRevenueQuery
        {
            ExchangeRates = responses,
            DollarAmount = dollarAmount,
            EndDate = endDate,
            StartDate = startDate
        });
    }
}