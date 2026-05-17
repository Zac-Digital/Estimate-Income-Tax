using System.ComponentModel.DataAnnotations;
using IncomeTax.Application.Session;
using IncomeTax.Domain.Journey;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IncomeTax.Presentation.Web.Pages;

public sealed class HowManyDaysWorked(SessionService sessionService) : PageModel
{
    [BindProperty]
    [Required]
    [Range(1, 7)]
    public int? DaysWorked { get; set; }

    public IActionResult OnGet()
    {
        string? daysWorked = sessionService.Get(JourneyStage.HowManyDaysWorked);
        if (daysWorked is not null) DaysWorked = int.Parse(daysWorked);
        
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        sessionService.Update(JourneyStage.HowManyDaysWorked, $"{DaysWorked}");

        return RedirectToPage(nameof(StatePension));
    }
}