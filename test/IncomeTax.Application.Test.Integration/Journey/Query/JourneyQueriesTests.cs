using IncomeTax.Application.Journey.Command;
using IncomeTax.Application.Journey.Query;
using IncomeTax.Domain.Constant;
using Microsoft.Extensions.DependencyInjection;

namespace IncomeTax.Application.Test.Integration.Journey.Query;

public sealed class JourneyQueriesTests : CustomWebApplicationFactory
{
    private readonly JourneyQueries _journeyQueries;
    private readonly JourneyCommands _journeyCommands;

    public JourneyQueriesTests()
    {
        using IServiceScope scope = Services.CreateScope();
        _journeyQueries = scope.ServiceProvider.GetRequiredService<JourneyQueries>();
        _journeyCommands = scope.ServiceProvider.GetRequiredService<JourneyCommands>();
    }

    [Theory]
    [InlineData(32768.64, "Yearly", "£32,768.64 a year")]
    [InlineData(3072, "Monthly", "£3,072 a month")]
    [InlineData(2048, "Every 4 weeks", "£2,048 every 4 weeks")]
    [InlineData(512, "Weekly", "£512 a week")]
    [InlineData(128, "Daily", "£128 a day")]
    [InlineData(32, "Hourly", "£32 an hour")]
    public void GetGrossIncome_Returns_ExpectedResult(double amount, string frequency, string expected)
    {
        _journeyCommands.UpdateSalary(amount, frequency);

        string result = _journeyQueries.GetGrossIncome();
        
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(true, "Yes")]
    [InlineData(false, "No")]
    public void GetIsOverStatePensionAge_Returns_ExpectedResult(bool isOverStatePensionAge, string expected)
    {
        _journeyCommands.UpdateStatePension(isOverStatePensionAge);
        
        string result = _journeyQueries.GetIsOverStatePensionAge();
        
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [InlineData(null)]
    public void GetIsPayingScottishTax_Returns_ExpectedResult(bool? isPayingScottishTax)
    {
        _journeyCommands.UpdateScottishTax(isPayingScottishTax);
        
        bool? result = _journeyQueries.GetIsPayingScottishTax();
        
        Assert.Equal(isPayingScottishTax, result);
    }

    [Theory]
    [InlineData("5", PensionDescriptor.Percentage, "%")]
    [InlineData("500", PensionDescriptor.Pound, "£")]
    public void GetPensionContributionDescriptor_Returns_ExpectedResult(string pensionContribution, PensionDescriptor descriptor, string expected)
    {
        _journeyCommands.UpdatePensionContribution(pensionContribution, descriptor);
        
        string? result = _journeyQueries.GetPensionContributionDescriptor();
        
        Assert.NotNull(result);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetPensionContributionDescriptor_Returns_Null_IfNoPensionContribution()
    {
        string? result = _journeyQueries.GetPensionContributionDescriptor();
        
        Assert.Null(result);
    }

    [Fact]
    public void GetPensionContributionDescriptor_Throws_Exception_IfDescriptorIsInvalid()
    {
        _journeyCommands.UpdatePensionContribution("5", (PensionDescriptor)2);

        Assert.Throws<ArgumentOutOfRangeException>(() => _journeyQueries.GetPensionContributionDescriptor());
    }

    [Fact]
    public void GetPensionContribution_Returns_ExpectedResult()
    {
        _journeyCommands.UpdatePensionContribution("5", PensionDescriptor.Percentage);

        string? result = _journeyQueries.GetPensionContribution();
        
        Assert.NotNull(result);
        Assert.Equal("5", result);
    }

    [Fact]
    public void GetPensionContribution_Returns_Null_IfNoPensionContribution()
    {
        string? result = _journeyQueries.GetPensionContribution();
        
        Assert.Null(result);
    }
}