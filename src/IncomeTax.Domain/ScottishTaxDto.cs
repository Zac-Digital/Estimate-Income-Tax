namespace IncomeTax.Domain;

public record ScottishTaxDto(bool? IsPayingScottishTax) : JourneyDto;