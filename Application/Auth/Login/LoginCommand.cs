using Application.Common;
using Application.Services;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;

namespace Application.Auth.Login;

public record LoginCommand(string PhoneNumber) : IRequest;

public class LoginCommandHandler(
    IHostEnvironment hostEnvironment,
    IMemoryCache cache,
    ISmsService smsService,
    UserManager<ApplicationUser> userManager)
    : IRequestHandler<LoginCommand>
{
    public async Task Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.PhoneNumber);
        if (user is null)
            throw new UnprocessableEntityException("کاربر با این شماره موبایل یافت نشد");

        var otp = hostEnvironment.IsDevelopment() ? "1111" : new Random().Next(1000, 9999).ToString();

        var cacheKey = $"OTP_{request.PhoneNumber}";

        await smsService.SendOtp(user.UserName!, otp);

        cache.Set(cacheKey, otp, TimeSpan.FromMinutes(3));
    }
}