using IncomeTax.Application.Journey.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IncomeTax.Presentation.Web.Pages;

public class CheckAnswers : PageModel
{
    public string GrossIncome { get; private set; } = null!;
    public string OverStatePensionAge { get; private set; } = null!;
    
    public IActionResult OnGet([FromServices] JourneyQueries journey)
    {
        GrossIncome = journey.GetGrossIncome();
        OverStatePensionAge = journey.GetIsOverStatePensionAge();
        
        return Page();
    }
}