using CompetitionManagement.Application.Services;
using CompetitionManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace CompetitionManagement.Application.Auth.Login;

public record LoginCommand(string PhoneNumber) : IRequest;

public class LoginCommandHandler(
    IMemoryCache cache,
    ISmsService smsService,
    UserManager<ApplicationUser> userManager)
    : IRequestHandler<LoginCommand>
{
    public async Task Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.PhoneNumber);
        if (user is null)
            throw new Exception("کاربر با این شماره موبایل یافت نشد");

        var otp = new Random().Next(1000, 9999).ToString();

        var cacheKey = $"OTP_{request.PhoneNumber}";

        await smsService.SendOtp(user.UserName!, otp);

        cache.Set(cacheKey, otp, TimeSpan.FromMinutes(3));
    }
}