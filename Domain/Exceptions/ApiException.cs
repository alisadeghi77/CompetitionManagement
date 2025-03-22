using System.Net;

namespace Domain.Exceptions;

public class ApiException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string ErrorCode { get; }

    protected ApiException(string message, HttpStatusCode statusCode, string errorCode = null) 
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode ?? statusCode.ToString();
    }
} 