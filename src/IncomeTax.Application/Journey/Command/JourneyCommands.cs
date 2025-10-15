using IncomeTax.Application.Session;
using IncomeTax.Domain;
using IncomeTax.Domain.Constant;

namespace IncomeTax.Application.Journey.Command;

public sealed class JourneyCommands
{
    private readonly SessionService _sessionService;

    public JourneyCommands(SessionService sessionService) { _sessionService = sessionService; }

    public void UpdateSalary(double amount, string frequency) => _sessionService.Serialise(JourneyStage.Salary,
        new SalaryDto(amount, SalaryFrequencyExtensions.SalaryPageRadioOptionToEnum[frequency]));

    public void UpdateStatePension(bool isOverStatePensionAge) =>
        _sessionService.Serialise(JourneyStage.StatePension, new StatePensionDto(isOverStatePensionAge));
}