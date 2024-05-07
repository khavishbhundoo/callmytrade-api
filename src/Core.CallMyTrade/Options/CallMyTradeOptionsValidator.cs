using Core.CallMyTrade.Model;
using FluentValidation;
using Light.GuardClauses;
using PhoneNumbers;

namespace Core.CallMyTrade.Options;

public sealed class CallMyTradeOptionsValidator : AbstractValidator<CallMyTradeOptions> 
{
    public CallMyTradeOptionsValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(c => c.VoIpProvider)
            .NotEmpty()
            .When(c => c.Enabled)
            .WithErrorCode(Constants.VoIpProviderInvalidErrorCode)
            .WithMessage(Constants.VoIpProviderInvalidErrorMessage);
        
        RuleFor(c => c.VoIpProvidersOptions)
            .NotEmpty()
            .When(c => c.Enabled)
            .WithErrorCode(Constants.VoIpProvidersOptionsMissingErrorCode)
            .WithMessage(Constants.VoIpProvidersOptionsMissingErrorMessage);
        
        /*RuleFor(c => c.VoIpProvidersOptions!.Twilio)
            .NotEmpty()
            .When(c => c.Enabled && c.VoIpProvider == VoIPProvider.Twilio)
            .WithErrorCode("")
            .WithMessage("");*/
        
        RuleFor(c => c.VoIpProvidersOptions!.Twilio!.TwilioAccountSid)
            .NotEmpty()
            .When(c => c.Enabled && c.VoIpProvider == VoIPProvider.Twilio)
            .WithErrorCode(Constants.TwilioAccountSidMissingErrorCode)
            .WithMessage(Constants.TwilioAccountSidMissingErrorMessage);
        
        RuleFor(c => c.VoIpProvidersOptions!.Twilio!.TwilioAuthToken)
            .NotEmpty()
            .When(c => c.Enabled && c.VoIpProvider == VoIPProvider.Twilio)
            .WithErrorCode(Constants.TwilioAuthTokenMissingErrorCode)
            .WithMessage(Constants.TwilioAuthTokenMissingErrorMessage);

        RuleFor(c => c.VoIpProvidersOptions!.Twilio!.FromPhoneNumber)
            .NotEmpty()
            .When(c => c.Enabled && c.VoIpProvider == VoIPProvider.Twilio)
            .WithErrorCode(Constants.TwilioFromPhoneNumberMissingErrorCode)
            .WithMessage(Constants.TwilioFromPhoneNumberMissingErrorMessage)
            .Must(IsValidPhoneNumber)
            .WithErrorCode(Constants.TwilioFromPhoneNumberInvalidErrorCode)
            .WithMessage(Constants.TwilioFromPhoneNumberInvalidErrorMessage);
        
        RuleFor(c => c.VoIpProvidersOptions!.Twilio!.ToPhoneNumber)
            .NotEmpty()
            .When(c => c.Enabled && c.VoIpProvider == VoIPProvider.Twilio)
            .WithErrorCode(Constants.TwilioToPhoneNumberMissingErrorCode)
            .WithMessage(Constants.TwilioToPhoneNumberMissingErrorMessage)
            .Must(IsValidPhoneNumber)
            .WithErrorCode(Constants.TwilioToPhoneNumberInvalidErrorCode)
            .WithMessage(Constants.TwilioToPhoneNumberInvalidErrorMessage);
    }

    private static bool IsValidPhoneNumber(string? value)
    {
        try
        {
            if (value.IsNullOrWhiteSpace() || !value.StartsWith('+')) return false;
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var phoneNumber = phoneNumberUtil.Parse(value, null);
            return phoneNumberUtil.IsValidNumber(phoneNumber);
        }
        catch (Exception)
        {
            return false;
        }
    }
}