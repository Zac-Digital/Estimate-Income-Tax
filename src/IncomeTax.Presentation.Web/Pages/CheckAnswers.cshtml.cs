using IncomeTax.Application.Session;
using IncomeTax.Domain.Journey;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IncomeTax.Presentation.Web.Pages;

public sealed class CheckAnswers(SessionService sessionService) : PageModel
{
    public string? GrossIncome { get; set; }
    public string? DaysWorkedPerWeek { get; set; }
    public string? HoursWorkedPerWeek { get; set; }
    public string? OverStatePensionAge { get; set; }

    public const string TaxCode = "1257L";
    public string? ScottishIncomeTax { get; set; }
    public string? PensionContributions { get; set; }
    public string? StudentLoan { get; set; }
    public string? PostgraduateLoan { get; set; }
    
    public IActionResult OnGet()
    {
        GrossIncome = sessionService.Get(JourneyStage.Salary);
        OverStatePensionAge = sessionService.Get(JourneyStage.StatePension);
        DaysWorkedPerWeek = sessionService.Get(JourneyStage.HowManyDaysWorked);
        HoursWorkedPerWeek = sessionService.Get(JourneyStage.HowManyHoursWorked);

        return Page();
    }
}