using CompetitionManagement.Domain.Common;
using CompetitionManagement.Domain.Validations;
using FluentValidation;

namespace CompetitionManagement.Domain.Entities;

public class AgeGroup : BaseAuditableEntity
{
    public long CompetitionDefinitionId { get; private set; }

    public required CompetitionDefinition CompetitionDefinition { get; set; }
    public required string Title { get;  set; }
    public required List<int> Weights { get; set; }
    public required List<string> Styles { get; set; }

    public static AgeGroup Create(CompetitionDefinition competitionDefinition, string title, List<int> weights, List<string> styles)
    {
        var ageGroup = new AgeGroup
        {
            Title = title,
            Weights = weights,
            Styles = styles,
            CompetitionDefinition = competitionDefinition,
            CompetitionDefinitionId = competitionDefinition.Id,
        };

        new AgeGroupValidator().ValidateAndThrow(ageGroup);
        return ageGroup;
    }
}
