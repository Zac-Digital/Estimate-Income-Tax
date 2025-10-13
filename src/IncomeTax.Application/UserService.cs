using System.Globalization;
using System.Text;
using IncomeTax.Domain;

namespace IncomeTax.Application;

public sealed class UserService
{
    private readonly UserDto _userDto = new();

    public void UpdateSalary(double amount, string frequency)
    {
        _userDto.Salary ??= new SalaryDto();

        _userDto.Salary.Amount = amount;
        _userDto.Salary.Frequency = frequency;
    }

    public void UpdateStatePension(bool isOverStatePensionAge)
    {
        _userDto.StatePension ??= new StatePensionDto();

        _userDto.StatePension.IsOverStatePensionAge = isOverStatePensionAge;
    }

    public string GetGrossIncome()
    {
        StringBuilder grossIncomeBuilder =
            new StringBuilder(
                _userDto.Salary?.Amount
                .ToString("C", CultureInfo.CreateSpecificCulture("en-GB"))
                .Replace(".00", "")
                );

        switch (_userDto.Salary?.Frequency)
        {
            case "Yearly":
                grossIncomeBuilder.Append(" a year");
                break;
            case "Monthly":
                grossIncomeBuilder.Append(" a month");
                break;
            case "Every 4 weeks":
                grossIncomeBuilder.Append(" every 4 weeks");
                break;
            case "Weekly":
                grossIncomeBuilder.Append(" a week");
                break;
            case "Daily":
                grossIncomeBuilder.Append(" a day");
                break;
            case "Hourly":
                grossIncomeBuilder.Append(" an hour");
                break;
        }

        return grossIncomeBuilder.ToString();
    }

    public string GetIsOverStatePensionAge() => _userDto.StatePension!.IsOverStatePensionAge ? "Yes" : "No";
}