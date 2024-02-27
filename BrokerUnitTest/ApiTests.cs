using BrokerUnitTest.Consts;
using BusinessLayer.Contexts;
using BusinessLayer.Mediator;

namespace BrokerUnitTest;

/// <summary>
/// Broker API tests
/// </summary>
public class ApiTests : BaseTest
{

    [SetUp]
    public void Setup()
    {

    }

    /// <summary>
    /// Check best rate
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task Best()
    {
        var context = new BestContext
        {
            MoneyUsd = TestConsts.DollarAmount,
            StartDate = TestConsts.StartDate,
            EndDate = TestConsts.EndDate,
        };

        var result = await Mediator.Send(new GetRatesQuery
        {
            Context = context
        });


        Assert.IsNotNull(result);
        Assert.That(result.Tool, Is.EqualTo("EUR"));
        Assert.That(result.Rates.Count, Is.EqualTo(3));
        Assert.That(result.SellDate, Is.EqualTo(TestConsts.MiddleDate));
        Assert.That(TestConsts.EndDate, Is.EqualTo(result.BuyDate));
    }
}
