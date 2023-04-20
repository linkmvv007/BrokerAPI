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
            moneyUsd = TestConsts.DollarAmount,
            startDate = TestConsts.StartDate,
            endDate = TestConsts.EndDate,
        };

        var result = await Mediator.Send(new GetRatesQuery
        {
            Context = context
        });


        Assert.IsNotNull(result);
        Assert.That(result.Tool, Is.EqualTo("EUR"));
        Assert.That(3, Is.EqualTo(result.Rates.Count));
        Assert.That(result.SellDate, Is.EqualTo(TestConsts.MiddleDate));
        Assert.That(TestConsts.EndDate, Is.EqualTo(result.BuyDate));
    }
}
