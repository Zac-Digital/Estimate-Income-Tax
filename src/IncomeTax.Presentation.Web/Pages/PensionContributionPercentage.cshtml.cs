using IncomeTax.Application.Journey.Command;
using IncomeTax.Application.Journey.Query;
using IncomeTax.Domain.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IncomeTax.Presentation.Web.Pages;

public sealed class PensionContributionPercentage : PageModel
{
    [BindProperty]
    public string? PensionContribution { get; set; }
    
    public void OnGet([FromServices] JourneyQueries journey)
    {
        PensionContribution = journey.GetPensionContribution();
    }

    public void OnGetSwap([FromServices] JourneyCommands journey)
    {
        journey.DeletePensionContribution();
    }

    public IActionResult OnPost([FromServices] JourneyCommands journey)
    {
        if (string.IsNullOrWhiteSpace(PensionContribution))
        {
            journey.DeletePensionContribution();
        }
        else
        {
            journey.UpdatePensionContribution(PensionContribution, PensionDescriptor.Percentage);
        }
        
        return RedirectToPage("./CheckAnswers");
    }
}