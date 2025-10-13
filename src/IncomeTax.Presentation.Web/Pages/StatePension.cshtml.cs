using System.ComponentModel.DataAnnotations;
using IncomeTax.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IncomeTax.Presentation.Web.Pages;

public class StatePension : PageModel
{
    [BindProperty]
    [Required]
    public bool? IsOverStatePensionAge { get; set; }

    public IActionResult OnPost([FromServices] UserService userService)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        userService.UpdateStatePension(IsOverStatePensionAge!.Value);

        return RedirectToPage("./CheckAnswers");
    }
}