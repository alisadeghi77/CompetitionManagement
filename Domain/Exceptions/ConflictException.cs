using System.Net;

namespace Domain.Exceptions;

public class ConflictException : ApiException
{
    public ConflictException(string message) 
        : base(message, HttpStatusCode.Conflict, "CONFLICT")
    {
    }
} 