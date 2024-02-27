using BrokerUnitTest.Consts;
using DataLayer.ApiLayer;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace BrokerUnitTest;

public class FinanceTests
{
    private const string Usd = "USD";
    readonly DateTime _startDate = DateTime.Now.Date.AddDays(-120);
    readonly DateTime _endDate = DateTime.Now.Date.AddDays(-90);
    readonly Collection<ExchangeRates> _currencyRates = [];


    [SetUp]
    public void Setup()
    {

    }

    /// <summary>
    /// Test data initialization
    /// </summary>
    private void InitApiData()
    {
        var difference = _endDate.Subtract(_startDate);
        var days = difference.Days;
        for (var i = 0; i < days; i++)
        {
            _currencyRates.Add(new ExchangeRates
            {
                Base = Usd,
                Date = _startDate.AddDays(i),
                Rates = new Rates
                {
                    Eur = 0.91 + i * 0.01,
                    Gbp = 0.8 + i * 0.001,
                    Jpy = 133 + i * 0.2,
                    Rub = 82 + i * 0.1
                }
            });
        }

        _currencyRates[28].Rates.Eur = 0.82;
    }

    /// <summary>
    /// Checks parsing json into a typed object ExchangeRates
    /// </summary>
    [Test]
    public void CheckFixerApiParser()
    {
        var json =
        "{\"success\":true,\"timestamp\":1681299483,\"historical\":true,\"base\":\"USD\",\"date\":\"2023-04-12\",\"rates\":{\"RUB\":82.35006,\"EUR\":0.91495,\"GBP\":0.805898,\"JPY\":133.688957}}";


        var data = JsonConvert.DeserializeObject<ExchangeRates>(json);

        Assert.IsTrue(data.Success);
    }

    /// <summary>
    /// Checks the algorithm for finding the best dates for exchanging dollars
    /// </summary>
    [Test]
    public void EurRates()
    {
        // init data
        InitApiData();

        // calculate
        var bestDates =
            from sell in _currencyRates
            from buy in _currencyRates
            where sell.Date < buy.Date
            orderby CalculateEur(sell, buy) descending
            select new { SellDate = sell.Date, BuyDate = buy.Date, Revenue = CalculateEur(sell, buy) };

        var bestSellDate = bestDates.First();

        Console.WriteLine($"{bestSellDate.SellDate}, {bestSellDate.BuyDate}, {bestSellDate.Revenue}");

        // Asserts
        Assert.That(_currencyRates[28].Date, Is.EqualTo(bestSellDate.BuyDate));
        Assert.That(_currencyRates[27].Date, Is.EqualTo(bestSellDate.SellDate));

    }
    static double CalculateEur(ExchangeRates sell, ExchangeRates buy)
    {
        TimeSpan difference = buy.Date.Subtract(sell.Date);
        var days = difference.Days;

        return (sell.Rates.Eur * TestConsts.DollarAmount / buy.Rates.Eur) - days;
    }

    /// <summary>
    /// Checks the algorithm for finding the best dates for exchanging dollars
    /// </summary>
    [Test]
    public void RubRates()
    {
        // init data
        var currencyRates = new Collection<ExchangeRates>
        {
            new()
            {
                Base = Usd,
                Date = DateTime.Parse("2014-12-15"),
                Rates = new Rates
                {
                    Rub = 60.17
                }
            },
            new()
            {
                Base = Usd,
                Date = DateTime.Parse("2014-12-16"),
                Rates = new Rates
                {
                    Rub = 72.99
                }
            },
            new()
            {
                Base = Usd,
                Date = DateTime.Parse("2014-12-17"),
                Rates = new Rates
                {
                    Rub = 66.01
                }
            },
            new()
            {
                Base = Usd,
                Date = DateTime.Parse("2014-12-18"),
                Rates = new Rates
                {
                    Rub = 61.44
                }
            },
            new()
            {
                Base = Usd,
                Date = DateTime.Parse("2014-12-19"),
                Rates = new Rates
                {
                    Rub = 59.79
                }
            },
            new()
            {
                Base = Usd,
                Date = DateTime.Parse("2014-12-20"),
                Rates = new Rates
                {
                    Rub = 59.79
                }
            },
            new()
            {
                Base = Usd,
                Date = DateTime.Parse("2014-12-21"),
                Rates = new Rates
                {
                    Rub = 59.79
                }
            },
            new()
            {
                Base = Usd,
                Date = DateTime.Parse("2014-12-22"),
                Rates = new Rates
                {
                    Rub = 54.78
                }
            },
            new()
            {
                Base = Usd,
                Date = DateTime.Parse("2014-12-23"),
                Rates = new Rates
                {
                    Rub = 54.8
                }
            }
        };

        // calculate
        var bestDates = from sell in currencyRates
                        from buy in currencyRates
                        where sell.Date < buy.Date
                        orderby CalculateRub(sell, buy) descending
                        select new { SellDate = sell.Date, BuyDate = buy.Date, Revenue = CalculateRub(sell, buy) };

        var bestSellDate = bestDates.First();


        // Asserts
        Assert.That(DateTime.Parse("2014-12-22"), Is.EqualTo(bestSellDate.BuyDate));
        Assert.That(DateTime.Parse("2014-12-16"), Is.EqualTo(bestSellDate.SellDate));
    }

    private static double CalculateRub(ExchangeRates sell, ExchangeRates buy)
    {
        var difference = buy.Date.Subtract(sell.Date);
        var days = difference.Days;

        return (sell.Rates.Rub * TestConsts.DollarAmount / buy.Rates.Rub) - days;
    }
}