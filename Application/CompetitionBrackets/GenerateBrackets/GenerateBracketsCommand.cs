using Application.Common;
using Application.CompetitionBrackets.BracketsBuilder;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CompetitionBrackets.GenerateBrackets;

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

        var combinationParams = GenerateCombinations(competition.RegisterParams);
        foreach (var param in combinationParams)
        {
            var participants = competition.Participants.Where(w => w.HasSameParamsWith(param)).ToList();
            if (!participants.Any())
                continue;

            var competitionBrackets = CompetitionBracket.Create(competition, param, BracketsType.SingleElimination);
            var bracketMatches = CompetitionBracketMatchBuilder.BuilderDirector(competitionBrackets, participants);
            dbContext.CompetitionBracketMatches.AddRange(bracketMatches);
        }


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