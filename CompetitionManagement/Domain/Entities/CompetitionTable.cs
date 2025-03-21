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
    
    public long FirstCompetitionRegisterId { get; private set; }
    public required CompetitionRegister FirstCompetitionRegister { get; set; }

    public long SecondCompetitionRegisterId { get; private set; }
    public required CompetitionRegister SecondCompetitionRegister { get; set; }

    public TableDetailStatus Status { get; private set; }


    public static CompetitionTable Create(
        CompetitionRegister firstCompetitionRegister,
        CompetitionRegister secondRedCompetitionRegister,
        List<CompetitionRegisterParam> registerParams,
        TableDetailStatus status)
    {
        var details = new CompetitionTable
        {
            
            FirstCompetitionRegister = firstCompetitionRegister,
            SecondCompetitionRegister = secondRedCompetitionRegister,
            FirstCompetitionRegisterId = firstCompetitionRegister.Id,
            SecondCompetitionRegisterId = secondRedCompetitionRegister.Id,
            RegisterParams = registerParams,
            Status = status
        };

        new CompetitionTableDetailsValidator().ValidateAndThrow(details);
        return details;
    }
}
