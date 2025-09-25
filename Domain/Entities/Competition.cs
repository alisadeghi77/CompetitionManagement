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
    public CompetitionParam? RegisterParams { get; private set; }

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

    public List<List<ParticipantParam>> GetParams() => GenerateAllCombinations(RegisterParams);

    public List<ParamsCombination> GetParamsWithKeys()
    {
        var combinationResult = GenerateAllCombinations(RegisterParams);
        return combinationResult.Select(s => new ParamsCombination(s, Bracket.GenerateKey(s))).ToList();
    }

    private List<List<ParticipantParam>> GenerateAllCombinations(CompetitionParam param,
        List<ParticipantParam>? currentPath = null)
    {
        var results = new List<List<ParticipantParam>>();
        currentPath ??= new List<ParticipantParam>();

        if (param.Values == null || !param.Values.Any())
            return results;

        foreach (var value in param.Values)
        {
            var path = new List<ParticipantParam>(currentPath)
            {
                new ParticipantParam { Key = param.Key, Value = value.Key }
            };

            if (value.Params == null || !value.Params.Any())
            {
                results.Add(path);
                continue;
            }

            var paramResults = new List<List<ParticipantParam>> { path };
            foreach (var nestedParam in value.Params)
            {
                var newResults = new List<List<ParticipantParam>>();
                foreach (var currentResult in paramResults)
                {
                    var nestedCombinations = GenerateAllCombinations(nestedParam, currentResult);
                    if (nestedCombinations.Any())
                        newResults.AddRange(nestedCombinations);
                    else
                        newResults.Add(currentResult);
                }

                paramResults = newResults;
            }

            results.AddRange(paramResults);
        }

        return results;
    }

    public void Update(string title, DateTime date, string address, long bannerFileId, long licenseFileId)
    {
        Title = title;
        Date = date;
        Address = address;
        BannerImageId = bannerFileId;
        LicenseImageId = licenseFileId;
    }
}

public record ParamsCombination(List<ParticipantParam> ParamsList, string Key);

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