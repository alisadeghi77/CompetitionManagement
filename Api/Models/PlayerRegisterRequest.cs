namespace Api.Models;

public record RegisterParticipantRequest(
    string? CoachId,
    string? CoachPhoneNumber,
    long CompetitionId,
    List<(string Key, string Value)> Params);