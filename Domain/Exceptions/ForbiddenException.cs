using System.Net;

namespace Domain.Exceptions;

public class ForbiddenException : ApiException
{
    public ForbiddenException(string message = "You don't have permission to access this resource") 
        : base(message, HttpStatusCode.Forbidden, "FORBIDDEN")
    {
    }
} 