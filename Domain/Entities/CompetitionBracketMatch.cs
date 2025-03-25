using Domain.Common;

namespace Domain.Entities;

public class CompetitionBracketMatch : BaseAuditableEntity
{
    private CompetitionBracketMatch(
        long competitionBracketId,
        string keyParams,
        long? firstParticipantId,
        long? secondParticipantId,
        long? nextMatchId,
        RoundType round,
        int matchNumberPosition)
    {
        CompetitionBracketId = competitionBracketId;
        KeyParams = keyParams;
        FirstParticipantId = firstParticipantId;
        SecondParticipantId = secondParticipantId;
        NextMatchId = nextMatchId ?? null;
        Round = round;
        MatchNumberPosition = matchNumberPosition;
    }

    public long CompetitionBracketId { get; private set; }
    public CompetitionBracket CompetitionBracket { get; private set; }

    public string KeyParams { get; private set; }

    public long? FirstParticipantId { get; private set; }
    public Participant? FirstParticipant { get; set; }

    public long? SecondParticipantId { get; private set; }
    public Participant? SecondParticipant { get; set; }


    public long? WinnerParticipantId { get; private set; }
    public Participant WinnerParticipant { get; set; }

    public long? NextMatchId { get; private set; }
    public CompetitionBracketMatch? NextMatch { get; private set; }

    /// <summary>
    /// 32, 16, 8, 4, 2, 1
    /// </summary>
    public RoundType Round { get; private set; }

    /// <summary>
    /// show match position in front
    /// </summary>
    public int MatchNumberPosition { get; private set; }

    public static CompetitionBracketMatch Create(
        CompetitionBracket competitionBracket,
        Participant? firstParticipant,
        Participant? secondParticipant,
        CompetitionBracketMatch? nextMatch,
        RoundType round,
        int matchNumberPosition)
    {
        var model = new CompetitionBracketMatch(
            competitionBracket.Id,
            competitionBracket.KeyParams,
            firstParticipant?.Id,
            secondParticipant?.Id,
            nextMatch?.Id,
            round,
            matchNumberPosition)
        {
            CompetitionBracket = competitionBracket,
            FirstParticipant = firstParticipant,
            SecondParticipant = secondParticipant,
            NextMatch = nextMatch
        };
        
        //TODO: validate competitionBracket should have key, round and position number should be valid  

        return model;
    }
}