using IncomeTax.Application.Journey;
using IncomeTax.Application.Session;
using IncomeTax.Domain.Journey;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace IncomeTax.Application.Unit.Journey;

public sealed class OnPostJourneyValidatorTests
{
    private readonly IHttpContextAccessor _accessor;

    private readonly OnPostJourneyValidator _onPostJourneyValidator;

    public OnPostJourneyValidatorTests()
    {
        _accessor = Substitute.For<IHttpContextAccessor>();
        _accessor.HttpContext = new DefaultHttpContext();
        _accessor.HttpContext.Session = new MockSession();

        _onPostJourneyValidator = new OnPostJourneyValidator(new SessionService(_accessor));
    }

    [Theory]
    [InlineData(nameof(JourneyStage.Index))]
    [InlineData(nameof(JourneyStage.StatePension))]
    [InlineData(nameof(JourneyStage.CheckAnswers))]
    public void ShouldNavigateToCheckAnswers_When_Stage_HasNoFollowUp_Returns_False(string stage)
    {
        bool shouldNavigate = _onPostJourneyValidator.ShouldNavigateToCheckAnswers(stage);

        Assert.False(shouldNavigate);
    }

    [Fact]
    public void ShouldNavigateToCheckAnswers_When_Stage_Salary_If_NextStage_IsDone_Returns_True()
    {
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.Salary), "£32,768 a year");
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.StatePension), "No");
        
        bool shouldNavigate = _onPostJourneyValidator.ShouldNavigateToCheckAnswers(nameof(JourneyStage.Salary));
        
        Assert.True(shouldNavigate);
    }

    [Fact]
    public void ShouldNavigateToCheckAnswers_When_Stage_Salary_If_NextOptionalStage_DaysWorked_IsDone_Returns_True()
    {
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.Salary), "£100.10 a day");
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.HowManyDaysWorked), "5");
        
        bool shouldNavigate = _onPostJourneyValidator.ShouldNavigateToCheckAnswers(nameof(JourneyStage.Salary));
        
        Assert.True(shouldNavigate);
    }
    
    [Fact]
    public void ShouldNavigateToCheckAnswers_When_Stage_Salary_If_NextOptionalStage_HoursWorked_IsDone_Returns_True()
    {
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.Salary), "£20.20 an hour");
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.HowManyHoursWorked), "40");
        
        bool shouldNavigate = _onPostJourneyValidator.ShouldNavigateToCheckAnswers(nameof(JourneyStage.Salary));
        
        Assert.True(shouldNavigate);
    }
    
    [Fact]
    public void ShouldNavigateToCheckAnswers_When_Stage_Salary_If_NextStage_Is_NotDone_Returns_False()
    {
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.Salary), "£32,768 a year");
        
        bool shouldNavigate = _onPostJourneyValidator.ShouldNavigateToCheckAnswers(nameof(JourneyStage.Salary));

        Assert.False(shouldNavigate);
    }

    [Fact]
    public void ShouldNavigateToCheckAnswers_When_Stage_Salary_If_NextOptionalStage_DaysWorked_IsNotDone_Returns_False()
    {
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.Salary), "£100.10 a day");
        
        bool shouldNavigate = _onPostJourneyValidator.ShouldNavigateToCheckAnswers(nameof(JourneyStage.Salary));
        
        Assert.False(shouldNavigate);
    }
    
    [Fact]
    public void ShouldNavigateToCheckAnswers_When_Stage_Salary_If_NextOptionalStage_HoursWorked_IsNotDone_Returns_False()
    {
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.Salary), "£20.20 an hour");
        
        bool shouldNavigate = _onPostJourneyValidator.ShouldNavigateToCheckAnswers(nameof(JourneyStage.Salary));
        
        Assert.False(shouldNavigate);
    }
    
    [Theory]
    [InlineData(nameof(JourneyStage.HowManyDaysWorked))]
    [InlineData(nameof(JourneyStage.HowManyHoursWorked))]
    public void ShouldNavigateToCheckAnswers_When_Stage_Optional_If_NextStage_IsDone_Returns_True(string stage)
    {
        _accessor.HttpContext.Session.SetString(stage, "5");
        _accessor.HttpContext.Session.SetString(nameof(JourneyStage.StatePension), "No");
        
        bool shouldNavigate = _onPostJourneyValidator.ShouldNavigateToCheckAnswers(stage);
        
        Assert.True(shouldNavigate);
    }
    
    [Theory]
    [InlineData(nameof(JourneyStage.HowManyDaysWorked))]
    [InlineData(nameof(JourneyStage.HowManyHoursWorked))]
    public void ShouldNavigateToCheckAnswers_When_Stage_Optional_If_NextStage_IsNotDone_Returns_False(string stage)
    {
        _accessor.HttpContext.Session.SetString(stage, "5");
        
        bool shouldNavigate = _onPostJourneyValidator.ShouldNavigateToCheckAnswers(stage);
        
        Assert.False(shouldNavigate);
    }
}