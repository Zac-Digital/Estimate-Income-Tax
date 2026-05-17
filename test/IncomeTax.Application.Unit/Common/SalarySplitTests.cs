using IncomeTax.Application.Common;

namespace IncomeTax.Application.Unit.Common;

public sealed class SalarySplitTests
{
    [Theory]
    [InlineData("£32,768 a year", 32768, "a year")]
    [InlineData("£32,768.64 a year", 32768.64, "a year")]
    [InlineData("£2048 a month", 2048, "a month")]
    [InlineData("£2048.32 a month", 2048.32, "a month")]
    [InlineData("£2048 every 4 weeks", 2048, "every 4 weeks")]
    [InlineData("£2048.32 every 4 weeks", 2048.32, "every 4 weeks")]
    [InlineData("£512 a week", 512, "a week")]
    [InlineData("£512.16 a week", 512.16, "a week")]
    [InlineData("£128 a day", 128, "a day")]
    [InlineData("£128.80 a day", 128.8, "a day")]
    [InlineData("£32 an hour", 32, "an hour")]
    [InlineData("£32.40 an hour", 32.4, "an hour")]
    public void Split_When_Salary_Is_Valid_Returns_Amount_And_Period(string salary, double amount, string period)
    {
        (double? Amount, string? Period) salarySplit = SalarySplit.Split(salary);
        
        Assert.NotNull(salarySplit.Amount);
        Assert.NotNull(salarySplit.Period);
        Assert.Equal(salarySplit.Amount, amount);
        Assert.Equal(salarySplit.Period, period);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void Split_When_Salary_IsNull_Returns_Null(string? salary)
    {
        (double? Amount, string? Period) salarySplit = SalarySplit.Split(salary);
        
        Assert.Null(salarySplit.Amount);
        Assert.Null(salarySplit.Period);
    }
}