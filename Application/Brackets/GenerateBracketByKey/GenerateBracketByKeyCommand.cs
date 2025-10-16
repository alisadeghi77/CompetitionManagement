using Application.Brackets.BracketsBuilder;
using Application.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Brackets.GenerateBracketByKey;

public record GenerateBracketByKeyCommand(long CompetitionId, string BracketKey) : IRequest;

public class GenerateBracketByKeyCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GenerateBracketByKeyCommand>
{
    public async Task Handle(
        GenerateBracketByKeyCommand request,
        CancellationToken cancellationToken)
    {
        var competition = await dbContext.Competitions
            .Include(c => c.Brackets.Where(w => w.KeyParams.Equals(request.BracketKey)))
            .ThenInclude(c => c.Matches)
            .Include(c => c.Participants)
            .ThenInclude(c => c.CoachUser)
            .Include(c => c.Participants)
            .ThenInclude(c => c.ParticipantUser)
            .FirstOrDefaultAsync(w => w.Id == request.CompetitionId, cancellationToken);

        if (competition is null)
            throw new UnprocessableEntityException("مسابقه مورد نظر یافت نشد.");

        if (competition.Status != CompetitionStatus.PendToStart)
            throw new UnprocessableEntityException("مسابقه مورد نظر امکان جدول بندی ندارد.");

        if (competition.Brackets.Any())
            throw new UnprocessableEntityException("برای جدول مورد نظر مسابقه ثبت شده است.");


        var combinationParam = competition.GetParamsWithKeys().FirstOrDefault(w => w.Key == request.BracketKey);
        if (combinationParam is null)
            throw new UnprocessableEntityException("جدول مورد نظر امکان ساخته شدن ندارد.");

        var participants = competition.Participants.Where(w => w.HasSameParamsWith(combinationParam.ParamsList)).ToList();
        if (!participants.Any())
            throw new UnprocessableEntityException("جدول مورد نظر شرکت کننده تایید شده ندارد.");

        var bracket = Bracket.Create(competition, combinationParam.ParamsList, BracketsType.SingleElimination);
        var bracketMatches = BracketMatchBuilder.BuilderDirector(bracket, participants);
        
        dbContext.Brackets.Add(bracket);
        dbContext.Matches.AddRange(bracketMatches);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    static List<List<ParticipantParam>> GenerateCombinations(CompetitionParam param,
        List<ParticipantParam>? currentPath = null)
    {
        var results = new List<List<ParticipantParam>>();
        currentPath ??= [];

        if (param.Values == null || param.Values.Count == 0)
        {
            results.Add([..currentPath]);
            return results;
        }

        foreach (var value in param.Values)
        {
            var newPath = new List<ParticipantParam>(currentPath)
                { new() { Key = param.Key, Value = value.Key } };

            if (value.Params == null || value.Params.Count == 0)
                results.Add(newPath);
            else
                foreach (var nestedParam in value.Params)
                    results.AddRange(GenerateCombinations(nestedParam, newPath));
        }

        return results;
    }
}