using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using CallMyTrade.Middleware;
using CallMyTrade.Options;
using Core.CallMyTrade;
using Core.CallMyTrade.Options;
using Core.CallMyTrade.Services;
using Core.CallMyTrade.Tradingview;
using FluentValidation;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;
using IPNetwork = Microsoft.AspNetCore.HttpOverrides.IPNetwork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ForwardedHeadersOptions>(options => 
{
    options.ForwardedForHeaderName = "Fly-Client-IP";
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor;
    options.KnownNetworks.Add(new IPNetwork(IPAddress.Any, 0)); 
    options.KnownNetworks.Add(new IPNetwork(IPAddress.IPv6Any, 0)); 
});

builder.Services.AddControllers()
    .AddJsonOptions(
        opts =>
        {
            opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        });


//Add TimeProvider
builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

//Add options
builder.Services.AddOptions();
builder.Services.AddOptions<CallMyTradeOptions>()
    .BindConfiguration("CallMyTrade")
    .ValidateFluently()
    .ValidateOnStart();

// Validators
builder.Services.AddValidatorsFromAssemblyContaining<CallMyTradeOptionsValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TradingViewRequestValidator>();

//Handlers
builder.Services.AddTransient<IPhoneCallHandler, PhoneCallHandler>();

//Services
builder.Services.AddKeyedSingleton<IVoIPService, TwilioService>("Twilio");
var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);

//Health checks
builder.Services.AddHealthChecks();

//Serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.UseForwardedHeaders();

app.MapHealthChecks("/_system/health");

//Add support to logging request with SERILOG
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

//Middlewares
app.UseMiddleware<IsCallMyTradeEnabledMiddleware>();
if (!app.Environment.IsDevelopment())
{ 
    app.UseMiddleware<ProtectEndpointMiddleware>();
}
app.UseMiddleware<ContentTypeValidationMiddleware>();
app.UseMiddleware<FormatRequestMiddleware>();
app.UseRouting();

app.MapControllers();

app.Run();
public partial class Program
{ }