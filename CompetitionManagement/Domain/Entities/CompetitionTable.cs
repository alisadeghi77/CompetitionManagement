using System.ComponentModel.DataAnnotations.Schema;
using CompetitionManagement.Domain.Common;
using CompetitionManagement.Domain.Enums;
using CompetitionManagement.Domain.Validations;
using FluentValidation;

namespace CompetitionManagement.Domain.Entities;

public class CompetitionTable : BaseAuditableEntity
{
    [Column("RegisterParams", TypeName = "jsonb")]
    public List<CompetitionRegisterParam>? RegisterParams { get; private set; } = new();
    
    public long FirstPlayerRegisterId { get; private set; }
    public required CompetitionRegister FirstPlayerRegister { get; set; }

    public long SecondPlayerRegisterId { get; private set; }
    public required CompetitionRegister SecondPlayerRegister { get; set; }

    public TableDetailStatus Status { get; private set; }


    public static CompetitionTable Create(
        CompetitionRegister firstCompetitionRegister,
        CompetitionRegister secondRedCompetitionRegister,
        List<CompetitionRegisterParam> registerParams,
        TableDetailStatus status)
    {
        var details = new CompetitionTable
        {
            
            FirstPlayerRegister = firstCompetitionRegister,
            SecondPlayerRegister = secondRedCompetitionRegister,
            FirstPlayerRegisterId = firstCompetitionRegister.Id,
            SecondPlayerRegisterId = secondRedCompetitionRegister.Id,
            RegisterParams = registerParams,
            Status = status
        };

        new CompetitionTableDetailsValidator().ValidateAndThrow(details);
        return details;
    }
}
