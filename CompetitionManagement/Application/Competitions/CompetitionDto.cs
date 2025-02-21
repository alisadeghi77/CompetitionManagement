namespace CompetitionManagement.Application.Competitions;

public record CompetitionDto(
        long Id,
        long CompetitionDefinitionId,
        long AgeGroupId,
        int Weight,
        string Style);