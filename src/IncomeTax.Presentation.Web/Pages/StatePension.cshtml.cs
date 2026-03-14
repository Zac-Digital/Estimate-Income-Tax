using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using IncomeTax.Application.Journey.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IncomeTax.Presentation.Web.Pages;

[ExcludeFromCodeCoverage(Justification = "OnPost Logic is Tested by Functional Test Suite")]
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