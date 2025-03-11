namespace CompetitionManagement.Application.Services;

public interface ISmsService
{
    Task SendOtp(string phoneNumber, string tokenOtpCode);
}

public class SmsService : ISmsService
{
    public Task SendOtp(string phoneNumber, string tokenOtpCode)
    {
        //TODO: send otp code via sms
        return Task.CompletedTask;
    }
}