namespace BrokerUnitTest.Consts;

/// <summary>
/// Common constants for tests
/// </summary>
internal class TestConsts
{
    /// <summary>
    /// Amount of dollars to exchange
    /// </summary>
    internal const int DollarAmount = 100;

    /// <summary>
    /// start date of currency trading
    /// </summary>
    internal static readonly DateTime StartDate = DateTime.Parse("2023-04-12");
    /// <summary>
    ///    end date of currency trading
    /// </summary>
    internal static readonly DateTime EndDate = DateTime.Parse("2023-04-14");
    /// <summary>
    /// 
    /// </summary>
    internal static DateTime MiddleDate = DateTime.Parse("2023-04-13");
}
