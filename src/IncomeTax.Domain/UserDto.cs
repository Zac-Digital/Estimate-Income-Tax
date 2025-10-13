namespace IncomeTax.Domain;

public sealed class UserDto
{
    public SalaryDto? Salary { get; set; }
    
    public StatePensionDto? StatePension { get; set; }
}