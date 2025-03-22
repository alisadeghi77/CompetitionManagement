using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Enums;
using Domain.Validations;
using FluentValidation;

namespace Domain.Entities;

public class Competition : BaseAuditableEntity
{
    private readonly List<Participant> _participants = new();
    
    public required string PlannerUserId { get; set; }
    public required ApplicationUser PlannerUser { get; set; }
    public required string Title { get; set; }
    public DateTime Date { get; private set; }
    public required string Address { get; set; }
    public CompetitionStatus Status { get; set; }
    public long BannerImageId { get; private set; }
    public long LicenseImageId { get; private set; }

    
    [Column("RegisterParams", TypeName = "jsonb")]
    public List<CompetitionParam>? RegisterParams { get; private set; } = new();
    public IReadOnlyCollection<Participant> Participants => _participants; 

    public static Competition Create(
        ApplicationUser plannerUser,
        string title,
        DateTime date,
        string address,
        long bannerImageId,
        long licenseImageId)
    {
        var competition = new Competition
        {
            Title = title,
            Address = address,
            PlannerUser = plannerUser,
            PlannerUserId = plannerUser.Id,
            Date = date,
            BannerImageId = bannerImageId,
            LicenseImageId = licenseImageId,
            Status = CompetitionStatus.PendToStart
        };

        new CompetitionValidator().ValidateAndThrow(competition);
        return competition;
    }
}

public record CompetitionParam
{
    public required string Key { get; set; }
    public required string Title { get; set; }
    public List<CompetitionParamValue>? Values { get; set; }
}

public record CompetitionParamValue
{
    public required string Key { get; set; }
    public required string Title { get; set; }
    public List<CompetitionParam>? Params { get; set; }
}