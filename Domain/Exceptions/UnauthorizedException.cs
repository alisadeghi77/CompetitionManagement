using System.Net;

namespace Domain.Exceptions;

public class UnauthorizedException : ApiException
{
    public UnauthorizedException(string message = "You are not authorized to access this resource") 
        : base(message, HttpStatusCode.Unauthorized, "UNAUTHORIZED")
    {
    }
} 