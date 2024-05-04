using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.CallMyTrade;
using Core.CallMyTrade.Model;
using Core.CallMyTrade.Options;
using Core.CallMyTrade.Tradingview;
using FluentValidation;
using Light.GuardClauses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CallMyTrade.Controllers;

public sealed class TradingViewController : ControllerBase
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
    [Route("/tradingview")]
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
                Message = tradingViewRequest.Text
            }, token);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new FailedResponse()
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