using IncomeTax.Domain.Constant;

namespace IncomeTax.Domain;

public record PensionContributionDto(string? PensionContribution, PensionDescriptor Descriptor) : JourneyDto;