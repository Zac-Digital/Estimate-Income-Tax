using IncomeTax.Application.Common;
using IncomeTax.Application.Session;
using IncomeTax.Domain.Journey;

namespace IncomeTax.Application.Journey;

public sealed class OnGetJourneyValidator(SessionService sessionService)
{
    public bool IsValid(string currentPage, out string? redirectPage)
    {
        return currentPage switch
        {
            nameof(JourneyStage.Index) or nameof(JourneyStage.Salary) => AlwaysValid(out redirectPage),
            nameof(JourneyStage.HowManyDaysWorked) => IsSalaryOptionalValid("A DAY", out redirectPage),
            nameof(JourneyStage.HowManyHoursWorked) => IsSalaryOptionalValid("AN HOUR", out redirectPage),
            nameof(JourneyStage.StatePension) => IsStatePensionValid(out redirectPage),
            nameof(JourneyStage.CheckAnswers) => IsCheckAnswersValid(out redirectPage),
            _ => throw new ArgumentOutOfRangeException(nameof(currentPage))
        };
    }

    private static bool AlwaysValid(out string? redirectPage)
    {
        redirectPage = null;
        return true;
    }

    private bool IsSalaryOptionalValid(string validPeriod, out string? redirectPage)
    {
        string? period = SalarySplit.Split(sessionService.Get(JourneyStage.Salary)).Period?.ToUpperInvariant();

        if (period is null || !period.Equals(validPeriod))
        {
            redirectPage = nameof(JourneyStage.Salary);
            return false;
        }

        redirectPage = null;
        return true;
    }

    private bool IsStatePensionValid(out string? redirectPage)
    {
        string? period = SalarySplit.Split(sessionService.Get(JourneyStage.Salary)).Period?.ToUpperInvariant();

        switch (period)
        {
            case "A DAY" when sessionService.Get(JourneyStage.HowManyDaysWorked) is null:
                redirectPage = nameof(JourneyStage.HowManyDaysWorked);
                return false;
            case "AN HOUR" when sessionService.Get(JourneyStage.HowManyHoursWorked) is null:
                redirectPage = nameof(JourneyStage.HowManyHoursWorked);
                return false;
            case null:
                redirectPage = nameof(JourneyStage.Salary);
                return false;
            default:
                redirectPage = null;
                return true;
        }
    }

    private bool IsCheckAnswersValid(out string? redirectPage)
    {
        if (sessionService.Get(JourneyStage.HowManyDaysWorked) is not null &&
            sessionService.Get(JourneyStage.HowManyHoursWorked) is not null)
        {
            redirectPage = nameof(JourneyStage.Salary);
            return false;
        }

        if (sessionService.Get(JourneyStage.StatePension) is null)
        {
            redirectPage = nameof(JourneyStage.StatePension);
            return false;
        }

        if (!IsStatePensionValid(out redirectPage))
        {
            return false;
        }

        redirectPage = null;
        return true;
    }
}