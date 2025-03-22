using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Enums;
using Domain.Validations;
using FluentValidation;

namespace Domain.Entities;

public class CompetitionTable : BaseAuditableEntity
{
    [Column("RegisterParams", TypeName = "jsonb")]
    public List<ParticipantParam>? RegisterParams { get; private set; } = new();
    
    public long FirstParticipantId { get; private set; }
    public required Participant FirstParticipant { get; set; }

    public long SecondParticipantId { get; private set; }
    public required Participant SecondParticipant { get; set; }

    public TableDetailStatus Status { get; private set; }


    public static CompetitionTable Create(
        Participant firstParticipant,
        Participant secondRedParticipant,
        List<ParticipantParam> registerParams,
        TableDetailStatus status)
    {
        var details = new CompetitionTable
        {
            
            FirstParticipant = firstParticipant,
            SecondParticipant = secondRedParticipant,
            FirstParticipantId = firstParticipant.Id,
            SecondParticipantId = secondRedParticipant.Id,
            RegisterParams = registerParams,
            Status = status
        };

        new CompetitionTableDetailsValidator().ValidateAndThrow(details);
        return details;
    }
}
