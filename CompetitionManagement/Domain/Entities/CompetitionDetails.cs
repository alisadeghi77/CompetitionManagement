using CompetitionManagement.Domain.Common;
using CompetitionManagement.Domain.Validations;
using FluentValidation;

namespace CompetitionManagement.Domain.Entities;

public class CompetitionDetails : BaseAuditableEntity
{
    public long CompetitionId { get; private set; }
    public required Competition Competition { get; set; }
    public long AgeGroupId { get; private set; }
    public required AgeGroup AgeGroup { get; set; }
    public int Weight { get; private set; }
    public required string Style { get; set; }
    public virtual List<CompetitionTableDetail> CompetitionTableDetails { get; set; } = new();

    public static CompetitionDetails Create(
        Competition competition,
        AgeGroup ageGroup,
        int weight,
        string styles)
    {
        var table = new CompetitionDetails
        {
            Competition = competition,
            CompetitionId = competition.Id,
            Style = styles,
            AgeGroup = ageGroup,
            AgeGroupId = ageGroup.Id,
            Weight = weight
        };

        new CompetitionTableValidator().ValidateAndThrow(table);
        return table;
    }
}
