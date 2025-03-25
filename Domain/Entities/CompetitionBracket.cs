using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities;

public class CompetitionBracket : BaseAuditableEntity
{
    private List<CompetitionBracketMatch> _matches = new();

    public string KeyParams { get; private set; }

    [Column("Params", TypeName = "jsonb")]
    public List<ParticipantParam> RegisterParams { get; private set; } = new();

    public BracketsType Type { get; private set; }

    public long CompetitionId { get; private set; }

    public Competition Competition { get; private set; }

    public IReadOnlyCollection<CompetitionBracketMatch> Matches => _matches;


    private CompetitionBracket(
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

    public static CompetitionBracket Create(Competition competition, List<ParticipantParam> registerParams, BracketsType type)
    {
        var model = new CompetitionBracket(competition.Id, registerParams, GenerateKey(registerParams), type)
        {
            
            Competition = competition
        };
        
        //TODO: validate model (params, competition status, key,....)

        return model;
    }

    private static string GenerateKey(List<ParticipantParam> param) 
        => string.Join('_', param.Select(s => $@"{s.Key}.{s.Value}"));
}