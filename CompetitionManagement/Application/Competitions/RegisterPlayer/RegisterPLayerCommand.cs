using CompetitionManagement.Domain.Constant;
using CompetitionManagement.Domain.Entities;
using CompetitionManagement.Domain.Enums;
using CompetitionManagement.Domain.Exceptions;
using CompetitionManagement.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CompetitionManagement.Application.Competitions.RegisterPlayer;

public record RegisterPLayerCommand(
    string PlayerUserId,
    string? CoachId,
    string? CoachPhoneNumber,
    long CompetitionId,
    List<(string Key, string Value)> Params) : IRequest<long>;

public class RegisterPLayerCommandHandler(
    ApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager) :
    IRequestHandler<RegisterPLayerCommand, long>
{
    public async Task<long> Handle(RegisterPLayerCommand request, CancellationToken cancellationToken)
    {
        var playerUser = await userManager.FindByIdAsync(request.PlayerUserId);
        if (playerUser is null)
            throw new UnprocessableEntityException("کاربر یافت نشد");

        var coachUser = await GetOrCreateCoachId(request.CoachId, request.CoachPhoneNumber);

        var competition = await dbContext.Competitions
            .FirstOrDefaultAsync(w => w.Id == request.CompetitionId, cancellationToken);

        if (competition is null)
            throw new UnprocessableEntityException("کاربر یافت نشد");

        if (competition.Status != CompetitionStatus.PendToStart)
            throw new UnprocessableEntityException("زمان ثبت نام مسابقه به پایان رسیده است.");

        var competitionRegister = CompetitionRegister.Create(
            competition,
            playerUser,
            coachUser,
            request.CoachPhoneNumber,
            request.Params.Select(s => new CompetitionRegisterParam
            {
                Key = s.Key,
                Value = s.Value
            }));
        
        dbContext.CompetitionRegisters.Add(competitionRegister);

        await dbContext.SaveChangesAsync(cancellationToken);
        
        return competitionRegister.Id;
    }

    private async Task<ApplicationUser> GetOrCreateCoachId(string? coachId, string? coachPhoneNumber)
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

            return coachUser;
        }

        throw new Exception("مربی انتخاب نشده است.");
    }
}