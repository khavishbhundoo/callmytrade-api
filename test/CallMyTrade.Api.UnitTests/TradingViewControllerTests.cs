using CallMyTrade.Controllers;
using Core.CallMyTrade;
using Core.CallMyTrade.Model;
using Core.CallMyTrade.Tradingview;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Time.Testing;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;

namespace CallMyTrade.Api.UnitTests;

public class TradingViewControllerTests
{
    private readonly IValidator<TradingViewRequest> _validator;
    private readonly IPhoneCallHandler _phoneCallHandler;
    private readonly TradingViewController _sut;
    private readonly TimeProvider _timeProvider;

    public TradingViewControllerTests()
    {
        _validator = new TradingViewRequestValidator();
        _phoneCallHandler = Substitute.For<IPhoneCallHandler>();
        _timeProvider = new FakeTimeProvider(startDateTime: DateTimeOffset.UtcNow);
        _sut = new TradingViewController(_validator, _phoneCallHandler, _timeProvider);
    }

    [Fact]
    public async Task GivenTradingViewRequestIsValid_ThenPhoneCallHandlerShouldBeCalledAndControllerReturnProperResponse()
    {
        // Arrange
        var tradingViewRequest = new TradingViewRequest()
        {
            Text = "BTCUSD Greater Than 9000"
        };
        var callRequest = new CallRequest()
        {
            Message = tradingViewRequest.Text
        };
        
        var callResponse = new Guid().ToString();
        _phoneCallHandler.HandleCallPhoneAsync(callRequest, CancellationToken.None).Returns(callResponse);
        
        // Act
        var result = await _sut.TradingViewAsync(tradingViewRequest, CancellationToken.None);
        _phoneCallHandler.Received(1).HandleCallPhoneAsync(callRequest, CancellationToken.None);
        result.ShouldNotBeNull();
        var objectResult = result as ObjectResult;
        objectResult.ShouldNotBeNull();
        objectResult.StatusCode.ShouldBe(StatusCodes.Status201Created);
        objectResult.Value.ShouldBeEquivalentTo(new PhoneCallResponse()
        {
            CallResponse = callResponse,
            UtcDateTime = _timeProvider.GetUtcNow().UtcDateTime
        });
    }
    
    [Fact]
    public async Task GivenPhoneCallFailedWithException_ControllerReturnResponseWithException()
    {
        // Arrange
        var tradingViewRequest = new TradingViewRequest()
        {
            Text = "BTCUSD Greater Than 9000"
        };
        var callRequest = new CallRequest()
        {
            Message = tradingViewRequest.Text
        };
        var exception = new Exception("Call failed");
        _phoneCallHandler.HandleCallPhoneAsync(callRequest, CancellationToken.None).Throws(exception);
        
        // Act
        var result = await _sut.TradingViewAsync(tradingViewRequest, CancellationToken.None);
        _phoneCallHandler.Received(1).HandleCallPhoneAsync(callRequest, CancellationToken.None);
        result.ShouldNotBeNull();
        var objectResult = result as ObjectResult;
        objectResult.ShouldNotBeNull();
        objectResult.StatusCode.ShouldBe(StatusCodes.Status403Forbidden);
        objectResult.Value.ShouldBeEquivalentTo(new FailedResponse()
        {
            ExceptionMessage = exception.Message,
            ExceptionStackTrace = exception.StackTrace
        });
    }
}