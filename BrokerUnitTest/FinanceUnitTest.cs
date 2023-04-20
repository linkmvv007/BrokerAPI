using BrokerUnitTest.Consts;
using DataLayer.ApiLayer;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace BrokerUnitTest;

public class FinanceTests
{
    const string USD = "USD";
    readonly DateTime startDate = DateTime.Now.Date.AddDays(-120);
    readonly DateTime endDate = DateTime.Now.Date.AddDays(-90);
    readonly Collection<ExchangeRates> currencyRates = new();


    [SetUp]
    public void Setup()
    {

    }

    /// <summary>
    /// Test data initialization
    /// </summary>
    private void InitAPIData()
    {
        TimeSpan difference = endDate.Subtract(startDate);
        int days = difference.Days;
        for (int i = 0; i < days; i++)
        {
            currencyRates.Add(new ExchangeRates
            {
                Base = USD,
                Date = startDate.AddDays(i),
                Rates = new Rates
                {
                    EUR = 0.91 + i * 0.01,
                    GBP = 0.8 + i * 0.001,
                    JPY = 133 + i * 0.2,
                    RUB = 82 + i * 0.1
                }
            });
        }

        currencyRates[28].Rates.EUR = 0.82;
    }

    /// <summary>
    /// Ñhecks parsing json into a typed object ExchangeRates
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
        InitAPIData();

        // calculate
        var bestDates =
            from sell in currencyRates
            from buy in currencyRates
            where sell.Date < buy.Date
            orderby CalculateEur(sell, buy) descending
            select new { SellDate = sell.Date, BuyDate = buy.Date, Revenue = CalculateEur(sell, buy) };

        var bestSellDate = bestDates.First();

        Console.WriteLine($"{bestSellDate.SellDate}, {bestSellDate.BuyDate}, {bestSellDate.Revenue}");

        // Asserts
        Assert.That(currencyRates[28].Date, Is.EqualTo(bestSellDate.BuyDate));
        Assert.That(currencyRates[27].Date, Is.EqualTo(bestSellDate.SellDate));

    }
    static double CalculateEur(ExchangeRates sell, ExchangeRates buy)
    {
        TimeSpan difference = buy.Date.Subtract(sell.Date);
        int days = difference.Days;

        return (sell.Rates.EUR * TestConsts.DollarAmount / buy.Rates.EUR) - days;
    }

    /// <summary>
    /// Checks the algorithm for finding the best dates for exchanging dollars
    /// </summary>
    [Test]
    public void RubRates()
    {
        // init data
        var currencyRates = new Collection<ExchangeRates>();
        currencyRates.Add(
            new ExchangeRates
            {
                Base = USD,
                Date = DateTime.Parse("2014-12-15"),
                Rates = new Rates
                {
                    RUB = 60.17
                }
            });
        currencyRates.Add(new ExchangeRates
        {
            Base = USD,
            Date = DateTime.Parse("2014-12-16"),
            Rates = new Rates
            {
                RUB = 72.99
            }
        });
        currencyRates.Add(new ExchangeRates
        {
            Base = USD,
            Date = DateTime.Parse("2014-12-17"),
            Rates = new Rates
            {
                RUB = 66.01
            }
        });
        currencyRates.Add(new ExchangeRates
        {
            Base = USD,
            Date = DateTime.Parse("2014-12-18"),
            Rates = new Rates
            {
                RUB = 61.44
            }
        });

        currencyRates.Add(new ExchangeRates
        {
            Base = USD,
            Date = DateTime.Parse("2014-12-19"),
            Rates = new Rates
            {
                RUB = 59.79
            }
        });
        currencyRates.Add(new ExchangeRates
        {
            Base = USD,
            Date = DateTime.Parse("2014-12-20"),
            Rates = new Rates
            {
                RUB = 59.79
            }
        });
        currencyRates.Add(new ExchangeRates
        {
            Base = USD,
            Date = DateTime.Parse("2014-12-21"),
            Rates = new Rates
            {
                RUB = 59.79
            }
        });
        currencyRates.Add(new ExchangeRates
        {
            Base = USD,
            Date = DateTime.Parse("2014-12-22"),
            Rates = new Rates
            {
                RUB = 54.78
            }
        });
        currencyRates.Add(new ExchangeRates
        {
            Base = USD,
            Date = DateTime.Parse("2014-12-23"),
            Rates = new Rates
            {
                RUB = 54.8
            }
        });

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
    static double CalculateRub(ExchangeRates sell, ExchangeRates buy)
    {
        TimeSpan difference = buy.Date.Subtract(sell.Date);
        int days = difference.Days;

        return (sell.Rates.RUB * TestConsts.DollarAmount / buy.Rates.RUB) - days;
    }
}