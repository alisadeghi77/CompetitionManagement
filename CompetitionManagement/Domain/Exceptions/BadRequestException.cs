using System.Net;

namespace CompetitionManagement.Domain.Exceptions;

public class BadRequestException : ApiException
{
    public BadRequestException(string message) 
        : base(message, HttpStatusCode.BadRequest, "BAD_REQUEST")
    {
    }
} 