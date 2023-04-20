using BusinessLayer.Contexts;
using FluentValidation;

namespace BusinessLayer.Validators;

/// <summary>
/// Query API Parameter Validator <see cref="RatesController"/>  method Best.
/// Validates the query input parameters.
/// </summary>
public class BestContextValidator : AbstractValidator<BestContext>
{
    /// <summary>
    /// Initialize a new instance of <see cref="BestContextValidator"/>
    /// </summary>
    public BestContextValidator()
    {
        RuleFor(x => x.endDate)
            .GreaterThan(x => x.startDate)
            .WithMessage("End date must be greater than start date")
            .Must((x, endDate) => endDate.Subtract(x.startDate).TotalDays <= 60)
            .WithMessage("The duration between start and end date must not exceed 60 days");

        RuleFor(x => x.endDate)
           .LessThanOrEqualTo(DateTime.Now)
           .WithMessage("End date cannot be greater than today");

        RuleFor(x => x.moneyUsd)
            .GreaterThan(0)
            .WithMessage("The number of dollars to exchange must be greater than 0");
    }
}
