using Domain.Entities;
using Domain.Enums;

namespace Application.Brackets.BracketsBuilder;

public class BracketMatchBuilder
{
    private int _bracketSize;
    private List<Participant> _participants = new();
    private Bracket _bracket;
    private List<Match> _matches;

    public static List<Match> BuilderDirector(
        Bracket bracket,
        List<Participant> participants) =>
        new BracketMatchBuilder()
            .SetBracket(bracket)
            .SetParticipants(participants)
            .GenerateMatch()
            .AssignByes()
            .Build();

    public BracketMatchBuilder SetBracket(Bracket bracket)
    {
        _bracket = bracket;
        return this;
    }

    public BracketMatchBuilder SetParticipants(List<Participant> participants)
    {
        _participants = participants;
        _bracketSize = NextPowerOfTwo(_participants.Count);
        return this;
    }
    
    public BracketMatchBuilder GenerateMatch()
    {
        _matches = BuildMatchTree(_bracketSize);
        return this;
    }

    public BracketMatchBuilder AssignByes()
    {
        var byeCount = _bracketSize - _participants.Count;

        if (byeCount <= 0)
            return this;

        var byePositions = (_bracketSize switch
            {
                2 => [1],
                4 => [3],
                8 => [7, 0, 4],
                16 => [15, 0, 8, 7, 12, 4, 11],
                32 => [31, 0, 16, 15, 24, 8, 23, 7, 28, 3, 19, 12, 27, 11, 20],
                64 =>
                [
                    63, 0, 32, 31, 48, 15, 47, 16, 56, 7, 40, 23, 55, 24, 39, 8, 60, 3, 36, 27, 44, 19, 52, 11, 59, 4,
                    35,
                    28, 43, 20, 51
                ],
                //TODO: throw exception
                _ => Enumerable.Range(0, _bracketSize).Reverse().ToArray()
            })
            .Take(byeCount);

        foreach (var item in _matches.Select((m, i) => new { m, f = i * 2, s = i * 2 + 1 }))
        {
            if (byePositions.Any(w => w == item.f))
                _matches.First(w => w.Id == item.m.Id).SetFirstParticipantsBye();

            if (byePositions.Any(w => w == item.s))
                _matches.First(w => w.Id == item.m.Id).SetSecondParticipantsBye();
        }

        return this;
    }

    public List<Match> Build()
    {
        var modifiableParticipants = new List<Participant>(_participants);

        var teamDistribution = modifiableParticipants
            .GroupBy(f => f.CoachUserId)
            .ToDictionary(g => g.Key, g => g.Count());

        foreach (var match in _matches)
        {
            if (match.IsFirstParticipantBye)
            {
                var optimalParticipant = FindOptimalParticipant(modifiableParticipants, teamDistribution, null);
                if (optimalParticipant is null)
                {
                    var exception = new Exception("Expected participants not found");
                    exception.Data.Add("Participants", _participants);
                    exception.Data.Add("Bracket", _bracket);
                    throw exception;
                }

                match.SetSecondParticipants(optimalParticipant);
                modifiableParticipants.Remove(optimalParticipant);
                teamDistribution[optimalParticipant.CoachUserId]--;
            }
            else if (match.IsSecondParticipantBye)
            {
                var optimalParticipant = FindOptimalParticipant(modifiableParticipants, teamDistribution, null);
                if (optimalParticipant is null)
                {
                    var exception = new Exception("Expected participants not found");
                    exception.Data.Add("Participants", _participants);
                    exception.Data.Add("Bracket", _bracket);
                    throw exception;
                }

                match.SetFirstParticipant(optimalParticipant);
                modifiableParticipants.Remove(optimalParticipant);
                teamDistribution[optimalParticipant.CoachUserId]--;
            }
            else
            {
                var firstFighter = FindOptimalParticipant(modifiableParticipants, teamDistribution, null);
                var secondFighter =
                    FindOptimalParticipant(modifiableParticipants, teamDistribution, firstFighter?.CoachUserId);
                if (firstFighter is null || secondFighter is null)
                {
                    var exception = new Exception("Expected participants not found");
                    exception.Data.Add("Participants", _participants);
                    exception.Data.Add("Bracket", _bracket);
                    throw exception;
                }

                match.SetFirstParticipant(firstFighter);
                modifiableParticipants.Remove(firstFighter);
                teamDistribution[firstFighter.CoachUserId]--;

                match.SetSecondParticipants(secondFighter);
                modifiableParticipants.Remove(secondFighter);
                teamDistribution[secondFighter.CoachUserId]--;
            }
        }

        return _matches;
    }

    
    protected static Participant? FindOptimalParticipant(
        IReadOnlyCollection<Participant> participant,
        IReadOnlyDictionary<string, int> teamDistribution,
        string? excludeTeam)
    {
        if (participant.Count == 0)
            return null;

        return participant
            .Where(f => excludeTeam is null || f.CoachUserId != excludeTeam)
            .MaxBy(f => teamDistribution.GetValueOrDefault(f.CoachUserId, 0));
    }

    protected List<Match> BuildMatchTree(int bracketSize)
    {
        var lastPositionNumber = bracketSize - 1;
        var matchCount = bracketSize / 2;
        var currentRound = Enumerable
            .Range(0, matchCount)
            .Select(_ => Match.Create(
                Guid.NewGuid(),
                _bracket,
                (RoundType)bracketSize,
                lastPositionNumber--))
            .ToList();

        if (matchCount > 1)
        {
            var nextRound = BuildMatchTree(bracketSize / 2);
            for (var i = 0; i < currentRound.Count; i += 2)
            {
                var nextMatch = nextRound[i / 2];
                currentRound[i].SetNextMatch(nextMatch);
                currentRound[i + 1].SetNextMatch(nextMatch);
            }
        }

        return currentRound;
    }

    protected static int NextPowerOfTwo(int n)
    {
        if (n <= 1) return 1;
        var exponent = (int)Math.Ceiling(Math.Log(n, 2));
        return (int)Math.Pow(2, exponent);
    }
}