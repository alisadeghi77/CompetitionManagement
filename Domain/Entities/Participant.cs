using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Enums;
using Domain.Validations;
using FluentValidation;

namespace Domain.Entities;

public class Participant : BaseAuditableEntity
{
    public long CompetitionId { get; set; }
    public required Competition Competition { get; set; }
    
    public required string ParticipantUserId { get; set; }

    public required ApplicationUser ParticipantUser { get; set; }

    public string? CoachPhoneNumber { get; set; }
    
    public string? CoachUserId { get; private set; }
    
    public ApplicationUser? CoachUser { get; private set; }

    public RegisterStatus Status { get; private set; }
    
    
    [Column("RegisterParams", TypeName = "jsonb")]
    public List<ParticipantParam>? RegisterParams { get; private set; } = new();

    public static Participant Create(
        Competition competition,
        ApplicationUser participantUser,
        ApplicationUser coachUser,
        string? coachPhoneNumber,
        IEnumerable<ParticipantParam> registerParams)
    {
        var register = new Participant
        {
            Competition = competition,
            CompetitionId = competition.Id,
            ParticipantUser = participantUser,
            ParticipantUserId = participantUser.Id,
            CoachPhoneNumber = coachPhoneNumber,
            CoachUserId = coachUser.Id,
            CoachUser = coachUser,
            Status = RegisterStatus.Pending,
        };

        new ParticipantValidator().ValidateAndThrow(register);
        
        return register;
    }
}


public record ParticipantParam
{
    public required string Key { get; set; }
    public required string Value { get; set; }
}