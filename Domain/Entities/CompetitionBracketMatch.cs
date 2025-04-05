using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class CompetitionBracketMatch : BaseEntity<Guid>
{
    private CompetitionBracketMatch(
        long competitionBracketId,
        string keyParams,
        RoundType round,
        int matchNumberPosition)
    {
        CompetitionBracketId = competitionBracketId;
        KeyParams = keyParams;
        Round = round;
        MatchNumberPosition = matchNumberPosition;
    }

    public long CompetitionBracketId { get; private set; }
    public CompetitionBracket CompetitionBracket { get; private set; }

    public string KeyParams { get; private set; }

    public long? FirstParticipantId { get; private set; }
    public Participant? FirstParticipant { get; set; }
    public bool IsFirstParticipantBye { get; set; }

    public long? SecondParticipantId { get; private set; }
    public Participant? SecondParticipant { get; set; }
    public bool IsSecondParticipantBye { get; set; }

    public long? WinnerParticipantId { get; private set; }
    public Participant WinnerParticipant { get; set; }

    public Guid? NextMatchId { get; private set; }
    public CompetitionBracketMatch? NextMatch { get; private set; }

    /// <summary>
    /// 32, 16, 8, 4, 2, 1
    /// </summary>
    public RoundType Round { get; private set; }

    /// <summary>
    /// show match position in front
    /// </summary>
    public int MatchNumberPosition { get; private set; }

    public void SetNextMatch(CompetitionBracketMatch match)
    {
        NextMatchId = match.Id;
        NextMatch = match;
    }

    public void SetFirstParticipant(Participant? p1)
    {
        FirstParticipant = p1;
        FirstParticipantId = p1.Id;
    }
    
    public void SetFirstParticipantsBye()
    {
        FirstParticipant = null;
        FirstParticipantId = null;
        IsFirstParticipantBye = true;
    }
    
    public void SetSecondParticipants(Participant? p2)
    {
        SecondParticipant = p2;
        SecondParticipantId = p2.Id;
        IsSecondParticipantBye = false;
    }

    public void SetSecondParticipantsBye()
    {
        SecondParticipant = null;
        SecondParticipantId = null;
        IsSecondParticipantBye = true;
    }


    public static CompetitionBracketMatch Create(
        Guid id,
        CompetitionBracket competitionBracket,
        RoundType round,
        int matchNumberPosition)
    {
        var model = new CompetitionBracketMatch(
            competitionBracket.Id,
            competitionBracket.KeyParams,
            round,
            matchNumberPosition)
        {
            Id = id,
            CompetitionBracket = competitionBracket,
        };

        //TODO: validate competitionBracket should have key, round and position number should be valid  

        return model;
    }
}