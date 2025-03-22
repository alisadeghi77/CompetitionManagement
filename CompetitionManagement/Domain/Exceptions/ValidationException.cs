using System.Net;

namespace CompetitionManagement.Domain.Exceptions;

public class ValidationException : ApiException
{
    public ValidationException(string message) 
        : base(message, HttpStatusCode.UnprocessableEntity, "VALIDATION_ERROR")
    {
    }
    
    public ValidationException(string field, string message) 
        : base($"Validation failed for {field}: {message}", HttpStatusCode.UnprocessableEntity, "VALIDATION_ERROR")
    {
    }
} 