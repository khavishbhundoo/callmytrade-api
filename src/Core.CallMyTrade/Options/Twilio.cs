namespace Core.CallMyTrade.Options;

public class Twilio
{
    public string? TwilioAccountSid { get; set; }
    public string? TwilioAuthToken { get; set; }
    
    public string? FromPhoneNumber { get; set; }
    
    public string? ToPhoneNumber { get; set; }
}