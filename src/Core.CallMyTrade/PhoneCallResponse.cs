namespace Core.CallMyTrade;

public sealed record PhoneCallResponse
{
    public string? CallResponse { get; init; }
    
    public DateTime UtcDateTime { get; init; } = DateTime.UtcNow;
}