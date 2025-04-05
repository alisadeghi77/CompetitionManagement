using Domain.Common;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Validations;
using FluentValidation;

namespace Domain.Entities;

public class Match : BaseEntity<Guid>
{
    private List<Match> _parentMatches = new();

    private Match(
        long bracketId,
        string keyParams,
        RoundType round,
        int matchNumberPosition)
    {
        BracketId = bracketId;
        KeyParams = keyParams;
        Round = round;
        MatchNumberPosition = matchNumberPosition;
    }

    public long BracketId { get; private set; }
    public Bracket Bracket { get; private set; }

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
    public Match? NextMatch { get; private set; }

    public IReadOnlyCollection<Match> ParentMatches => _parentMatches;

    /// <summary>
    /// 32, 16, 8, 4, 2, 1
    /// </summary>
    public RoundType Round { get; private set; }

    /// <summary>
    /// show match position in front
    /// </summary>
    public int MatchNumberPosition { get; private set; }

    public void SetNextMatch(Match match)
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


    public static Match Create(
        Guid id,
        Bracket bracket,
        RoundType round,
        int matchNumberPosition)
    {
        var model = new Match(
            bracket.Id,
            bracket.KeyParams,
            round,
            matchNumberPosition)
        {
            Id = id,
            Bracket = bracket,
        };

        new MatchValidator().ValidateAndThrow(model);

        return model;
    }

    public void SetFirstParticipantWinner()
    {
        if (FirstParticipantId is null || IsFirstParticipantBye)
            throw new UnprocessableEntityException("شرکت کننده اول نمی تواند به عنوان برنده انتخاب شود.");

        WinnerParticipant = FirstParticipant;
        WinnerParticipantId = FirstParticipantId;

        if (IsFinalMatch())
        {
            Bracket.SetGoldMedalist(FirstParticipant);
            Bracket.SetSilverMedalist(SecondParticipant);

            var bronzeParentMatch = ParentMatches.FirstOrDefault(w => w.WinnerParticipantId == FirstParticipantId);
            if (bronzeParentMatch is not null)
            {
                if (bronzeParentMatch.FirstParticipantId == FirstParticipantId && !IsSecondParticipantBye)
                    Bracket.SetBronzeMedalist(bronzeParentMatch.SecondParticipant);

                if (bronzeParentMatch.SecondParticipantId == FirstParticipantId && !IsFirstParticipantBye)
                    Bracket.SetBronzeMedalist(bronzeParentMatch.FirstParticipant);
            }

            var jointBronzeParentMatch =
                ParentMatches.FirstOrDefault(w => w.WinnerParticipantId == SecondParticipantId);
            if (jointBronzeParentMatch is not null)
            {
                if (jointBronzeParentMatch.FirstParticipantId == SecondParticipantId && !IsSecondParticipantBye)
                    Bracket.SetJoinBronzeMedalist(jointBronzeParentMatch.SecondParticipant);

                if (jointBronzeParentMatch.SecondParticipantId == SecondParticipantId && !IsFirstParticipantBye)
                    Bracket.SetJoinBronzeMedalist(jointBronzeParentMatch.FirstParticipant);
            }


            return;
        }

        if (MatchNumberPosition % 2 == 1)
            NextMatch.SetFirstParticipant(FirstParticipant);
        else
            NextMatch.SetSecondParticipants(FirstParticipant);
    }


    public void SetSecondParticipantWinner()
    {
        if (SecondParticipantId is null || IsSecondParticipantBye)
            throw new UnprocessableEntityException("شرکت کننده دوم نمی تواند به عنوان برنده انتخاب شود.");

        WinnerParticipant = SecondParticipant;
        WinnerParticipantId = SecondParticipantId;

        if (IsFinalMatch())
        {
            Bracket.SetGoldMedalist(FirstParticipant);
            Bracket.SetSilverMedalist(SecondParticipant);

            var bronzeParentMatch = ParentMatches.FirstOrDefault(w => w.WinnerParticipantId == SecondParticipantId);
            if (bronzeParentMatch is not null)
            {
                if (bronzeParentMatch.FirstParticipantId == SecondParticipantId && !IsSecondParticipantBye)
                    Bracket.SetBronzeMedalist(bronzeParentMatch.SecondParticipant);

                if (bronzeParentMatch.SecondParticipantId == SecondParticipantId && !IsFirstParticipantBye)
                    Bracket.SetBronzeMedalist(bronzeParentMatch.FirstParticipant);
            }

            var jointBronzeParentMatch =
                ParentMatches.FirstOrDefault(w => w.WinnerParticipantId == SecondParticipantId);
            if (jointBronzeParentMatch is not null)
            {
                if (jointBronzeParentMatch.FirstParticipantId == SecondParticipantId && !IsSecondParticipantBye)
                    Bracket.SetJoinBronzeMedalist(jointBronzeParentMatch.SecondParticipant);

                if (jointBronzeParentMatch.SecondParticipantId == SecondParticipantId && !IsFirstParticipantBye)
                    Bracket.SetJoinBronzeMedalist(jointBronzeParentMatch.FirstParticipant);
            }


            return;
        }


        if (MatchNumberPosition % 2 == 1)
            NextMatch.SetFirstParticipant(SecondParticipant);
        else
            NextMatch.SetSecondParticipants(SecondParticipant);
    }

    public bool IsFinalMatch() => MatchNumberPosition == 1 && NextMatchId is null;
}