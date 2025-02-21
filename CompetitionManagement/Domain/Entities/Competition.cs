using CompetitionManagement.Domain.Common;
using CompetitionManagement.Domain.Validations;
using FluentValidation;

namespace CompetitionManagement.Domain.Entities;

public class Competition : BaseAuditableEntity
{
    public long CompetitionDefinitionId { get; private set; }
    public required CompetitionDefinition CompetitionDefinition { get; set; }
    public long AgeGroupId { get; private set; }
    public required AgeGroup AgeGroup { get; set; }
    public int Weight { get; private set; }
    public required string Style { get; set; }
    public virtual List<CompetitionTableDetail> CompetitionTableDetails { get; set; } = new();

    public static Competition Create(
        CompetitionDefinition competitionDefinition,
        AgeGroup ageGroup,
        int weight,
        string styles)
    {
        var table = new Competition
        {
            CompetitionDefinition = competitionDefinition,
            CompetitionDefinitionId = competitionDefinition.Id,
            Style = styles,
            AgeGroup = ageGroup,
            AgeGroupId = ageGroup.Id,
            Weight = weight
        };

        new CompetitionTableValidator().ValidateAndThrow(table);
        return table;
    }
}
