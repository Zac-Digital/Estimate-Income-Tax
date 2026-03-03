using IncomeTax.Domain.Constant;

namespace IncomeTax.Domain;

public sealed record SalaryDto(double Amount, SalaryFrequency Frequency) : JourneyDto;