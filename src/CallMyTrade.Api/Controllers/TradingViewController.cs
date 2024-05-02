using System.Text;
using Core.CallMyTrade;
using Core.CallMyTrade.Model;
using Core.CallMyTrade.Options;
using Core.CallMyTrade.Tradingview;
using FluentValidation;
using Light.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CallMyTrade.Controllers;

public class TradingViewController : ControllerBase
{
    private readonly IValidator<TradingViewRequest> _validator;
    private readonly IPhoneCallHandler _phoneCallHandler;
    private readonly IOptionsMonitor<CallMyTradeOptions> _options;
    
    public TradingViewController(
        IValidator<TradingViewRequest> validator, 
        IPhoneCallHandler phoneCallHandler, 
        IOptionsMonitor<CallMyTradeOptions> options)
    {
        _validator = validator.MustNotBeNull();
        _phoneCallHandler = phoneCallHandler.MustNotBeNull();
        _options = options.MustNotBeNull();
    }
    
    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("/tradingview/json")]
    [ProducesResponseType(typeof(PhoneCallResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(FailedResponse),StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(FailedResponse),StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> TradingViewAsync([FromBody]TradingViewRequest tradingViewRequest, CancellationToken token)
    {
        if (!_options.CurrentValue.Enabled)
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, new FailedResponse()
            {
                ValidationErrors = new List<ValidationError>()
                {
                    new ValidationError()
                    {
                        ErrorCode = "callmytrade_disabled",
                        ErrorMessage = "CallMyTrade is currently disabled."
                    }
                }
            });    
        }
        
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
            return StatusCode(StatusCodes.Status422UnprocessableEntity, new FailedResponse()
            {
                ValidationErrors = validationErrors
            });
        }
        
        

        string result;
        try
        {
            result = _phoneCallHandler.HandleCallPhoneAsync(new CallRequest()
            {
                Message = tradingViewRequest.Text!
            }, token);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, new FailedResponse()
            {
                ExceptionMessage = e.Message,
                ExceptionStackTrace = e.StackTrace
            });
        }
        
        
        return StatusCode(StatusCodes.Status201Created, new PhoneCallResponse()
        {
            CallResponse = result
        });
    }
    
    [HttpPost]
    [Route("/tradingview")]
    [Consumes("text/plain")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(PhoneCallResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(FailedResponse),StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(FailedResponse),StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> TradingViewAsync(CancellationToken token)
    {
        if (!_options.CurrentValue.Enabled)
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, new FailedResponse()
            {
                ValidationErrors = new List<ValidationError>()
                {
                    new ValidationError()
                    {
                        ErrorCode = "callmytrade_disabled",
                        ErrorMessage = "CallMyTrade is currently disabled."
                    }
                }
            });    
        }

        string? message;
        
        using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
        {  
            message = await reader.ReadToEndAsync(token);
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, new FailedResponse()
            {
                ValidationErrors = new List<ValidationError>()
                {
                    new ValidationError()
                    {
                        ErrorCode = "text_required",
                        ErrorMessage = "The message to be said during phone call cannot be empty"
                    }
                }
            });    
        }

        string result;
        try
        {
            result = _phoneCallHandler.HandleCallPhoneAsync(new CallRequest()
            {
                Message = message
            }, token);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, new FailedResponse()
            {
                ExceptionMessage = e.Message,
                ExceptionStackTrace = e.StackTrace
            });
        }
        
        
        return StatusCode(StatusCodes.Status201Created, new PhoneCallResponse()
        {
            CallResponse = result
        });
    }
}