using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IncomeTax.Presentation.Web.Pages;

public class Salary : PageModel
{
    public readonly string[] Options = ["Yearly", "Monthly", "Every 4 weeks", "Weekly", "Daily", "Hourly"];

    [BindProperty] 
    [Required]
    public double? Amount { get; set; }
    
    [BindProperty] 
    public string Frequency { get; set; } = null!;
    
    public bool ErrorAmount { get; private set; }
    public bool ErrorFrequency { get; private set; }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            ErrorAmount = ModelState[nameof(Amount)]?.Errors.Count > 0;
            ErrorFrequency = ModelState[nameof(Frequency)]?.Errors.Count > 0;
            
            return Page();
        }

        return Page(); // TODO: Next Page
    }
}