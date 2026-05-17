using IncomeTax.Application.Common;
using IncomeTax.Application.Session;
using IncomeTax.Domain.Journey;

namespace IncomeTax.Application.Journey;

public sealed class OnPostJourneyValidator(SessionService sessionService)
{
    public bool ShouldNavigateToCheckAnswers(string currentPage)
    {
        return currentPage switch
        {
            nameof(JourneyStage.Index) or nameof(JourneyStage.StatePension) or nameof(JourneyStage.CheckAnswers) => false,
            nameof(JourneyStage.Salary) => ShouldSalaryNavigate(),
            nameof(JourneyStage.HowManyDaysWorked) => ShouldSalaryOptionalNavigate(),
            nameof(JourneyStage.HowManyHoursWorked) => ShouldSalaryOptionalNavigate(),
            _ => throw new ArgumentOutOfRangeException(nameof(currentPage))
        };
    }

    private bool ShouldSalaryNavigate()
    {
        string? period = SalarySplit.Split(sessionService.Get(JourneyStage.Salary)).Period?.ToUpperInvariant();

        return period switch
        {
            "A DAY" => sessionService.Get(JourneyStage.HowManyDaysWorked) is not null,
            "AN HOUR" => sessionService.Get(JourneyStage.HowManyHoursWorked) is not null,
            _ => sessionService.Get(JourneyStage.StatePension) is not null
        };
    }

    private bool ShouldSalaryOptionalNavigate() =>
        sessionService.Get(JourneyStage.StatePension) is not null;
}