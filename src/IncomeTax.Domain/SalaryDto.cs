namespace IncomeTax.Domain;

public sealed class SalaryDto
{
    public double Amount { get; set; }
    public string Frequency { get; set; } = null!;
}