using Application.Common;
using Domain.Constant;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Participants.RegisterParticipant;

public record RegisterParticipantCommand(
    string ParticipantUserId,
    string? CoachId,
    string? CoachPhoneNumber,
    long CompetitionId,
    List<(string Key, string Value)> Params) : IRequest<long>;

public class RegisterParticipantCommandHandler(
    ISmsService smsService,
    IApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager) :
    IRequestHandler<RegisterParticipantCommand, long>
{
    public async Task<long> Handle(RegisterParticipantCommand request, CancellationToken cancellationToken)
    {
        var participantUser = await userManager.FindByIdAsync(request.ParticipantUserId);
        if (participantUser is null)
            throw new UnprocessableEntityException("کاربر یافت نشد");

        var coachUser = await GetOrCreateCoachUser(request.CoachId, request.CoachPhoneNumber);

        var competition = await dbContext.Competitions
            .FirstOrDefaultAsync(w => w.Id == request.CompetitionId, cancellationToken);

        if (competition is null)
            throw new UnprocessableEntityException("مسابقه یافت نشد");

        if (competition.Status != CompetitionStatus.PendToStart || !competition.CanRegister || !competition.IsVisible)
            throw new UnprocessableEntityException("زمان ثبت نام مسابقه به پایان رسیده است.");

        var participant = Participant.Create(
            competition,
            participantUser,
            coachUser,
            request.CoachPhoneNumber,
            request.Params.Select(s => new ParticipantParam
            {
                Key = s.Key,
                Value = s.Value
            }));

        dbContext.Participants.Add(participant);

        await dbContext.SaveChangesAsync(cancellationToken);

        return participant.Id;
    }

    private async Task<ApplicationUser> GetOrCreateCoachUser(string? coachId, string? coachPhoneNumber)
    {
        if (!string.IsNullOrEmpty(coachId))
        {
            var coachUser = await userManager.FindByIdAsync(coachId);
            if (coachUser is not null)
                return coachUser;
        }

        if (!string.IsNullOrEmpty(coachPhoneNumber))
        {
            var coachUser = await userManager.FindByNameAsync(coachPhoneNumber);
            if (coachUser is not null)
                return coachUser;

            coachUser = ApplicationUser.Create(coachPhoneNumber, "مربی", "مربی");

            var createCoachResult = await userManager.CreateAsync(coachUser);
            if (!createCoachResult.Succeeded)
            {
                var exception = new UnprocessableEntityException("اطلاعات مربی ثبت نشده است.");
                exception.Data.Add("Error", createCoachResult.Errors);
                throw exception;
            }

            var assignCoachRoleResult = await userManager.AddToRoleAsync(coachUser, RoleConstant.Coach);
            if (!assignCoachRoleResult.Succeeded)
            {
                var exception = new UnprocessableEntityException("اطلاعات مربی ثبت نشده است.");
                exception.Data.Add("Error", createCoachResult.Errors);
                throw exception;
            }

            await smsService.SendCoachRegister(coachUser.PhoneNumber!);

            return coachUser;
        }

        throw new Exception("مربی انتخاب نشده است.");
    }
}