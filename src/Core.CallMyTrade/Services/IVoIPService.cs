using Core.CallMyTrade.Model;

namespace Core.CallMyTrade.Services;

// ReSharper disable once InconsistentNaming
public interface IVoIPService
{
    string SendCallAsync(CallRequest callRequest);
}