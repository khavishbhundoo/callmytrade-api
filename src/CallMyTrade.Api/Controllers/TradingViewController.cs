
using Core.CallMyTrade;
using Core.CallMyTrade.Model;
using Core.CallMyTrade.Tradingview;
using FluentValidation;
using Light.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CallMyTrade.Controllers;

[Route(Constants.TradingViewWebhookPath)]
public sealed class TradingViewController : ControllerBase
{
    private readonly IValidator<TradingViewRequest> _validator;
    private readonly IPhoneCallHandler _phoneCallHandler;
    private readonly TimeProvider _timeProvider;
    private readonly IDiagnosticContext _diagnosticContext;

    public TradingViewController(
        IValidator<TradingViewRequest> validator,
        IPhoneCallHandler phoneCallHandler, 
        TimeProvider timeProvider, 
        IDiagnosticContext diagnosticContext)
    {
        _validator = validator.MustNotBeNull();
        _phoneCallHandler = phoneCallHandler.MustNotBeNull();
        _timeProvider = timeProvider.MustNotBeNull();
        _diagnosticContext = diagnosticContext.MustNotBeNull();
    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(PhoneCallResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status415UnsupportedMediaType)]
    [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> TradingViewAsync([FromBody] TradingViewRequest tradingViewRequest,
        CancellationToken token)
    {
        var validation = await _validator.ValidateAsync(tradingViewRequest, token);

        if (!validation.IsValid)
        {
            var validationErrors = new List<ValidationError>();
            foreach (var validationFailure in validation.Errors)
            {
                validationErrors.Add(new ValidationError()
                {
                    ErrorCode = validationFailure.ErrorCode,
                    ErrorMessage = validationFailure.ErrorMessage
                });
            }

            var failedResponse = new FailedResponse()
            {
                ValidationErrors = validationErrors
            };
            _diagnosticContext.Set("FailedResponse", failedResponse, true);
            
            return StatusCode(StatusCodes.Status422UnprocessableEntity, failedResponse);
        }

        string result;
        try
        {
            result = _phoneCallHandler.HandleCallPhoneAsync(new CallRequest()
            {
                Message = tradingViewRequest.Text
            }, token);
        }
        catch (Exception e)
        {
            var failedResponse = new FailedResponse()
            {
                ExceptionMessage = e.Message,
                ExceptionStackTrace = e.StackTrace
            };
            _diagnosticContext.Set("FailedResponse", failedResponse, true);
            return StatusCode(StatusCodes.Status403Forbidden, failedResponse);
        }

        var phoneCallResponse = new PhoneCallResponse()
        {
            CallResponse = result,
            UtcDateTime = _timeProvider.GetUtcNow().UtcDateTime
        };
        
        _diagnosticContext.Set("PhoneCallResponse", phoneCallResponse, true);

        return StatusCode(StatusCodes.Status201Created, new PhoneCallResponse()
        {
            CallResponse = result,
            UtcDateTime = _timeProvider.GetUtcNow().UtcDateTime
        });
    }
}