using System.Globalization;
using IncomeTax.Application.Session;
using IncomeTax.Domain;
using IncomeTax.Domain.Constant;

namespace IncomeTax.Application.Journey.Query;

public sealed class JourneyQueries
{
    private readonly SessionService _sessionService;

    public JourneyQueries(SessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public string GetGrossIncome()
    {
        SalaryDto salaryDto = _sessionService.Deserialise<SalaryDto>(JourneyStage.Salary)!;
        return
            $"{salaryDto.Amount.ToString("C", CultureInfo.CreateSpecificCulture("en-GB")).Replace(".00", "")}{SalaryFrequencyExtensions.CheckAnswersPageGrossIncomeFriendlySuffix[salaryDto.Frequency]}";
    }

    public string GetIsOverStatePensionAge() =>
        _sessionService.Deserialise<StatePensionDto>(JourneyStage.StatePension)!.IsOverStatePensionAge ? "Yes" : "No";

    public string? GetIsPayingScottishTax()
    {
        bool? isPayingScottishTax =
            _sessionService.Deserialise<ScottishTaxDto>(JourneyStage.ScottishTax)?.IsPayingScottishTax;

        if (isPayingScottishTax is null) return null;
        return isPayingScottishTax.Value ? "Yes" : "No";
    }
}