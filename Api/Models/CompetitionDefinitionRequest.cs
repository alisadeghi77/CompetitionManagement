namespace Api.Models;

public record CompetitionDefinitionRequest(
    string Title,
    DateTime Date,
    string Address,
    long? LicenseImageId,
    long? BannerImageId);