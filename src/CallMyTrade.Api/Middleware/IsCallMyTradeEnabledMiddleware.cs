using System.Text.Json;
using Core.CallMyTrade;
using Core.CallMyTrade.Options;
using Light.GuardClauses;
using Microsoft.Extensions.Options;
using Serilog;

namespace CallMyTrade.Middleware;

public class IsCallMyTradeEnabledMiddleware
{
    private readonly IOptionsMonitor<CallMyTradeOptions> _options;
    private readonly IDiagnosticContext _diagnosticContext;
    private readonly RequestDelegate _next;
    

    public IsCallMyTradeEnabledMiddleware(IOptionsMonitor<CallMyTradeOptions> options,
        IDiagnosticContext diagnosticContext,
        RequestDelegate next)
    {
        _options = options.MustNotBeNull();
        _diagnosticContext = diagnosticContext.MustNotBeNull();
        _next = next.MustNotBeNull();
    }

    public async Task Invoke(HttpContext context)
    {
        if (!_options.CurrentValue.Enabled)
        {
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            context.Response.ContentType = "application/json; charset=utf-8";
            var failedResponse = new FailedResponse()
            {
                ValidationErrors = new List<ValidationError>()
                {
                    new ValidationError()
                    {
                        ErrorCode = "callmytrade_disabled",
                        ErrorMessage = "CallMyTrade is currently disabled."
                    }
                }
            };
            _diagnosticContext.Set("FailedResponse", failedResponse, true);
            await context.Response.WriteAsync(JsonSerializer.Serialize(failedResponse, Utils.JsonSerializerOptions));
            return;
        }
        await _next(context);
    }
}