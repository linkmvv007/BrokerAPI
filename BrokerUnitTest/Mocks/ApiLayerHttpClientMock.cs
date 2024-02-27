using BrokerUnitTest.Consts;
using BusinessLayer.Interfaces;
using DataLayer.ApiLayer;
using Moq;
using Newtonsoft.Json;

namespace BrokerUnitTest.Mocks;

/// <summary>
/// ApiLayerHttpClient mock object. 
/// </summary>
public class ApiLayerHttpClientMock
{
    /// <summary>
    /// Test data of exchange rates for test dates
    /// </summary>
    /// <returns></returns>
    public static Mock<IApiLayerHttpClient> GetMock()
    {
        var mock = new Mock<IApiLayerHttpClient>();

        mock.Setup(x => x.GetExchangeRatesAsync(It.Is<DateTime>(d => d == TestConsts.StartDate)))
            .ReturnsAsync(GetTestData(0));

        mock.Setup(x => x.GetExchangeRatesAsync(It.Is<DateTime>(d => d == TestConsts.EndDate)))
            .ReturnsAsync(GetTestData(2));

        mock.Setup(x => x.GetExchangeRatesAsync(It.Is<DateTime>(d => d == TestConsts.MiddleDate)))
            .ReturnsAsync(GetTestData(1));

        return mock;
    }

    /// <summary>
    /// Formation of test data of exchange rates
    /// </summary>
    /// <param name="dayNumber">day number in order</param>
    /// <returns>Dollar exchange rate test data</returns>
    private static ExchangeRates? GetTestData(int dayNumber)
    {
        const string json =
            "{\"success\":true,\"timestamp\":1681299483,\"historical\":true,\"base\":\"USD\",\"date\":\"2023-04-12\",\"rates\":{\"RUB\":82.35006,\"EUR\":0.91495,\"GBP\":0.805898,\"JPY\":133.688957}}";

        lock (json)
        {
            var rates = JsonConvert.DeserializeObject<ExchangeRates>(json);

            rates.Date = rates.Date.AddDays(dayNumber);
            if (dayNumber == 1)
                rates.Rates.Eur += 10;

            return rates;
        }
    }
}
