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

    public PhoneCallHandler(
        IOptionsMonitor<CallMyTradeOptions> options, 
        IServiceProvider keyedServiceProvider)
    {
         options.MustNotBeNull();
        _voIpService = keyedServiceProvider.GetRequiredKeyedService<IVoIPService>(options.CurrentValue.VoIpProvider.ToString());
        _voIpService.MustNotBeNull();
    }

    public string HandleCallPhoneAsync(CallRequest callRequest, CancellationToken cancellationToken)
    {
        return _voIpService.SendCallAsync(callRequest);
    }
}