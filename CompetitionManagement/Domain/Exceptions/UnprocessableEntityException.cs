using System.Net;

namespace CompetitionManagement.Domain.Exceptions;

public class UnprocessableEntityException : ApiException
{
    public UnprocessableEntityException(string message) 
        : base(message, HttpStatusCode.UnprocessableEntity, "UNPROCESSABLE_ENTITY")
    {
    }
    
    public UnprocessableEntityException(string entity, string reason) 
        : base($"تغییر {entity} امکان‌پذیر نیست: {reason}", HttpStatusCode.UnprocessableEntity, "UNPROCESSABLE_ENTITY")
    {
    }
} 