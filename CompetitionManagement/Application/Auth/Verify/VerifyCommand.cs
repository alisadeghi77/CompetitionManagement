using System.Security.Claims;
using CompetitionManagement.Application.Auth.Login;
using CompetitionManagement.Application.Services;
using CompetitionManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using CompetitionManagement.Domain.Exceptions;

namespace CompetitionManagement.Application.Auth.VerifyCommand;

public record VerifyCommand(string PhoneNumber, string OtpCode) : IRequest<LoginDto>;

public class VerifyCommandHandler(
    IMemoryCache cache,
    ITokenService tokenService,
    UserManager<ApplicationUser> userManager)
    : IRequestHandler<VerifyCommand, LoginDto>
{
    public async Task<LoginDto> Handle(VerifyCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = $"OTP_{request.PhoneNumber}";

        if (!cache.TryGetValue(cacheKey, out string? cachedOtp))
            throw new UnprocessableEntityException("کد ارسال شده برای شما منقضی شده است، دوباره تلاش کنید");

        if (cachedOtp != request.OtpCode)
            throw new UnprocessableEntityException("کد صحیح نمی باشد.");

        var user = await userManager.FindByNameAsync(request.PhoneNumber);
        if (user == null)
            throw new UnprocessableEntityException("کابر یافت نشد.");

        cache.Remove(cacheKey);

        if (!user.TwoFactorEnabled)
        {
            var enableResult = await userManager.SetTwoFactorEnabledAsync(user, true);
            if (enableResult.Errors.Any())
            {
                var exception = new Exception("فعال سازی کاربر با مشکل مواجه شده است.");
                exception.Data.Add("CreateUserIdentityResult", enableResult);
                throw exception;
            }
        }

        var userRoles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim> { new(ClaimTypes.Name, user.UserName!), new(ClaimTypes.NameIdentifier, user.Id) };
        
        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = tokenService.GenerateToken(claims);

        return new LoginDto(token, user.UserName!, user.FullName);
    }
}