using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

public class Bracket : BaseAuditableEntity
{
    private List<Match> _matches = new();

    public string KeyParams { get; private set; }

    [Column("Params", TypeName = "jsonb")]
    public List<ParticipantParam> RegisterParams { get; private set; } = new();

    public BracketsType Type { get; private set; }

    public long CompetitionId { get; private set; }
    public Competition Competition { get; private set; }

    public IReadOnlyCollection<Match> Matches => _matches;


    public long? GoldMedalistParticipantId { get; private set; }
    public Participant? GoldMedalistParticipant { get; private set; }


    public long? SilverMedalistParticipantId { get; private set; }
    public Participant? SilverMedalistParticipant { get; private set; }


    public long? BronzeMedalistParticipantId { get; private set; }
    public Participant? BronzeMedalistParticipant { get; private set; }


    public long? JoinBronzeMedalistParticipantId { get; private set; }
    public Participant? JoinBronzeMedalistParticipant { get; private set; }


    private Bracket(
        long competitionId,
        List<ParticipantParam> registerParams,
        string keyParams,
        BracketsType type)
    {
        CompetitionId = competitionId;
        RegisterParams = registerParams;
        KeyParams = keyParams;
        Type = type;
    }

    public static Bracket Create(Competition competition, List<ParticipantParam> registerParams, BracketsType type)
    {
        var model = new Bracket(competition.Id, registerParams, GenerateKey(registerParams), type)
        {
            Competition = competition
        };

        //TODO: validate model (params, competition status, key,....)

        return model;
    }

    public static string GenerateKey(List<ParticipantParam> param)
        => string.Join('_', param.Select(s => $@"{s.Key}.{s.Value}"));

    public void SetGoldMedalist(Participant participant)
    {
        GoldMedalistParticipant =
            participant ?? throw new UnprocessableEntityException("شرکت کننده انتخاب نشده است.");
        GoldMedalistParticipantId = participant.Id;
    }

    public void SetSilverMedalist(Participant participant)
    {
        SilverMedalistParticipant =
            participant ?? throw new UnprocessableEntityException("شرکت کننده انتخاب نشده است.");
        SilverMedalistParticipantId = participant.Id;
    }

    public void SetBronzeMedalist(Participant participant)
    {
        BronzeMedalistParticipant =
            participant ?? throw new UnprocessableEntityException("شرکت کننده انتخاب نشده است.");
        BronzeMedalistParticipantId = participant.Id;
    }

    public void SetJoinBronzeMedalist(Participant participant)
    {
        JoinBronzeMedalistParticipant =
            participant ?? throw new UnprocessableEntityException("شرکت کننده انتخاب نشده است.");
        JoinBronzeMedalistParticipantId = participant.Id;
    }
}