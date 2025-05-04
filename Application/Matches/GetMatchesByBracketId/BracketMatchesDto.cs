using Domain.Enums;

namespace Application.Matches.GetMatchesByBracketId;

public record BracketMatchesDto(
    Guid Id,
    RoundType Round,
    int MatchNumberPosition,
    
    // First Participant Info
    long? FirstParticipantId,
    string? FirstParticipantFullName,
    string? FirstParticipantCoachId,
    string? FirstParticipantCoachFullName,
    bool IsFirstParticipantBye,
    
    // Second Participant Info
    long? SecondParticipantId,
    string? SecondParticipantFullName,
    string? SecondParticipantCoachId,
    string? SecondParticipantCoachFullName,
    bool IsSecondParticipantBye,
    
    // Winner Participant Info
    long? WinnerParticipantId,
    string? WinnerParticipantFullName,
    string? WinnerParticipantCoachId,
    string? WinnerParticipantCoachFullName);
