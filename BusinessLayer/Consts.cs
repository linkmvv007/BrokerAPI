using Microsoft.Extensions.Caching.Distributed;

namespace BusinessLayer;

/// <summary>
/// Common constants and variables, properties and functions
/// </summary>
public static class Consts
{
    /// <summary>
    /// List of currencies for dollar exchange
    /// </summary>
    internal enum UsdExchangeEnum { RUB, EUR, GBP, JPY }
    /// <summary>
    /// The number of processors for methods of the Parallel class
    /// </summary>
    internal const int MaxDegreeOfParallelism = 5;

    /// <summary>
    /// Cache period time
    /// </summary>
    /// 
    internal static readonly DateTimeOffset CachePeriodTime = DateTimeOffset.MaxValue;


    /// <summary>
    /// List of currencies for dollar exchange
    /// </summary>
    public static readonly string[] UsdExchangeMoneyArray = Enum.GetNames(typeof(UsdExchangeEnum)).ToArray();

    /// <summary>
    /// Url value parameters
    /// </summary>
    public static readonly string UsdExchangeMoney = string.Join(",", Consts.UsdExchangeMoneyArray);

    /// <summary>
    /// Defines the number of processors for methods of the Parallel class
    /// </summary>
    /// <param name="parallels"></param>
    /// <returns></returns>
    internal static ParallelOptions GetParallelOptions(int parallels = MaxDegreeOfParallelism) => new()
    {
        MaxDegreeOfParallelism = parallels
    };

    /// <summary>
    /// Redis caching option
    /// </summary>
    internal static DistributedCacheEntryOptions GetDistributedCacheEntryOptions => new()
    {
        AbsoluteExpiration = CachePeriodTime
    };

    /// <summary>
    /// 
    /// </summary>
    /// <param name="money"></param>
    /// <param name="date"></param>
    /// <returns></returns>
    internal static string GetCacheKey(string money, string date) => $"{money};{date}";
}
