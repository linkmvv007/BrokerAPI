using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Contexts;


/// <summary>
/// Query context to determine the best exchange currency for maximum revenue in the selected time period
/// </summary>
public record BestContext
{
    /// <summary>
    /// period start date
    /// </summary>
    [Required]
    public DateTime StartDate { get; init; }
    /// <summary>
    /// period end date
    /// </summary>
    [Required]
    public DateTime EndDate { get; init; }
    /// <summary>
    /// Number of dollars to exchange
    /// </summary>
    [Required]
    public int MoneyUsd { get; init; } = 100;
};