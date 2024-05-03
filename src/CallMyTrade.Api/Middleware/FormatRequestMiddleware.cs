using System.Text;
using System.Text.Json;
using Core.CallMyTrade;
using Core.CallMyTrade.Tradingview;

namespace CallMyTrade.Middleware;

public sealed class FormatRequestMiddleware
{
    private readonly RequestDelegate _next;

    public FormatRequestMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.Equals("/tradingview") && 
            context.Request.ContentType != null && 
            context.Request.ContentType.StartsWith("text/plain", StringComparison.OrdinalIgnoreCase))
        {
            // Read the request body
            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true);
            var requestBody = await reader.ReadToEndAsync();

            var tradingViewRequest = new TradingViewRequest()
            {
                Text = requestBody
            };
            var tradingViewRequestJson = JsonSerializer.Serialize(tradingViewRequest, Utils.JsonSerializerOptions);
            var requestBodyBytes = Encoding.UTF8.GetBytes(tradingViewRequestJson);
            context.Request.Body = new MemoryStream(requestBodyBytes);
            context.Request.ContentType = "application/json";

        }
        await _next(context);
    }

}