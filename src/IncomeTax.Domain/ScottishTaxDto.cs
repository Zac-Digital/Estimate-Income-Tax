namespace IncomeTax.Domain;

public sealed record ScottishTaxDto(bool? IsPayingScottishTax) : JourneyDto;