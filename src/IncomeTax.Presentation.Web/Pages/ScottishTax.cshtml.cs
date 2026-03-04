using IncomeTax.Application.Journey.Command;
using IncomeTax.Application.Journey.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IncomeTax.Presentation.Web.Pages;

public sealed class ScottishTax : PageModel
{
    [BindProperty]
    public bool? IsPayingScottishTax { get; set; }

    public void OnGet([FromServices] JourneyQueries journey)
    {
        IsPayingScottishTax = journey.GetIsPayingScottishTax();
    }
    
    public IActionResult OnPost([FromServices] JourneyCommands journey)
    {
        if (ModelState.IsValid)
        {
            journey.UpdateScottishTax(IsPayingScottishTax); 
        }
        
        return RedirectToPage("./CheckAnswers");
    }
}