using CompetitionManagement.Domain.Common;
using CompetitionManagement.Domain.Validations;
using FluentValidation;

namespace CompetitionManagement.Domain.Entities;

public class CompetitionDefinition : BaseAuditableEntity
{
    public required string PlannerUserId { get; set; }

    public required ApplicationUser PlannerUser { get; set; }
    public required string Title { get; set; }
    public DateTime Date { get; private set; }
    public required string Address { get; set; }
    public long BannerImageId { get; private set; }
    public long CertificateImageId { get; private set; }

    public virtual List<AgeGroup> AgeGroups { get; set; } = new();

    public static CompetitionDefinition Create(
        ApplicationUser plannerUser,
        string title,
        DateTime date,
        string address,
        long bannerImageId,
        long certificateImageId)
    {
        var competition = new CompetitionDefinition
        {
            Title = title,
            Address = address,
            PlannerUser = plannerUser,
            PlannerUserId = plannerUser.Id,
            Date = date,
            BannerImageId = bannerImageId,
            CertificateImageId = certificateImageId
        };

        new CompetitionValidator().ValidateAndThrow(competition);
        return competition;
    }
}
