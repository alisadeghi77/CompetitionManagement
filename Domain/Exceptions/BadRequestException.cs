using System.Net;

namespace Domain.Exceptions;

public class BadRequestException : ApiException
{
    public BadRequestException(string message) 
        : base(message, HttpStatusCode.BadRequest, "BAD_REQUEST")
    {
    }
} 