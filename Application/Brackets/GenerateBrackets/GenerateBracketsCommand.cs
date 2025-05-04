using Application.Brackets.BracketsBuilder;
using Application.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Brackets.GenerateBrackets;

public record GenerateBracketsCommand(long CompetitionId) : IRequest;

public class GenerateBracketsCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GenerateBracketsCommand>
{
    public async Task Handle(
        GenerateBracketsCommand request,
        CancellationToken cancellationToken)
    {
        var competition = await dbContext.Competitions
            .Include(c => c.Brackets)
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
            throw new UnprocessableEntityException("مسابقه مورد نظر جدول بندی شده است.");

        if (competition.Participants.All(w => w.Status != RegisterStatus.Approved))
            throw new UnprocessableEntityException("مسابقه مورد نظر شرکت کننده تایید شده ندارد.");

        var combinationParams = GenerateAllCombinations(competition.RegisterParams);
        foreach (var param in combinationParams)
        {
            var participants = competition.Participants.Where(w => w.HasSameParamsWith(param)).ToList();
            if (!participants.Any())
                continue;

            var bracket = Bracket.Create(competition, param, BracketsType.SingleElimination);
            var bracketMatches = BracketMatchBuilder.BuilderDirector(bracket, participants);
            
            dbContext.Brackets.Add(bracket);
            dbContext.Matches.AddRange(bracketMatches);
        }


        await dbContext.SaveChangesAsync(cancellationToken);
    }

    static List<List<ParticipantParam>> GenerateAllCombinations(CompetitionParam param,
        List<ParticipantParam>? currentPath = null)
    {
        var results = new List<List<ParticipantParam>>();
        currentPath ??= new List<ParticipantParam>();

        if (param.Values == null || !param.Values.Any())
            return results;

        foreach (var value in param.Values)
        {
            var path = new List<ParticipantParam>(currentPath)
            {
                new ParticipantParam { Key = param.Key, Value = value.Key }
            };

            if (value.Params == null || !value.Params.Any())
            {
                results.Add(path);
                continue;
            }

            var paramResults = new List<List<ParticipantParam>> { path };
            foreach (var nestedParam in value.Params)
            {
                var newResults = new List<List<ParticipantParam>>();
                foreach (var currentResult in paramResults)
                {
                    var nestedCombinations = GenerateAllCombinations(nestedParam, currentResult);
                    if (nestedCombinations.Any())
                        newResults.AddRange(nestedCombinations);
                    else
                        newResults.Add(currentResult);
                }
                paramResults = newResults;
            }
            results.AddRange(paramResults);
        }

        return results;
    }
}