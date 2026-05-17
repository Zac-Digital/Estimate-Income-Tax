using System.ComponentModel.DataAnnotations;
using System.Globalization;
using IncomeTax.Application.Common;
using IncomeTax.Application.Session;
using IncomeTax.Domain.Journey;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IncomeTax.Presentation.Web.Pages;

public sealed class Salary(SessionService sessionService) : PageModel
{
    [BindProperty]
    [Required]
    [RegularExpression(@"^\d+(\.\d{1,2})?$")]
    public double? Amount { get; set; }

    [BindProperty] [Required] public string? Period { get; set; }

    public void OnGet()
    {
        (Amount, Period) = SalarySplit.Split(sessionService.Get(JourneyStage.Salary));
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        sessionService.Update(JourneyStage.Salary,
            $"{Amount?.ToString("C", CultureInfo.CreateSpecificCulture("en-GB")).Replace(".00", "")} {Period}");

        switch (Period?.ToUpperInvariant())
        {
            case "A DAY":
                sessionService.Remove(JourneyStage.HowManyHoursWorked);
                return RedirectToPage(nameof(JourneyStage.HowManyDaysWorked));
            case "AN HOUR":
                sessionService.Remove(JourneyStage.HowManyDaysWorked);
                return RedirectToPage(nameof(JourneyStage.HowManyHoursWorked));
            default:
                sessionService.Remove(JourneyStage.HowManyDaysWorked);
                sessionService.Remove(JourneyStage.HowManyHoursWorked);
                return RedirectToPage(nameof(JourneyStage.StatePension));
        }
    }
}