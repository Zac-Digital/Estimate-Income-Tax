using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IncomeTax.Presentation.Web.Pages;

public class YourPay : PageModel
{
    public readonly string[] Options = ["Yearly", "Monthly", "Every 4 weeks", "Weekly", "Daily", "Hourly"];

    [BindProperty] 
    [Required]
    public double? Cost { get; set; }
    
    [BindProperty] 
    public string Frequency { get; set; } = null!;
    
    public bool ErrorCost { get; private set; }
    public bool ErrorFrequency { get; private set; }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            ErrorCost = ModelState[nameof(Cost)]?.Errors.Count > 0;
            ErrorFrequency = ModelState[nameof(Frequency)]?.Errors.Count > 0;
            
            return Page();
        }

        return Page(); // TODO: Next Page
    }
}