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
using Serilog;
using IPNetwork = Microsoft.AspNetCore.HttpOverrides.IPNetwork;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

// Load configuration based on the environment
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();


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
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    var knownNetworks = new List<IPDetails>();
    var knownProxies = new List<IPDetails>();
    builder.Configuration.GetSection("ForwardedHeadersOptions").Bind(options);
    builder.Configuration.GetSection("ForwardedHeadersOptions:KnownNetworks").Bind(knownNetworks);
    builder.Configuration.GetSection("ForwardedHeadersOptions:KnownProxies").Bind(knownProxies);

    if (knownNetworks.Count == 0)
    {
        options.KnownNetworks.Clear();
    }
    else
    {
        foreach (var ip in knownNetworks.Select(knownNetwork => IPNetwork2.Parse(knownNetwork.ToString())))
        {
            options.KnownNetworks.Add(new IPNetwork(ip.Network, ip.Cidr));
        }
    }
    
    if (knownProxies.Count == 0)
    {
        options.KnownProxies.Clear();
    }
    else
    {
        foreach (var ip in knownProxies.Select(knownProxy => IPNetwork2.Parse(knownProxy.ToString())))
        {
            options.KnownProxies.Add(ip.Network);
        }
    }
});
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