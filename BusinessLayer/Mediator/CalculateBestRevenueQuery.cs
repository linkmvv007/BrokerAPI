using DataLayer;
using DataLayer.ApiLayer;
using MediatR;

namespace BusinessLayer.Mediator;

/// <summary>
/// Query that calculates the best revenue on currencies
/// </summary>
public class CalculateBestRevenueQuery : IRequest<OutputExchangeRates>
{
    /// <summary>
    /// Dollar exchange rates
    /// </summary>
    public required ExchangeRates[] ExchangeRates { get; init; }
    /// <summary>
    /// Start date of currency trading
    /// </summary>
    public DateTime StartDate { get; set; }
    /// <summary>
    /// End date of currency trading
    /// </summary>
    public DateTime EndDate { get; set; }
    /// <summary>
    /// Amount of dollars to exchange
    /// </summary>
    public int DollarAmount { get; init; }

}

/// <summary>
/// Query handler for <see cref="CalculateBestRevenueQuery"/>
/// </summary>
public class CalculateBestRevenueQueryHandler : IRequestHandler<CalculateBestRevenueQuery, OutputExchangeRates>
{
    /// <summary>
    /// Initialize a new instance of <see cref="CalculateBestRevenueQueryHandler"/>
    /// </summary>
    public CalculateBestRevenueQueryHandler()
    {
    }

    /// <summary>
    /// CalculateBestRevenueQuery request handler
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<OutputExchangeRates> Handle(CalculateBestRevenueQuery request, CancellationToken cancellationToken)
    {
        var result = new OutputExchangeRates
        {
            Rates = request.ExchangeRates
            .OrderBy(x => x.Date)
            .Select(data => new OutputRates
            {
                Date = data.Date,
                Eur = data.Rates.Eur,
                Gbp = data.Rates.Gbp,
                Jpy = data.Rates.Jpy,
                Rub = data.Rates.Rub,
            }).ToList()
        };

        var bestDates = await Task.Run(() => CalculateFunc(result.Rates, request.DollarAmount), cancellationToken);

        result.Revenue = bestDates.Revenue;
        result.BuyDate = bestDates.BuyDate;
        result.SellDate = bestDates.SellDate;
        result.Tool = bestDates.Tool;

        return result;
    }

    /// <summary>
    /// Calculates the best currency to exchange
    /// </summary>
    /// <param name="rates">exchange rates</param>
    /// <param name="dollarAmount">amount of money in dollars</param>
    /// <returns></returns>
    private static OutputBestRevenue CalculateFunc(IList<OutputRates> rates, int dollarAmount)
    {
        var data = new OutputBestRevenue[Consts.UsdExchangeMoneyArray.Length];

        Parallel.ForEach(
            Consts.UsdExchangeMoneyArray,
            Consts.GetParallelOptions(),
            (item, state, i) =>
            {
                data[i] = (from sell in rates
                           from buy in rates
                           where sell.Date < buy.Date
                           orderby CalculateRevenue(sell, buy, dollarAmount, item) descending
                           select new OutputBestRevenue
                           {
                               SellDate = sell.Date,
                               BuyDate = buy.Date,
                               Revenue = CalculateRevenue(sell, buy, dollarAmount, item),
                               Tool = item.ToString()
                           }).First();
            });

        return data.First(x => x.Revenue == data.Max(x => x.Revenue));
    }

    /// <summary>
    /// Formula for calculating revenue
    /// </summary>
    /// <param name="sell">Date of sale</param>
    /// <param name="buy">date of purchase</param>
    /// <param name="dollarAmount">amount of money in dollars</param>
    /// <param name="currencyName">currency name <see cref="Consts.UsdExchangeEnum"/></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException">exception if unsupported currency for exchange</exception>
    private static double CalculateRevenue(OutputRates sell, OutputRates buy, int dollarAmount, Consts.UsdExchangeEnum currencyName)
    {
        var difference = buy.Date.Subtract(sell.Date);

        return currencyName switch
        {
            Consts.UsdExchangeEnum.RUB => buy.Rub == 0 ? 0 : (sell.Rub * dollarAmount / buy.Rub) - difference.Days,
            Consts.UsdExchangeEnum.EUR => buy.Eur == 0 ? 0 : (sell.Eur * dollarAmount / buy.Eur) - difference.Days,
            Consts.UsdExchangeEnum.GBP => buy.Gbp == 0 ? 0 : (sell.Gbp * dollarAmount / buy.Gbp) - difference.Days,
            Consts.UsdExchangeEnum.JPY => buy.Jpy == 0 ? 0 : (sell.Jpy * dollarAmount / buy.Jpy) - difference.Days,
            _ => throw new NotSupportedException($"{currencyName.ToString()} is not supported.")
        };
    }
}