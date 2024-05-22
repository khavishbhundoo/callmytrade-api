using FluentValidation;
namespace Core.CallMyTrade.Tradingview;

public sealed class TradingViewRequestValidator : AbstractValidator<TradingViewRequest> 
{
    public TradingViewRequestValidator()
    {
        RuleFor(tvRequest => tvRequest.Text)
            .NotEmpty()
            .WithErrorCode(Constants.VoiceMessageInvalidErrorCode)
            .WithMessage(Constants.VoiceMessageInvalidErrorMessage);
    }
}