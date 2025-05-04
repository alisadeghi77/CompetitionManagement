using Application.Common;
using Application.Matches.GetMatchesByBracketId;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Brackets.GetBracketMatches;

public record GetMatchesByBracketIdQuery(string BracketKey) : IRequest<List<BracketMatchesDto>>; 

public class GetMatchesByBracketIdQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetMatchesByBracketIdQuery, List<BracketMatchesDto>>
{
    public async Task<List<BracketMatchesDto>> Handle(
        GetMatchesByBracketIdQuery request,
        CancellationToken cancellationToken)
    {
        var bracket = await dbContext.Brackets
            .Include(b => b.Matches)
            .ThenInclude(m => m.FirstParticipant)
            .ThenInclude(p => p.ParticipantUser)
            .Include(b => b.Matches)
            .ThenInclude(m => m.FirstParticipant)
            .ThenInclude(p => p.CoachUser)
            .Include(b => b.Matches)
            .ThenInclude(m => m.SecondParticipant)
            .ThenInclude(p => p.ParticipantUser)
            .Include(b => b.Matches)
            .ThenInclude(m => m.SecondParticipant)
            .ThenInclude(p => p.CoachUser)
            .Include(b => b.Matches)
            .ThenInclude(m => m.WinnerParticipant)
            .ThenInclude(p => p.ParticipantUser)
            .Include(b => b.Matches)
            .ThenInclude(m => m.WinnerParticipant)
            .ThenInclude(p => p.CoachUser)
            .FirstOrDefaultAsync(b => b.KeyParams == request.BracketKey, cancellationToken);

        if (bracket is null)
            throw new UnprocessableEntityException("جدول مورد نظر یافت نشد.");

        return bracket.Matches.Select(match => new BracketMatchesDto(
            match.Id,
            match.Round,
            match.MatchNumberPosition,
            
            // First Participant Info
            match.FirstParticipant?.Id,
            match.FirstParticipant?.ParticipantUser.FullName,
            match.FirstParticipant?.CoachUser.Id,
            match.FirstParticipant?.CoachUser.FullName,
            match.IsFirstParticipantBye,
            
            // Second Participant Info
            match.SecondParticipant?.Id,
            match.SecondParticipant?.ParticipantUser.FullName,
            match.SecondParticipant?.CoachUser.Id,
            match.SecondParticipant?.CoachUser.FullName,
            match.IsSecondParticipantBye,
            
            // Winner Participant Info
            match.WinnerParticipant?.Id,
            match.WinnerParticipant?.ParticipantUser.FullName,
            match.WinnerParticipant?.CoachUser.Id,
            match.WinnerParticipant?.CoachUser.FullName))
            .ToList();
    }
} 