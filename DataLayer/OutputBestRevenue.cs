namespace DataLayer;

/// <summary>
/// Best revenue data
/// </summary>
public class OutputBestRevenue
{
    /// <summary>
    ///  Date of purchase
    /// </summary>
    public DateTime BuyDate { get; set; }
    /// <summary>
    /// Date of sale
    /// </summary>
    public DateTime SellDate { get; set; }
    /// <summary>
    /// Currency name
    /// </summary>
    public string? Tool { get; set; }
    /// <summary>
    /// Revenue
    /// </summary>
    public double Revenue { get; set; }
}
