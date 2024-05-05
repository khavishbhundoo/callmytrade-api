using System.Net;
using System.Text;
using Core.CallMyTrade;
using Microsoft.AspNetCore.Mvc.Testing;
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
        var response = await client.PostAsync(Constants.TradingViewServicePath, content);

        // Assert
        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType.ShouldNotBeNull();
        response.Content.Headers.ContentType.ToString().ShouldBe("application/json; charset=utf-8");
        var result = await response.Content.ReadAsStringAsync();
        result.ShouldNotBeNullOrEmpty();
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
        var response = await client.PostAsync(Constants.TradingViewServicePath, content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnsupportedMediaType);
        response.Content.Headers.ContentType.ShouldNotBeNull();
        response.Content.Headers.ContentType.ToString().ShouldBe("application/json; charset=utf-8");
        var result = await response.Content.ReadAsStringAsync();
        result.ShouldNotBeNullOrEmpty();
    }
}