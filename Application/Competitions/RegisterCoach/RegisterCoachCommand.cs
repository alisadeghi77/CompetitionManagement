using Application.Common;
using Domain.Constant;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Competitions.RegisterCoach;

public record RegisterCoachCommand(
    string CoachUserId,
    string FirstName,
    string LastName,
    DateTime BirthDate) : IRequest;

public class RegisterCoachCommandHandler(
    IApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager) :
    IRequestHandler<RegisterCoachCommand>
{
    public async Task Handle(RegisterCoachCommand request, CancellationToken cancellationToken)
    {
        var coachUser = await userManager.FindByIdAsync(request.CoachUserId);
        if (coachUser is null)
            throw new UnprocessableEntityException("کاربر یافت نشد");

        var coachUserRoles = await userManager.GetRolesAsync(coachUser);
        if (coachUserRoles.All(c => c.ToUpper() != RoleConstant.Coach))
            throw new UnprocessableEntityException("کاربر به عنوان مربی ثبت نشده است.");

        coachUser.UpdateInfo(request.FirstName, request.LastName, request.BirthDate);

        await userManager.UpdateAsync(coachUser);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}