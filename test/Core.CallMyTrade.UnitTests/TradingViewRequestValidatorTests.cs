using Core.CallMyTrade.Tradingview;
using FluentValidation;
using Shouldly;

namespace Core.CallMyTrade.UnitTests;

public sealed class TradingViewRequestValidatorTests
{
    private readonly TradingViewRequestValidator _validator;

    public TradingViewRequestValidatorTests()
    {
        _validator = new TradingViewRequestValidator();
    }
    
    
    [Fact]
    public void GivenTradingViewRequestIsValid_Then_ValidationShouldPass()
    {
        var tradingViewRequest = new TradingViewRequest()
        {
            Text = "BTCUSD Greater Than 9000",
        };
        var result = _validator.Validate(tradingViewRequest);
        result.IsValid.ShouldBe(true);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GivenTradingViewRequestTextIsMissing_Then_ValidationShouldFail(string? text)
    {
        var tradingViewRequest = new TradingViewRequest()
        {
            Text = text,
        };
        var result = _validator.Validate(tradingViewRequest);
        result.IsValid.ShouldBe(false);
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorCode.ShouldBe(Constants.VoiceMessageInvalidErrorCode);
        result.Errors[0].ErrorMessage.ShouldBe(Constants.VoiceMessageInvalidErrorMessage);
        result.Errors[0].Severity.ShouldBe(Severity.Error);
        
    }
}