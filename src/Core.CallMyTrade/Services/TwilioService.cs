using Core.CallMyTrade.Model;
using Core.CallMyTrade.Options;
using Light.GuardClauses;
using Microsoft.Extensions.Options;
using Serilog;
using SerilogTimings.Extensions;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Core.CallMyTrade.Services;

public sealed class TwilioService : IVoIPService
{
    private readonly IOptionsMonitor<CallMyTradeOptions> _options;
    private readonly ILogger _logger;
    public TwilioService(
        IOptionsMonitor<CallMyTradeOptions> options, 
        ILogger logger)
    {
        _options = options.MustNotBeNull();
        _logger = logger.ForContext<TwilioService>().MustNotBeNull();
    }

    public string SendCallAsync(CallRequest callRequest)
    {
        using var op = _logger.BeginOperation("Making phoneCall to Twilio");
        try
        {
            
            TwilioClient.Init(_options.CurrentValue.VoIpProvidersOptions!.Twilio!.TwilioAccountSid, _options.CurrentValue.VoIpProvidersOptions!.Twilio!.TwilioAuthToken);
            var call = CallResource.Create(
                twiml: new Twiml($"<Response><Say>{callRequest.Message}</Say></Response>"),
                to: new Twilio.Types.PhoneNumber(_options.CurrentValue.VoIpProvidersOptions!.Twilio!.ToPhoneNumber),
                from: new Twilio.Types.PhoneNumber(_options.CurrentValue.VoIpProvidersOptions!.Twilio!.FromPhoneNumber)
            );
            op.Complete();
            return call.Sid;
        }
        catch (Exception e)
        {
            op.SetException(e);
            op.Abandon();               
            throw;
        }
    }
}