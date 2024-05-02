using FluentValidation;
namespace Core.CallMyTrade.Tradingview;

public class TradingViewRequestValidator : AbstractValidator<TradingViewRequest> 
{
    public TradingViewRequestValidator()
    {
        RuleFor(tvRequest => tvRequest.Text)
            .NotEmpty()
            .WithErrorCode("text_required")
            .WithMessage("The message to be said during phone call cannot be empty");
    }
}