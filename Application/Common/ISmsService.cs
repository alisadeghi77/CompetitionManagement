namespace Application.Services;

public interface ISmsService
{
    Task SendOtp(string phoneNumber, string tokenOtpCode);
    Task SendCoachRegister(string phoneNumber);
}