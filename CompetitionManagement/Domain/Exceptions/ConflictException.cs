using System.Net;

namespace CompetitionManagement.Domain.Exceptions;

public class ConflictException : ApiException
{
    public ConflictException(string message) 
        : base(message, HttpStatusCode.Conflict, "CONFLICT")
    {
    }
} 