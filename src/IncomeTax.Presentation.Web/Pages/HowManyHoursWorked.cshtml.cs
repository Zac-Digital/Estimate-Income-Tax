using System.ComponentModel.DataAnnotations;
using IncomeTax.Application.Session;
using IncomeTax.Domain.Journey;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IncomeTax.Presentation.Web.Pages;

public sealed class HowManyHoursWorked(SessionService sessionService) : PageModel
{
    [BindProperty]
    [Required]
    [Range(1, 168)]
    public int? HoursWorked { get; set; }

    public IActionResult OnGet()
    {
        string? hoursWorked = sessionService.Get(JourneyStage.HowManyHoursWorked);
        if (hoursWorked is not null) HoursWorked = int.Parse(hoursWorked);
        
        return Page();
    }
    
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        sessionService.Update(JourneyStage.HowManyHoursWorked, $"{HoursWorked}");

        return RedirectToPage(nameof(StatePension));
    }
}