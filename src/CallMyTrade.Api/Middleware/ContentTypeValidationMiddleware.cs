using Core.CallMyTrade;

namespace CallMyTrade.Middleware;
using System.Text.Json;

public sealed class ContentTypeValidationMiddleware
{
    private readonly RequestDelegate _next;

    public ContentTypeValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Check if the request path matches the specific route you want to validate
        if (context.Request.Path.Equals("/tradingview"))
        {
            // Perform content type validation
            if (context.Request.ContentType == null || 
                (!context.Request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase) && 
                 !context.Request.ContentType.StartsWith("text/plain", StringComparison.OrdinalIgnoreCase)))
            {
                context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
                context.Response.ContentType = "application/json; charset=utf-8";
                var failedResponse = new FailedResponse()
                {
                    ValidationErrors = new List<ValidationError>()
                    {
                        new ValidationError()
                        {
                            ErrorCode = "content_type_invalid",
                            ErrorMessage =
                                "Content-Type header missing or invalid. Tradingview will send either application/json or text/plain"
                        }
                    }
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(failedResponse, Utils.JsonSerializerOptions));
                return;
            }
        }

        await _next(context);
    }
}