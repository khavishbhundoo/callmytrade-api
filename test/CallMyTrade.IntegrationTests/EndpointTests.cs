using System.Net;
using System.Text;
using System.Text.Json;
using Core.CallMyTrade;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Shouldly;

namespace CallMyTrade.IntegrationTests;

public class EndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory;

    public EndpointTests(WebApplicationFactory<Program> webApplicationFactory)
    {
        _webApplicationFactory = webApplicationFactory;
    }

    [Theory]
    [InlineData("text/plain")]
    [InlineData("application/json")]
    public async Task GivenValidEndpointForTradingViewWithValidDataAndValidContentTypeThenReturnSuccess(string contentType)
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        
        StringContent? content = null;
        if (contentType == "text/plain")
        {
            content = new(
                "BTCUSD Greater Than 9000",
                Encoding.UTF8,
                contentType);
        }
        else if (contentType == "application/json")
        {
            content = new(
                "{\"text\": \"BTCUSD Greater Than 9000\"}",
                Encoding.UTF8,
                contentType);
        }
        
        // Act
        var response = await client.PostAsync(Constants.TradingViewWebhookPath, content);

        // Assert
        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType.ShouldNotBeNull();
        response.Content.Headers.ContentType.ToString().ShouldBe("application/json; charset=utf-8");
        var result = await response.Content.ReadAsStringAsync();
        result.ShouldNotBeNullOrEmpty();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(null)]
    public async Task
        GivenValidEndpointForTradingViewWithValidDataAndValidContentTypeWithIsCallMyTradeDisabledThenReturnFailure(bool? enabled)
    {
        // Arrange
        var client = _webApplicationFactory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((_, config) =>
            {
                var appsettings = new Dictionary<string, string?>
                {
                    { "CallMyTrade:Enabled", enabled?.ToString() }
                };

                config.AddInMemoryCollection(appsettings);
            });
        }).CreateClient();

        StringContent content = new(
            "BTCUSD Greater Than 9000",
            Encoding.UTF8,
            "text/plain");

        // Act
        var response = await client.PostAsync(Constants.TradingViewWebhookPath, content);

        // Assert
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
        response.StatusCode.ShouldBe(HttpStatusCode.ServiceUnavailable);
        response.Content.Headers.ContentType.ShouldNotBeNull();
        response.Content.Headers.ContentType.ToString().ShouldBe("application/json; charset=utf-8");
        var result = await response.Content.ReadAsStringAsync();
        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe(JsonSerializer.Serialize(failedResponse, Utils.JsonSerializerOptions));
    }

    [Theory]
    [InlineData("text/xml")]
    public async Task GivenValidEndpointForTradingViewWithValidDataAndInvalidContentTypeThenReturnFailure(string contentType)
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();

        StringContent? content = new(
            "BTCUSD Greater Than 9000",
            Encoding.UTF8,
            contentType);

        // Act
        var response = await client.PostAsync(Constants.TradingViewWebhookPath, content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnsupportedMediaType);
        response.Content.Headers.ContentType.ShouldNotBeNull();
        response.Content.Headers.ContentType.ToString().ShouldBe("application/json; charset=utf-8");
        var result = await response.Content.ReadAsStringAsync();
        result.ShouldNotBeNullOrEmpty();
    }
    
    [Fact]
    public void ShouldInvokeCallMyTradeOptionsValidationOnStartup()
    {
        //Arrange
        var client = _webApplicationFactory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((_, config) =>
            {
                var appsettings = new Dictionary<string, string?>
                {
                    { "CallMyTrade:VoIpProvidersOptions:Twilio:TwilioAccountSid", "" }
                };

                config.AddInMemoryCollection(appsettings);
            });
        });
        
        //Act & Assert
        Should.Throw<OptionsValidationException>(() =>{ client.CreateClient(); }).Message.ShouldBe("Fluent validation failed for 'CallMyTradeOptions.VoIpProvidersOptions.Twilio.TwilioAccountSid' with the error: 'The TwilioAccountSid is required when Twilio is the VoIPProvider'.");
    }
    
    [Fact]
    public async Task GivenAppIsNotInDevelopmentAndWebhookIsTradingViewWithIpAddressIsNotValidThenShouldReturnFailure()
    {
        //Arrange
        var client = _webApplicationFactory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Production");
        }).CreateClient();
        
        StringContent content = new(
            "BTCUSD Greater Than 9000",
            Encoding.UTF8,
            "text/plain");

        // Act
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
        var response = await client.PostAsync(Constants.TradingViewWebhookPath, content);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        response.Content.Headers.ContentType.ShouldNotBeNull();
        response.Content.Headers.ContentType.ToString().ShouldBe("application/json; charset=utf-8");
        var result = await response.Content.ReadAsStringAsync();
        result.ShouldNotBeNullOrEmpty();
        result.ShouldBe(JsonSerializer.Serialize(failedResponse, Utils.JsonSerializerOptions));
    }
}