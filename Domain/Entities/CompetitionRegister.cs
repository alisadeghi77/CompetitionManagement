using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Enums;
using Domain.Validations;
using FluentValidation;

namespace Domain.Entities;

public class CompetitionRegister : BaseAuditableEntity
{
    
    public long CompetitionId { get; set; }
    public required Competition Competition { get; set; }
    
    public required string PlayerUserId { get; set; }

    public required ApplicationUser PlayerUser { get; set; }

    public string? CoachPhoneNumber { get; set; }
    
    public string? CoachUserId { get; private set; }
    
    public ApplicationUser? CoachUser { get; private set; }

    public RegisterStatus Status { get; private set; }
    
    
    [Column("RegisterParams", TypeName = "jsonb")]
    public List<CompetitionRegisterParam>? RegisterParams { get; private set; } = new();

    public static CompetitionRegister Create(
        Competition competition,
        ApplicationUser playerUser,
        ApplicationUser coachUser,
        string? coachPhoneNumber,
        IEnumerable<CompetitionRegisterParam> registerParams)
    {
        var register = new CompetitionRegister
        {
            Competition = competition,
            CompetitionId = competition.Id,
            PlayerUser = playerUser,
            PlayerUserId = playerUser.Id,
            CoachPhoneNumber = coachPhoneNumber,
            CoachUserId = coachUser.Id,
            CoachUser = coachUser,
            Status = RegisterStatus.Pending,
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