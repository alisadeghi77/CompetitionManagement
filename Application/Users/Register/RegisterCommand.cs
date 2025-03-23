using Application.Common;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Users.Register;

public record RegisterCommand(string PhoneNumber, string FirstName, string LastName, string Role) : IRequest;

public class RegisterCommandHandler(
    IMemoryCache cache,
    ISmsService smsService,
    UserManager<ApplicationUser> userManager)
    : IRequestHandler<RegisterCommand>
{
    public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.PhoneNumber);
        if (user is not null)
        {
            await AssignRole(request.Role, user);

            await SendOtp(request.PhoneNumber, user);

            return;
        }


        user = ApplicationUser.Create(request.PhoneNumber, request.FirstName, request.LastName);
        user.TwoFactorEnabled = false;

        var createResult = await userManager.CreateAsync(user);
        if (createResult.Errors.Any())
        {
            var exception = new Exception("ثبت کاربر با مشکل مواجه شده است.");
            exception.Data.Add("CreateUserIdentityResult", createResult);
            throw exception;
        }

        await AssignRole(request.Role, user);

        await SendOtp(request.PhoneNumber, user);
    }

    private async Task AssignRole(string role, ApplicationUser user)
    {
        var roles = await userManager.GetRolesAsync(user);
        if (roles.All(c => c.ToUpper() != role.ToUpper()))
        {
            var newRoleResult = await userManager.AddToRoleAsync(user, role);
            if (!newRoleResult.Succeeded)
            {
                var exception = new Exception("ثبت کاربر با مشکل مواجه شده است.");
                exception.Data.Add("AsinineNewRoleIdentityResult", newRoleResult);
                throw exception;
            }
        }
    }

    private async Task SendOtp(string phoneNumber, ApplicationUser user)
    {
        var otp = new Random().Next(1000, 9999).ToString();
        var cacheKey = $"OTP_{phoneNumber}";

        await smsService.SendOtp(user.UserName!, otp);

        cache.Set(cacheKey, otp, TimeSpan.FromMinutes(3));
    }
}