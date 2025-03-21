namespace CompetitionManagement.WebApi.ApiModel;

public record CompetitionDefinitionRequest(
    string CompetitionTitle,
    DateTime CompetitionDate,
    string CompetitionAddress,
    long? LicenseFileId,
    long? BannerFileId);