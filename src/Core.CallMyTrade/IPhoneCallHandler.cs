using Core.CallMyTrade.Model;

namespace Core.CallMyTrade;

public interface IPhoneCallHandler
{
    public string HandleCallPhoneAsync(CallRequest callRequest, CancellationToken cancellationToken);
}