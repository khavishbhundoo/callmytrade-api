using System.Text.Json;
using System.Text.Json.Serialization;
using CallMyTrade.Middleware;
using CallMyTrade.Options;
using Core.CallMyTrade;
using Core.CallMyTrade.Options;
using Core.CallMyTrade.Services;
using Core.CallMyTrade.Tradingview;
using FluentValidation;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(
        opts =>
        {
            opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        });

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

//Serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

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
if (!app.Environment.IsDevelopment())
{ 
    app.UseMiddleware<ProtectEndpointMiddleware>();
}
app.UseMiddleware<ContentTypeValidationMiddleware>();
app.UseMiddleware<FormatRequestMiddleware>();

app.MapControllers();

app.Run();
