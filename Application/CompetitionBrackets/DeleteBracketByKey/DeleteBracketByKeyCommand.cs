using Application.Common;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CompetitionBrackets.DeleteBracketByKey;

public record DeleteBracketByKeyCommand(long CompetitionId, string BracketKey) : IRequest;

public class DeleteBracketByKeyCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<DeleteBracketByKeyCommand>
{
    public async Task Handle(DeleteBracketByKeyCommand request, CancellationToken cancellationToken)
    {
        var competitionBracket = await dbContext.CompetitionBrackets
            .Include(c => c.Matches)
            .FirstOrDefaultAsync(w =>
                w.CompetitionId == request.CompetitionId && w.KeyParams.Equals(request.BracketKey), cancellationToken);

        if (competitionBracket is null)
            throw new UnprocessableEntityException("جدول مورد نظر یافت نشد.");

        dbContext.RemoveRange(competitionBracket.Matches);
        dbContext.Remove(competitionBracket);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}