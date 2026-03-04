using IncomeTax.Domain.Constant;

namespace IncomeTax.Domain;

public sealed record PensionContributionDto(string? PensionContribution, PensionDescriptor Descriptor) : JourneyDto;