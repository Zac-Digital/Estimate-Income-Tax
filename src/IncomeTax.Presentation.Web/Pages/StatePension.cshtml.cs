using System.ComponentModel.DataAnnotations;
using IncomeTax.Application.Journey.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IncomeTax.Presentation.Web.Pages;

public sealed class StatePension : PageModel
{
    [BindProperty]
    [Required]
    public bool? IsOverStatePensionAge { get; set; }

    public IActionResult OnPost([FromServices] JourneyCommands journey)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        journey.UpdateStatePension(IsOverStatePensionAge!.Value);

        return RedirectToPage("./CheckAnswers");
    }
}