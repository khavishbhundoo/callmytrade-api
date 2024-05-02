using Core.CallMyTrade.Model;
using Core.CallMyTrade.Options;
using Core.CallMyTrade.Services;
using Light.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Core.CallMyTrade;

public sealed class PhoneCallHandler : IPhoneCallHandler
{
    private readonly IVoIPService _voIpService;
    private readonly IOptionsMonitor<CallMyTradeOptions> _options;

    public PhoneCallHandler(
        IOptionsMonitor<CallMyTradeOptions> options, 
        IServiceProvider keyedServiceProvider)
    {
        _options = options.MustNotBeNull();
        _voIpService = keyedServiceProvider.GetRequiredKeyedService<IVoIPService>(_options.CurrentValue.VoIpProvider.ToString()).MustNotBeNull();
    }

    public string HandleCallPhoneAsync(CallRequest callRequest, CancellationToken cancellationToken)
    {
        if (_options.CurrentValue.Enabled && _options.CurrentValue.VoIpProvider == VoIPProvider.Twilio)
        {
            return _voIpService.SendCallAsync(callRequest);
        }

        return string.Empty;
    }
}