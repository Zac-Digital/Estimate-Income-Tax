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

    public bool? GetIsPayingScottishTax() => 
        _sessionService.Deserialise<ScottishTaxDto>(JourneyStage.ScottishTax)?.IsPayingScottishTax;

    public string? GetPensionContributionDescriptor()
    {
        PensionDescriptor? pensionDescriptor =
            _sessionService.Deserialise<PensionContributionDto>(JourneyStage.PensionContribution)?.Descriptor;
        return pensionDescriptor switch
        {
            PensionDescriptor.Percentage => "%",
            PensionDescriptor.Pound => "£",
            null => null,
            _ => throw new ArgumentOutOfRangeException(nameof(pensionDescriptor))
        };
    }
    
    public string? GetPensionContribution() =>
        _sessionService.Deserialise<PensionContributionDto>(JourneyStage.PensionContribution)?.PensionContribution;
}