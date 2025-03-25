using Application.Common;

namespace Infrastructure.Services;

public class SmsService : ISmsService
{
    public Task SendOtp(string phoneNumber, string tokenOtpCode)
    {
        //TODO: send otp code via sms
        return Task.CompletedTask;
    }

    public Task SendCoachRegister(string phoneNumber)
    {
        //TODO: send otp code via sms
        return Task.CompletedTask;
    }
}