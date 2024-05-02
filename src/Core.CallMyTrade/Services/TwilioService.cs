using Core.CallMyTrade.Model;
using Core.CallMyTrade.Options;
using Light.GuardClauses;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Core.CallMyTrade.Services;

public class TwilioService : IVoIPService
{
    private readonly IOptionsMonitor<CallMyTradeOptions> _options;
    public TwilioService(IOptionsMonitor<CallMyTradeOptions> options)
    {
        _options = options.MustNotBeNull();
    }

    public string SendCallAsync(CallRequest callRequest)
    {
        try
        {
            TwilioClient.Init(_options.CurrentValue.VoIpProvidersOptions!.Twilio!.TwilioAccountSid, _options.CurrentValue.VoIpProvidersOptions!.Twilio!.TwilioAuthToken);
            var call = CallResource.Create(
                twiml: new Twiml($"<Response><Say>{callRequest.Message}</Say></Response>"),
                to: new Twilio.Types.PhoneNumber(_options.CurrentValue.VoIpProvidersOptions!.Twilio!.ToPhoneNumber),
                from: new Twilio.Types.PhoneNumber(_options.CurrentValue.VoIpProvidersOptions!.Twilio!.FromPhoneNumber)
            );
            return call.Sid;
        }
        catch (Exception e)
        {
            throw;
        }
    }
}