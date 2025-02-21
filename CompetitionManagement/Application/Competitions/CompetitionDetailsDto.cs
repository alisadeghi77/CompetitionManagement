namespace CompetitionManagement.Application.Competitions;

public record CompetitionDetailsDto(
        long Id,
        long CompetitionDefinitionId,
        long AgeGroupId,
        int Weight,
        string Style);