using Application.Common;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CompetitionBrackets.DeleteBrackets;

public record DeleteBracketsCommand(long CompetitionId) : IRequest;

public class DeleteBracketsCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<DeleteBracketsCommand>
{
    public async Task Handle(DeleteBracketsCommand request, CancellationToken cancellationToken)
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
            throw new UnprocessableEntityException("مسابقه مورد نظر امکان حذف جدول بندی ندارد.");

        if (!competition.Brackets.Any())
            throw new UnprocessableEntityException("مسابقه مورد نظر جدول بندی نشده است.");

        dbContext.RemoveRange(competition.Brackets.SelectMany(b => b.Matches));
        dbContext.RemoveRange(competition.Brackets);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}