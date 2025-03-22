namespace Api.Models;

public record CompetitionDefinitionRequest(
    string CompetitionTitle,
    DateTime CompetitionDate,
    string CompetitionAddress,
    long? LicenseFileId,
    long? BannerFileId);