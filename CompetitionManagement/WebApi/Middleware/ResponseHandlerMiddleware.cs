using System.Net;
using System.Text.Json;
using CompetitionManagement.Domain.Exceptions;

namespace CompetitionManagement.WebApi.Middleware
{
    public class ResponseHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ResponseHandlerMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ResponseHandlerMiddleware(
            RequestDelegate next,
            ILogger<ResponseHandlerMiddleware> logger,
            IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Store the original body stream
            var originalBodyStream = context.Response.Body;

            try
            {
                // Create a new memory stream to capture the response
                using var responseBody = new MemoryStream();
                context.Response.Body = responseBody;

                // Continue processing the request
                await _next(context);

                // Reset the stream position to read from the beginning
                responseBody.Seek(0, SeekOrigin.Begin);

                // Read the response body
                var responseContent = await new StreamReader(responseBody).ReadToEndAsync();
                
                // Only handle JSON responses
                if (context.Response.ContentType?.ToLower().Contains("application/json") == true)
                {
                    // Create standardized response
                    var standardResponse = new ApiResponse
                    {
                        Status = context.Response.StatusCode,
                        Data = string.IsNullOrEmpty(responseContent) ? null : JsonDocument.Parse(responseContent).RootElement
                    };

                    // Reset the stream for writing
                    responseBody.SetLength(0);

                    // Serialize and write the standardized response
                    await JsonSerializer.SerializeAsync(responseBody, standardResponse, 
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                }

                // Copy the modified response to the original stream
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطایی رخ داده است: {Message}", ex.Message);
                
                context.Response.Body = originalBodyStream;
                context.Response.ContentType = "application/json";
                
                var errorResponse = new ApiResponse
                {
                    Status = DetermineStatusCode(ex),
                    ErrorMessages = BuildErrorMessages(ex)
                };

                context.Response.StatusCode = errorResponse.Status;
                
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                await JsonSerializer.SerializeAsync(context.Response.Body, errorResponse, options);
            }
        }

        private int DetermineStatusCode(Exception exception)
        {
            return exception switch
            {
                ApiException apiEx => (int)apiEx.StatusCode,
                FluentValidation.ValidationException _ => (int)HttpStatusCode.UnprocessableEntity,
                _ => (int)HttpStatusCode.InternalServerError
            };
        }

        private List<ErrorMessage> BuildErrorMessages(Exception exception)
        {
            var errorMessages = new List<ErrorMessage>();

            switch (exception)
            {
                case ApiException apiEx:
                    errorMessages.Add(new ErrorMessage
                    {
                        Title = GetPersianErrorTitle(apiEx.ErrorCode),
                        Message = apiEx.Message
                    });
                    break;

                case FluentValidation.ValidationException validationEx:
                    foreach (var error in validationEx.Errors)
                    {
                        errorMessages.Add(new ErrorMessage
                        {
                            Title = "خطای اعتبارسنجی",
                            Message = error.ErrorMessage
                        });
                    }
                    break;

                default:
                    errorMessages.Add(new ErrorMessage
                    {
                        Title = "خطای داخلی سرور",
                        Message = _environment.IsDevelopment() 
                            ? exception.Message 
                            : "خطای غیرمنتظره‌ای رخ داده است"
                    });
                    break;
            }

            return errorMessages;
        }
        
        private string GetPersianErrorTitle(string errorCode)
        {
            return errorCode switch
            {
                "RESOURCE_NOT_FOUND" => "یافت نشد",
                "BAD_REQUEST" => "درخواست نامعتبر",
                "VALIDATION_ERROR" => "خطای اعتبارسنجی",
                "UNAUTHORIZED" => "عدم احراز هویت",
                "FORBIDDEN" => "دسترسی غیرمجاز",
                "CONFLICT" => "تداخل در داده‌ها",
                _ => "خطای سرور"
            };
        }
    }

    public class ApiResponse
    {
        public object Data { get; set; }
        public int Status { get; set; }
        public List<ErrorMessage> ErrorMessages { get; set; } = new List<ErrorMessage>();
    }

    public class ErrorMessage
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }
} 