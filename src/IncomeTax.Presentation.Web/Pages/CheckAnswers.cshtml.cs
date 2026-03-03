using System.Text;
using IncomeTax.Application.Journey.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IncomeTax.Presentation.Web.Pages;

public class CheckAnswers : PageModel
{
    public const string NotProvided = "Not provided";
    
    public string GrossIncome { get; private set; } = null!;
    public string OverStatePensionAge { get; private set; } = null!;
    
    public string? PayingScottishTax { get; private set; }
    
    public string? PensionContributionDescriptor { get; private set; }
    public string? PensionContribution { get; private set; }
    
    public IActionResult OnGet([FromServices] JourneyQueries journey)
    {
        GrossIncome = journey.GetGrossIncome();
        OverStatePensionAge = journey.GetIsOverStatePensionAge();

        bool? isPayingScottishTax = journey.GetIsPayingScottishTax();
        PayingScottishTax = ParseIsPayingScottishTax(isPayingScottishTax);
        
        string? pensionContributionDescriptor = journey.GetPensionContributionDescriptor();
        string? pensionContribution = journey.GetPensionContribution();
        PensionContributionDescriptor = pensionContributionDescriptor;
        PensionContribution = ParsePensionContribution(pensionContributionDescriptor, pensionContribution);
        
        return Page();
    }

    private static string? ParseIsPayingScottishTax(bool? isPayingScottishTax)
    {
        if (isPayingScottishTax is null) return null;
        return isPayingScottishTax.Value ? "Yes" : "No";
    }

    private static string? ParsePensionContribution(string? pensionDescriptor, string? pensionContribution)
    {
        if (pensionDescriptor is null || pensionContribution is null) return null;

        StringBuilder pensionString = new(pensionContribution);
        if (pensionDescriptor.Equals("£")) pensionString.Insert(0, '£');
        else pensionString.Append('%');
        
        return pensionString.ToString();
    }
}