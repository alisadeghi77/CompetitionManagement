using Application.Common;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Participants.RegisterChangeStatus;

public record ParticipantRegisterChangeStatusCommand(long ParticipantId, RegisterStatus Status) : IRequest;

public class ParticipantRegisterChangeStatusCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<ParticipantRegisterChangeStatusCommand>
{
    public async Task Handle(ParticipantRegisterChangeStatusCommand request, CancellationToken cancellationToken)
    {
        var participant = await dbContext.Participants
            .FirstOrDefaultAsync(w => w.Id == request.ParticipantId, cancellationToken);

        if (participant is null)
            throw new UnprocessableEntityException("شرکت کننده یافت نشد.");

        participant.ChangeStatus(request.Status);

        dbContext.Participants.Update(participant);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}