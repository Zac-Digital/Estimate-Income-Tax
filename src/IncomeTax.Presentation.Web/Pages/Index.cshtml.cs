using IncomeTax.Domain.Journey;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IncomeTax.Presentation.Web.Pages;

public sealed class IndexModel : PageModel
{
    public IActionResult OnPost()
    {
        return RedirectToPage(nameof(JourneyStage.Salary));
    }
}