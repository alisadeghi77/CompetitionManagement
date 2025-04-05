using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Enums;
using Domain.Validations;
using FluentValidation;

namespace Domain.Entities;

public class Competition : BaseAuditableEntity
{
    private readonly List<Participant> _participants = new();
    private List<Bracket>? _brackets = new();

    public required string PlannerUserId { get; set; }
    public required ApplicationUser PlannerUser { get; set; }
    public required string Title { get; set; }
    public DateTime Date { get; private set; }
    public required string Address { get; set; }
    public CompetitionStatus Status { get; set; }
    public long BannerImageId { get; private set; }
    public long LicenseImageId { get; private set; }
    public bool IsVisible { get; private set; } = false;
    public bool CanRegister { get; private set; } = false;


    [Column("RegisterParams", TypeName = "jsonb")]
    public CompetitionParam RegisterParams { get; private set; }

    public IReadOnlyCollection<Participant> Participants => _participants;
    public IReadOnlyCollection<Bracket> Brackets => _brackets;

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
            Status = CompetitionStatus.PendToAdminApprove
        };

        new CompetitionValidator().ValidateAndThrow(competition);
        return competition;
    }


    public void SetRegisterParams(CompetitionParam param) => RegisterParams = param;

    public void ChangeRegistrationStatus(bool canRegister) => CanRegister = canRegister;
    public void ChangeVisibility(bool canVisitOnSite) => IsVisible = canVisitOnSite;

    public void SetApprove() => Status = CompetitionStatus.PendToStart;
    public void SetOnProgress() => Status = CompetitionStatus.OnProgress;
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