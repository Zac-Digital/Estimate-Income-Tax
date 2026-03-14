using IncomeTax.Application.Journey.Command;
using IncomeTax.Domain.Constant;
using Microsoft.Extensions.DependencyInjection;

namespace IncomeTax.Application.Test.Integration.Journey.Command;

public sealed class JourneyCommandsTests : CustomWebApplicationFactory
{
    private readonly JourneyCommands _journeyCommands;

    public JourneyCommandsTests()
    {
        using IServiceScope scope = Services.CreateScope();
        _journeyCommands = scope.ServiceProvider.GetRequiredService<JourneyCommands>();
    }

    [Theory]
    [InlineData("Yearly")]
    [InlineData("Monthly")]
    [InlineData("Every 4 weeks")]
    [InlineData("Weekly")]
    [InlineData("Daily")]
    [InlineData("Hourly")]
    public void UpdateSalary_IsSuccessful_With_CorrectValues(string frequency)
        => _journeyCommands.UpdateSalary(32768.64, frequency);

    [Fact]
    public void UpdateSalary_ThrowsException_With_UnknownFrequency() =>
        Assert.Throws<KeyNotFoundException>(() => _journeyCommands.UpdateSalary(32768.64, "Invalid"));

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void UpdateStatePension_IsSuccessful_With_CorrectValue(bool isOverStatePensionAge)
        => _journeyCommands.UpdateStatePension(isOverStatePensionAge);

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [InlineData(null)]
    public void UpdateScottishTax_IsSuccessful_With_CorrectValue(bool? isPayingScottishTax)
        => _journeyCommands.UpdateScottishTax(isPayingScottishTax);

    [Theory]
    [InlineData("5", PensionDescriptor.Percentage)]
    [InlineData("1638.43", PensionDescriptor.Pound)]
    public void UpdatePensionContribution_IsSuccessful_With_CorrectValues(string pensionContribution,
        PensionDescriptor descriptor)
    {
        _journeyCommands.UpdatePensionContribution(pensionContribution, descriptor);
    }

    [Fact]
    public void DeletePensionContribution_IsSuccessful_If_JourneyExists()
    {
        _journeyCommands.UpdatePensionContribution("5", PensionDescriptor.Percentage);
        _journeyCommands.DeletePensionContribution();
    }

    [Fact]
    public void DeletePensionContribution_IsSuccessful_If_JourneyDoesNotExist()
    {
        _journeyCommands.DeletePensionContribution();
    }
}