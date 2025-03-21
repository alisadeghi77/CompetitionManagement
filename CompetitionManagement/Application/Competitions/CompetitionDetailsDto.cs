using CompetitionManagement.Domain.Entities;
using CompetitionManagement.Domain.Enums;

namespace CompetitionManagement.Application.Competitions;

public record CompetitionDetailsDto(
    long Id,
    string Title,
    DateTime Date,
    string Address,
    long BannerImageId,
    long LicenseImageId,
    CompetitionStatus Status,
    List<CompetitionParam>? RegisterParams);