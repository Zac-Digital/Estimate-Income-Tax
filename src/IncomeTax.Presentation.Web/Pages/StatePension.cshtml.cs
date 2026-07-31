using System.ComponentModel.DataAnnotations;
using IncomeTax.Application.Session;
using IncomeTax.Domain.Journey;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IncomeTax.Presentation.Web.Pages;

public class StatePension(SessionService sessionService) : PageModel
{
    [BindProperty]
    [Required]
    public bool? Pensioner { get; set; }

    public IActionResult OnGet()
    {
        string? pensioner = sessionService.Get(JourneyStage.StatePension);
        if (pensioner is not null) Pensioner = pensioner.Equals("Yes");
        
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        sessionService.Update(JourneyStage.StatePension, Pensioner is not null && Pensioner.Value ? "Yes" : "No");

        return RedirectToPage(nameof(JourneyStage.CheckAnswers));
    }
}