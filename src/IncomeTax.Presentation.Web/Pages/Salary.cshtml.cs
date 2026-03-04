using System.ComponentModel.DataAnnotations;
using System.Linq;
using IncomeTax.Application.Journey.Command;
using IncomeTax.Domain.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IncomeTax.Presentation.Web.Pages;

public class Salary : PageModel
{
    public readonly string[] Options = SalaryFrequencyExtensions.SalaryPageRadioSet;

    [BindProperty]
    [Required(ErrorMessage = nameof(ErrorAmountIsInvalidType))]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = nameof(ErrorAmountIsNotTwoDecimalPlaces))]
    public double? Amount { get; set; }
    
    [BindProperty] 
    [Required] 
    public string Frequency { get; set; } = null!;

    public bool ErrorAmountIsInvalidType { get; private set; }
    public bool ErrorAmountIsNotTwoDecimalPlaces { get; private set; }

    public bool ErrorFrequencyIsNotGiven { get; private set; }

    public IActionResult OnPost([FromServices] JourneyCommands journey)
    {
        if (!ModelState.IsValid)
        {
            ModelErrorCollection? amountErrorCollection = ModelState[nameof(Amount)]?.Errors;

            if (amountErrorCollection is not null)
            {
                ErrorAmountIsInvalidType =
                    amountErrorCollection.Any(e => e.ErrorMessage.Equals(nameof(ErrorAmountIsInvalidType))) 
                    || Amount is null;
                ErrorAmountIsNotTwoDecimalPlaces = amountErrorCollection.Any(e =>
                    e.ErrorMessage.Equals(nameof(ErrorAmountIsNotTwoDecimalPlaces)));
            }

            ErrorFrequencyIsNotGiven = ModelState[nameof(Frequency)]?.Errors.Count > 0;

            return Page();
        }

        journey.UpdateSalary(Amount!.Value, Frequency);

        return RedirectToPage("./StatePension");
    }
}