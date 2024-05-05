using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Core.CallMyTrade;
using Microsoft.AspNetCore.Http;

namespace CallMyTrade.Middleware;

public sealed class ProtectEndpointMiddleware
{
    private readonly RequestDelegate _next;

    private static readonly List<string?> ValidTradingViewIpAddresses =
    [
        "52.89.214.238",
        "34.212.75.30",
        "54.218.53.128",
        "52.32.178.7"
    ];
    
    public ProtectEndpointMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task Invoke(HttpContext context)
    {
        // Check if the request path matches the specific route you want to validate
        if (context.Request.Path.Equals(Constants.TradingViewServicePath))
        {
            /*
             * Perform IP whitelisting
             * Reference: https://www.tradingview.com/support/solutions/43000529348-about-webhooks/
             */
            if (!ValidTradingViewIpAddresses.Contains(context.Request.HttpContext.Connection.RemoteIpAddress?.ToString()))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json; charset=utf-8";
                var failedResponse = new FailedResponse()
                {
                    ValidationErrors = new List<ValidationError>()
                    {
                        new ValidationError()
                        {
                            ErrorCode = "ip_banned",
                            ErrorMessage = "Only whitelisted IP addresses allowed"
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