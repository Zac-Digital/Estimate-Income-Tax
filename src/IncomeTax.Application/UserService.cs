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
}