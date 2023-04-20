using BusinessLayer.Contexts;
using BusinessLayer.Mediator;
using DataLayer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BrokerAPI.Controllers;

/// <summary>
/// Bad Broker controller API
/// </summary>
[Route("[controller]")]
[ApiController]
public class RatesController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initialize a new instance of <see cref="RatesController"/>
    /// </summary>
    /// <param name="mediator"></param>
    public RatesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get the best exchange currency for maximum revenue in the selected time period
    /// </summary>
    /// <param name="context"><see cref="BestContext"/></param>
    /// <returns>Exchange rate information and the best currency to exchange</returns>
    [HttpGet("best")]
    public async Task<OutputExchangeRates> Best([FromQuery] BestContext context) =>
        await _mediator.Send(new GetRatesQuery
        {
            Context = context
        });
}
