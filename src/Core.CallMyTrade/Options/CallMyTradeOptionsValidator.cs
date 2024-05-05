using Core.CallMyTrade.Model;
using FluentValidation;
using Light.GuardClauses;
using PhoneNumbers;

namespace Core.CallMyTrade.Options;

public sealed class CallMyTradeOptionsValidator : AbstractValidator<CallMyTradeOptions> 
{
    public CallMyTradeOptionsValidator()
    {
        RuleFor(c => c.VoIpProvider)
            .IsInEnum()
            .When(c => c.Enabled)
            .WithErrorCode("voIpProvider_invalid")
            .WithMessage("The VoIPProvider is required and must contain a valid value. Valid values: Twilio");
        
        RuleFor(c => c.VoIpProvidersOptions)
            .NotEmpty()
            .When(c => c.Enabled)
            .WithErrorCode("VoIpProvidersOptions_required")
            .WithMessage("The VoIpProvidersOptions must contain valid VoIPProvider settings for the VoIPProvider used");
        
        RuleFor(c => c.VoIpProvidersOptions!.Twilio!.TwilioAccountSid)
            .NotEmpty()
            .When(c => c.Enabled && c.VoIpProvider == VoIPProvider.Twilio)
            .WithErrorCode("twilioAccountSid_required")
            .WithMessage("The TwilioAccountSid is required when Twilio is the VoIPProvider");
        
        RuleFor(c => c.VoIpProvidersOptions!.Twilio!.TwilioAuthToken)
            .NotEmpty()
            .When(c => c.Enabled && c.VoIpProvider == VoIPProvider.Twilio)
            .WithErrorCode("twilioAuthToken_required")
            .WithMessage("The TwilioAuthToken is required when Twilio is the VoIPProvider");

        RuleFor(c => c.VoIpProvidersOptions!.Twilio!.FromPhoneNumber)
            .NotEmpty()
            .When(c => c.Enabled && c.VoIpProvider == VoIPProvider.Twilio)
            .WithErrorCode("fromPhoneNumber_required")
            .WithMessage(
                "The Twilio FromPhoneNumber is required when Twilio is the VoIPProvider and is the phone number of the caller")
            .Must(IsValidPhoneNumber)
            .WithErrorCode("fromPhoneNumber_invalid")
            .WithMessage("The Twilio FromPhoneNumber should be in an international format and be valid for the country associated with the number");
        
        RuleFor(c => c.VoIpProvidersOptions!.Twilio!.ToPhoneNumber)
            .NotEmpty()
            .When(c => c.Enabled && c.VoIpProvider == VoIPProvider.Twilio)
            .WithErrorCode("toPhoneNumber_required")
            .WithMessage("The Twilio ToPhoneNumber is required when Twilio is the VoIPProvider and the phone number of the receiver")
            .Must(IsValidPhoneNumber)
            .WithErrorCode("toPhoneNumber_invalid")
            .WithMessage("The Twilio ToPhoneNumber should be in an international format and be valid for the country associated with the number");
    }

    private bool IsValidPhoneNumber(string? value)
    {
        if (value.IsNullOrWhiteSpace() || !value.StartsWith('+')) return false;
        var phoneNumberUtil = PhoneNumberUtil.GetInstance();
        var phoneNumber = phoneNumberUtil.Parse(value, null);
        return phoneNumberUtil.IsValidNumber(phoneNumber);
    }
}