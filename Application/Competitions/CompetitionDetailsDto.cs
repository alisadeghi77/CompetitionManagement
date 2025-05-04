using Domain.Entities;
using Domain.Enums;

namespace Application.Competitions;

public record CompetitionDetailsDto(
    long Id,
    string Title,
    DateTime Date,
    string Address,
    long BannerImageId,
    long LicenseImageId,
    CompetitionStatus Status,
    bool IsVisible,
    bool CanRegister,    
    CompetitionParam? RegisterParams);