using IncomeTax.Application.Journey;
using IncomeTax.Application.Session;
using IncomeTax.Domain.Journey;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace IncomeTax.Application.Unit.Journey;

public sealed class JourneyValidatorTests
{
    private IHttpContextAccessor _accessor;

    private readonly JourneyValidator _journeyValidator;

    public JourneyValidatorTests()
    {
        _accessor = Substitute.For<IHttpContextAccessor>();
        _accessor.HttpContext = new DefaultHttpContext();
        _accessor.HttpContext.Session = new MockSession();
        
        _journeyValidator = new JourneyValidator(new SessionService(_accessor));
    }

    [Fact]
    public void IsValid_When_Stage_Index_IsValid_Returns_True_With_NoRedirect()
    {
        bool isValid = _journeyValidator.IsValid(nameof(JourneyStage.Index), out string? redirectPage);
        
        Assert.True(isValid);
        Assert.Null(redirectPage);
    }
    
    [Fact]
    public void IsValid_When_Stage_Salary_IsValid_Returns_True_With_NoRedirect()
    {
        bool isValid = _journeyValidator.IsValid(nameof(JourneyStage.Salary), out string? redirectPage);
        
        Assert.True(isValid);
        Assert.Null(redirectPage);
    }
    
    [Fact]
    public void IsValid_When_Stage_HowManyDaysWorked_IsValid_Returns_True_With_NoRedirect()
    {
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.Salary), "£100.10 a day");
        
        bool isValid = _journeyValidator.IsValid(nameof(JourneyStage.HowManyDaysWorked), out string? redirectPage);
        
        Assert.True(isValid);
        Assert.Null(redirectPage);
    }
    
    [Theory]
    [InlineData("£32,768 a year")]
    [InlineData("2048 a month")]
    [InlineData("2048 every 4 weeks")]
    [InlineData("512 weekly")]
    [InlineData(null)]
    public void IsValid_When_Stage_HowManyDaysWorked_IsNotValid_Returns_False_With_Redirect(string? salary)
    {
        if (salary is not null) 
            _accessor.HttpContext.Session.SetString(nameof(JourneyStage.Salary), salary);
        
        bool isValid = _journeyValidator.IsValid(nameof(JourneyStage.HowManyDaysWorked), out string? redirectPage);
        
        Assert.False(isValid);
        Assert.Equal(nameof(JourneyStage.Salary), redirectPage);
    }
    
    [Fact]
    public void IsValid_When_Stage_HowManyHoursWorked_IsValid_Returns_True_With_NoRedirect()
    {
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.Salary), "£20.20 an hour");
        
        bool isValid = _journeyValidator.IsValid(nameof(JourneyStage.HowManyHoursWorked), out string? redirectPage);
        
        Assert.True(isValid);
        Assert.Null(redirectPage);
    }
    
    [Theory]
    [InlineData("£32,768 a year")]
    [InlineData("2048 a month")]
    [InlineData("2048 every 4 weeks")]
    [InlineData("512 weekly")]
    [InlineData(null)]
    public void IsValid_When_Stage_HowManyHoursWorked_IsNotValid_Returns_False_With_Redirect(string? salary)
    {
        if (salary is not null) 
            _accessor.HttpContext.Session.SetString(nameof(JourneyStage.Salary), salary);
        
        bool isValid = _journeyValidator.IsValid(nameof(JourneyStage.HowManyHoursWorked), out string? redirectPage);
        
        Assert.False(isValid);
        Assert.Equal(nameof(JourneyStage.Salary), redirectPage);
    }

    [Fact]
    public void IsValid_When_Core_Stage_StatePension_IsValid_Returns_True_With_NoRedirect()
    {
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.Salary), "£32,768 a year");
        
        bool isValid = _journeyValidator.IsValid(nameof(JourneyStage.StatePension), out string? redirectPage);
        
        Assert.True(isValid);
        Assert.Null(redirectPage);
    }
    
    [Fact]
    public void IsValid_When_Optional_Stage_HowManyDaysWorked_StatePension_IsValid_Returns_True_With_NoRedirect()
    {
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.Salary), "£100.10 a day");
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.HowManyDaysWorked), "5");
        
        bool isValid = _journeyValidator.IsValid(nameof(JourneyStage.StatePension), out string? redirectPage);
        
        Assert.True(isValid);
        Assert.Null(redirectPage);
    }
    
    [Fact]
    public void IsValid_When_Optional_Stage_HowManyHoursWorked_StatePension_IsValid_Returns_True_With_NoRedirect()
    {
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.Salary), "20.20 an hour");
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.HowManyHoursWorked), "40");
        
        bool isValid = _journeyValidator.IsValid(nameof(JourneyStage.StatePension), out string? redirectPage);
        
        Assert.True(isValid);
        Assert.Null(redirectPage);
    }

    [Theory]
    [InlineData("£100.10 a day", nameof(JourneyStage.HowManyDaysWorked))]
    [InlineData("£20.20 an hour", nameof(JourneyStage.HowManyHoursWorked))]
    [InlineData(null, nameof(JourneyStage.Salary))]
    public void IsValid_When_Stage_StatePension_IsNotValid_Returns_False_With_Redirect(string? salary, string expectedRedirect)
    {
        if (salary is not null)
            _accessor.HttpContext.Session.SetString(nameof(JourneyStage.Salary), salary);
        
        bool isValid = _journeyValidator.IsValid(nameof(JourneyStage.StatePension), out string? redirectPage);
        
        Assert.False(isValid);
        Assert.Equal(expectedRedirect, redirectPage);
    }

    [Fact]
    public void IsValid_When_Core_Stage_CheckAnswers_IsValid_Returns_True_With_NoRedirect()
    {
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.Salary), "£32,768 a year");
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.StatePension), "No");
        
        bool isValid = _journeyValidator.IsValid(nameof(JourneyStage.CheckAnswers), out string? redirectPage);
        
        Assert.True(isValid);
        Assert.Null(redirectPage);   
    }
    
    [Theory]
    [InlineData("£100.10 a day", nameof(JourneyStage.HowManyDaysWorked))]
    [InlineData("£20.20 an hour", nameof(JourneyStage.HowManyHoursWorked))]
    public void IsValid_When_Optional_Stage_CheckAnswers_IsValid_Returns_True_With_NoRedirect(string salary, string optionalStage)
    {
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.Salary), salary);
        _accessor.HttpContext.Session.SetString(optionalStage, "4");
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.StatePension), "No");
        
        bool isValid = _journeyValidator.IsValid(nameof(JourneyStage.CheckAnswers), out string? redirectPage);
        
        Assert.True(isValid);
        Assert.Null(redirectPage);   
    }
    
    [Fact]
    public void IsValid_When_Core_Stage_StatePension_YearlySalary_CheckAnswers_IsNotValid_Returns_False_With_Redirect()
    {
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.Salary), "£32,768 a year");
        
        bool isValid = _journeyValidator.IsValid(nameof(JourneyStage.CheckAnswers), out string? redirectPage);
        
        Assert.False(isValid);
        Assert.Equal(nameof(JourneyStage.StatePension), redirectPage);
    }

    [Fact]
    public void IsValid_When_Core_Stage_Salary_CheckAnswers_IsNotValid_Returns_False_With_Redirect()
    {
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.StatePension), "No");
        
        bool isValid = _journeyValidator.IsValid(nameof(JourneyStage.CheckAnswers), out string? redirectPage);
        
        Assert.False(isValid);
        Assert.Equal(nameof(JourneyStage.Salary), redirectPage);
    }
    
    [Fact]
    public void IsValid_When_Core_Stage_Salary_StatePension_CheckAnswers_IsNotValid_Returns_False_With_Redirect()
    {
        bool isValid = _journeyValidator.IsValid(nameof(JourneyStage.CheckAnswers), out string? redirectPage);
        
        Assert.False(isValid);
        Assert.Equal(nameof(JourneyStage.StatePension), redirectPage);
    }
    
    [Fact]
    public void IsValid_When_Optional_Stage_CheckAnswers_IsNotValid_Returns_False_With_Redirect()
    {
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.HowManyDaysWorked), "5");
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.HowManyHoursWorked), "40");
        
        bool isValid = _journeyValidator.IsValid(nameof(JourneyStage.CheckAnswers), out string? redirectPage);
        
        Assert.False(isValid);
        Assert.Equal(nameof(JourneyStage.Salary), redirectPage);
    }
}