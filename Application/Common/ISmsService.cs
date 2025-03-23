namespace Application.Common;

public interface ISmsService
{
    Task SendOtp(string phoneNumber, string tokenOtpCode);
    Task SendCoachRegister(string phoneNumber);
}