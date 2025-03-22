namespace Api.Models;

public record PlayerRegisterRequest(
    string? CoachId,
    string? CoachPhoneNumber,
    long CompetitionId,
    List<(string Key, string Value)> Params);