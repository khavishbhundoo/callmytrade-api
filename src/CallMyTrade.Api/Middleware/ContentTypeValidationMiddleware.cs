using Core.CallMyTrade;
using System.Text.Json;
using Light.GuardClauses;
using Serilog;

namespace CallMyTrade.Middleware;

public sealed class ContentTypeValidationMiddleware
{
    private readonly IDiagnosticContext _diagnosticContext;
    private readonly RequestDelegate _next;

    public ContentTypeValidationMiddleware(RequestDelegate next, 
        IDiagnosticContext diagnosticContext)
    {
        _next = next.MustNotBeNull();
        _diagnosticContext = diagnosticContext.MustNotBeNull();
    }

    public async Task Invoke(HttpContext context)
    {
        // Check if the request path matches the specific route you want to validate
        if (context.Request.Path.Equals(Constants.TradingViewWebhookPath))
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
                _diagnosticContext.Set("FailedResponse", failedResponse, true);
                await context.Response.WriteAsync(JsonSerializer.Serialize(failedResponse, Utils.JsonSerializerOptions));
                return;
            }
        }

        await _next(context);
    }
}