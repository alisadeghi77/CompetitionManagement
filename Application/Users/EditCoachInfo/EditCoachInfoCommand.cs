using Application.Common;
using Domain.Constant;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.EditCoachInfo;

public record EditCoachInfoCommand(
    string CoachUserId,
    string FirstName,
    string LastName,
    DateTime BirthDate) : IRequest;

public class EditCoachInfoCommandHandler(
    IApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager) :
    IRequestHandler<EditCoachInfoCommand>
{
    public async Task Handle(EditCoachInfoCommand request, CancellationToken cancellationToken)
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