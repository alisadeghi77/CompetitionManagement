    using Application.Common;
using Domain.Constant;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Participants.RegisterParticipantByAdmin;

public record RegisterParticipantByAdminCommand(
    string? ParticipantId,
    string? PhoneNumber,
    string? FirstName,
    string? LastName,
    string? CoachId,
    string? CoachPhoneNumber,
    long CompetitionId,
    List<ParticipantParam> Params) : IRequest<long>;

public class RegisterParticipantByAdminCommandHandler(
    ISmsService smsService,
    IApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager) :
    IRequestHandler<RegisterParticipantByAdminCommand, long>
{
    public async Task<long> Handle(RegisterParticipantByAdminCommand request, CancellationToken cancellationToken)
    {
        if (request.ParticipantId is null && string.IsNullOrEmpty(request.PhoneNumber) && string.IsNullOrEmpty(request.FirstName) && string.IsNullOrEmpty(request.LastName))
            throw new BadRequestException("اطلاعات شرکت کننده ناقص است.");

        if (request.CoachId is null && string.IsNullOrEmpty(request.CoachPhoneNumber))
            throw new BadRequestException("اطلاعات مربی ناقص است.");

        var competition = await dbContext.Competitions.FirstOrDefaultAsync(w => w.Id == request.CompetitionId, cancellationToken);
        if (competition is null)
            throw new UnprocessableEntityException("مسابقه یافت نشد");

        var participantUser = await GetParticipantUser(request.ParticipantId, request.FirstName, request.LastName, request.PhoneNumber);

        var coachUser = await GetOrCreateCoachUser(request.CoachId, request.CoachPhoneNumber);


        if (competition.Status != CompetitionStatus.PendToStart || !competition.CanRegister || !competition.IsVisible)
            throw new UnprocessableEntityException("زمان ثبت نام مسابقه به پایان رسیده است.");

        var isUserAlreadyRegister = await dbContext.Participants
            .AnyAsync(w => w.ParticipantUserId == participantUser.Id &&
                           w.CompetitionId == competition.Id, cancellationToken);

        if (isUserAlreadyRegister)
            throw new UnprocessableEntityException("شما قبلا ثبت نام کردید.");

        var participant = Participant.Create(
            competition,
            participantUser,
            coachUser,
            request.Params.Select(s => new ParticipantParam
            {
                Key = s.Key,
                Value = s.Value
            })
            .ToList());

        dbContext.Participants.Add(participant);

        await dbContext.SaveChangesAsync(cancellationToken);

        return participant.Id;
    }

    private async Task<ApplicationUser> GetParticipantUser(string? participantUserId, string? firstName, string? lastName, string? phoneNumber)
    {
        if (!string.IsNullOrEmpty(participantUserId))
        {
            var participantUser = await userManager.FindByIdAsync(participantUserId);
            if (participantUser is not null)
                return participantUser;
        }

        if (!string.IsNullOrEmpty(phoneNumber))
        {
            var participantUser = await userManager.FindByNameAsync(phoneNumber);
            if (participantUser is not null)
                return participantUser;

            participantUser = ApplicationUser.Create(phoneNumber, firstName!, lastName!);

            var createResult = await userManager.CreateAsync(participantUser);
            if (!createResult.Succeeded)
            {
                var exception = new UnprocessableEntityException("اطلاعات شرکت کننده ثبت نشده است.");
                exception.Data.Add("Error", createResult.Errors);
                throw exception;
            }

            var assignRoleResult = await userManager.AddToRoleAsync(participantUser, RoleConstant.Participant);
            if (!assignRoleResult.Succeeded)
            {
                var exception = new UnprocessableEntityException("اطلاعات شرکت کننده ثبت نشده است.");
                exception.Data.Add("Error", createResult.Errors);
                throw exception;
            }

            return participantUser;
        }
   
        throw new Exception("شرکت کننده انتخاب نشده است.");
   
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