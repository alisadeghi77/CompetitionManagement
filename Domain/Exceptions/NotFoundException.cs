using System.Net;

namespace Domain.Exceptions;

public class NotFoundException : ApiException
{
    public NotFoundException(string message) 
        : base(message, HttpStatusCode.NotFound, "RESOURCE_NOT_FOUND")
    {
    }
    
    public NotFoundException(string entity, object key) 
        : base($"Entity {entity} with identifier {key} was not found.", HttpStatusCode.NotFound, "RESOURCE_NOT_FOUND")
    {
    }
} 