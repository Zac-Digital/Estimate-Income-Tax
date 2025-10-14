namespace IncomeTax.Domain;

public sealed record StatePensionDto(bool IsOverStatePensionAge) : JourneyDto;