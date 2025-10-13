using IncomeTax.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IncomeTax.Presentation.Web.Pages;

public class CheckAnswers : PageModel
{
    public string GrossIncome { get; private set; } = null!;
    public string OverStatePensionAge { get; private set; } = null!;
    
    public IActionResult OnGet([FromServices] UserService userService)
    {
        GrossIncome = userService.GetGrossIncome();
        OverStatePensionAge = userService.GetIsOverStatePensionAge();
        
        return Page();
    }
}