using CompetitionManagement.Domain.Enums;

namespace CompetitionManagement.Application.Competitions;

public record CompetitionDto(
    long Id,
    string Title,
    DateTime Date,
    string Address,
    long BannerImageId,
    long LicenseImageId,
    CompetitionStatus Status);
