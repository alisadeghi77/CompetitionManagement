using System.ComponentModel.DataAnnotations.Schema;
using CompetitionManagement.Domain.Common;
using CompetitionManagement.Domain.Enums;
using CompetitionManagement.Domain.Validations;
using FluentValidation;

namespace CompetitionManagement.Domain.Entities;

public class CompetitionRegister : BaseAuditableEntity
{
    public required string AthleteUserId { get; set; }

    public required ApplicationUser AthleteUser { get; set; }

    public required string CoachPhoneNumber { get; set; }
    
    public string? CoachUserId { get; private set; }
    
    public ApplicationUser? CoachUser { get; private set; }

    public RegisterStatus Status { get; private set; }
    
    
    [Column("RegisterParams", TypeName = "jsonb")]
    public List<CompetitionRegisterParam>? RegisterParams { get; private set; } = new();

    public static CompetitionRegister Create(
        ApplicationUser athleteUser,
        ApplicationUser coachUser,
        string coachPhoneNumber,
        RegisterStatus status,
        int weight,
        string styles)
    {
        var register = new CompetitionRegister
        {
            AthleteUser = athleteUser,
            AthleteUserId = athleteUser.Id,
            CoachPhoneNumber = coachPhoneNumber,
            CoachUserId = coachUser.Id,
            CoachUser = coachUser,
            Status = status,
        };

        new CompetitionRegisterValidator().ValidateAndThrow(register);
        return register;
    }
}


public record CompetitionRegisterParam
{
    public required string Key { get; set; }
    public required string Value { get; set; }
}