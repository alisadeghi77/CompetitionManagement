using CompetitionManagement.Domain.Common;
using CompetitionManagement.Domain.Enums;
using CompetitionManagement.Domain.Validations;
using FluentValidation;

namespace CompetitionManagement.Domain.Entities;

public class CompetitionTableDetail : BaseAuditableEntity
{
    public long CompetitionTableId { get; private set; }
    public required Competition Competition { get; set; }

    public long FirstCompetitionRegisterId { get; private set; }
    public required CompetitionRegister FirstCompetitionRegister { get; set; }

    public long SecondRedCompetitionRegisterId { get; private set; }
    public required CompetitionRegister SecondRedCompetitionRegister { get; set; }

    public TableDetailStatus Status { get; private set; }


    public static CompetitionTableDetail Create(
        CompetitionRegister firstCompetitionRegister,
        CompetitionRegister secondRedCompetitionRegister,
        Competition competition,
        TableDetailStatus status)
    {
        var details = new CompetitionTableDetail
        {
            FirstCompetitionRegister = firstCompetitionRegister,
            SecondRedCompetitionRegister = secondRedCompetitionRegister,
            Competition = competition,
            CompetitionTableId = competition.Id,
            FirstCompetitionRegisterId = firstCompetitionRegister.Id,
            SecondRedCompetitionRegisterId = secondRedCompetitionRegister.Id,
            Status = status
        };

        new CompetitionTableDetailsValidator().ValidateAndThrow(details);
        return details;
    }
}
