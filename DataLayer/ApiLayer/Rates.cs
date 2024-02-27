namespace DataLayer.ApiLayer;


/// <summary>
/// Dollar exchange rates. Part of the data for <see cref="OutputExchangeRates"/>
/// </summary>
public class Rates
{
    /// <summary>
    /// In rubles
    /// </summary>
    public double Rub { get; init; }
    /// <summary>
    /// In Euro
    /// </summary>
    public double Eur { get; set; }
    /// <summary>
    /// In pounds
    /// </summary>
    public double Gbp { get; init; }
    /// <summary>
    /// In yen
    /// </summary>
    public double Jpy { get; init; }
}

