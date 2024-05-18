using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Core.CallMyTrade;
using Core.CallMyTrade.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CallMyTrade.Middleware;

public class IsCallMyTradeEnabledMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IOptionsMonitor<CallMyTradeOptions> _options;

    public IsCallMyTradeEnabledMiddleware(IOptionsMonitor<CallMyTradeOptions> options,RequestDelegate next)
    {
        _next = next;
        _options = options;
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
            await context.Response.WriteAsync(JsonSerializer.Serialize(failedResponse, Utils.JsonSerializerOptions));
            return;
        }
        await _next(context);
    }
}