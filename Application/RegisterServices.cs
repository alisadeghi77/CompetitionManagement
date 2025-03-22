using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class RegisterServices
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ISmsService, SmsService>();
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterServices).Assembly));
        return services;
    }
}