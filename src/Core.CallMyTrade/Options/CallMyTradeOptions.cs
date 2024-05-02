using Core.CallMyTrade.Model;

namespace Core.CallMyTrade.Options;

public class CallMyTradeOptions
{
    public bool Enabled { get; set; }
    public VoIPProvider VoIpProvider { get; set; }
    
    public VoIpProvidersOptions? VoIpProvidersOptions { get; set; }
}
