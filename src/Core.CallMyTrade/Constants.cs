namespace Core.CallMyTrade;

public static class Constants
{
    public const string TradingViewWebhookPath = "/webhook/tradingview";
    
    public const string VoIpProviderInvalidErrorCode = "voIpProvider_invalid";
    public const string VoIpProviderInvalidErrorMessage = "The VoIPProvider must contain a valid value. Valid values: Twilio";
    public const string VoIpProvidersOptionsMissingErrorCode = "VoIpProvidersOptions_required";
    public const string VoIpProvidersOptionsMissingErrorMessage = "The VoIpProvidersOptions must contain valid VoIPProvider settings for the VoIPProvider used";
    public const string TwilioAccountSidMissingErrorCode = "twilioAccountSid_required";
    public const string TwilioAccountSidMissingErrorMessage = "The TwilioAccountSid is required when Twilio is the VoIPProvider";
    public const string TwilioAuthTokenMissingErrorCode = "twilioAuthToken_required";
    public const string TwilioAuthTokenMissingErrorMessage = "The TwilioAuthToken is required when Twilio is the VoIPProvider";
    public const string TwilioFromPhoneNumberMissingErrorCode = "fromPhoneNumber_required";
    public const string TwilioFromPhoneNumberMissingErrorMessage = "The Twilio FromPhoneNumber is required when Twilio is the VoIPProvider and is the phone number of the caller";
    public const string TwilioFromPhoneNumberInvalidErrorCode = "fromPhoneNumber_invalid";
    public const string TwilioFromPhoneNumberInvalidErrorMessage = "The Twilio FromPhoneNumber should be in an international format and be valid for the country associated with the number";
    
    public const string TwilioToPhoneNumberMissingErrorCode = "toPhoneNumber_required";
    public const string TwilioToPhoneNumberMissingErrorMessage = "The Twilio ToPhoneNumber is required when Twilio is the VoIPProvider and the phone number of the receiver";
    public const string TwilioToPhoneNumberInvalidErrorCode = "toPhoneNumber_invalid";
    public const string TwilioToPhoneNumberInvalidErrorMessage = "The Twilio ToPhoneNumber should be in an international format and be valid for the country associated with the number";
}