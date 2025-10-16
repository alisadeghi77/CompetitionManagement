using Application.Common;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Participants.DeleteParticipantByAdmin;

public record DeleteParticipantByAdminCommand(long ParticipantId) : IRequest<long>;

public class DeleteParticipantByAdminCommandHandler(IApplicationDbContext dbContext) :
    IRequestHandler<DeleteParticipantByAdminCommand, long>
{
    public async Task<long> Handle(DeleteParticipantByAdminCommand request, CancellationToken cancellationToken)
    {
        var participant = await dbContext.Participants
            .FirstOrDefaultAsync(f => f.Id == request.ParticipantId, cancellationToken);

        if (participant is null)
            throw new UnprocessableEntityException("شرکت کننده یافت نشد.");

        
        var hasAnyMatch = await dbContext.Matches
            .AnyAsync(w => w.FirstParticipantId == request.ParticipantId || w.SecondParticipantId == request.ParticipantId, cancellationToken);

        if (hasAnyMatch)
            throw new UnprocessableEntityException("شرکت کننده در مسابقات شرکت کرده است. امکان حذف نیست.");

        
        dbContext.Participants.Remove(participant);

        await dbContext.SaveChangesAsync(cancellationToken);

        return participant.Id;
    }
}