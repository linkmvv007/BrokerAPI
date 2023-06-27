namespace DataLayer;

/// <summary>
/// Describes json with data on the best exchange currency for the highest revenue
/// </summary>
public sealed class OutputExchangeRates : OutputBestRevenue
{
    /// <summary>
    /// Initialize a new instance of <see cref="OutputExchangeRates"/>
    /// </summary>
    public OutputExchangeRates()
    {
    }

    /// <summary>
    /// Exchange rates
    /// </summary>
    required public List<OutputRates> Rates { get; init; }
}
