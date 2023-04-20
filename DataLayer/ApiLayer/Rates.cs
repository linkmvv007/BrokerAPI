namespace DataLayer.ApiLayer;


/// <summary>
/// Dollar exchange rates. Part of the data for <see cref="OutputExchangeRates"/>
/// </summary>
public class Rates
{
    /// <summary>
    /// In rubles
    /// </summary>
    public double RUB { get; set; }
    /// <summary>
    /// In Euro
    /// </summary>
    public double EUR { get; set; }
    /// <summary>
    /// In pounds
    /// </summary>
    public double GBP { get; set; }
    /// <summary>
    /// In yen
    /// </summary>
    public double JPY { get; set; }
}

