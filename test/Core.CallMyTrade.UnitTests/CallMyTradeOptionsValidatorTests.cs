using Core.CallMyTrade.Model;
using Core.CallMyTrade.Options;
using FluentValidation;
using Shouldly;

namespace Core.CallMyTrade.UnitTests;

public class CallMyTradeOptionsValidatorTests
{
    private readonly IValidator<CallMyTradeOptions> _validator;
    
    public CallMyTradeOptionsValidatorTests()
    {
        _validator = new CallMyTradeOptionsValidator();
    }


    [Fact]
    public void GivenCallMyTradeOptionsValid_Then_ValidationShouldPass()
    {
        var options = new CallMyTradeOptions()
        {
            Enabled = true,
            VoIpProvider = VoIPProvider.Twilio,
            VoIpProvidersOptions = new VoIpProvidersOptions()
            {
                Twilio = new Options.Twilio()
                {
                    TwilioAccountSid = "AC70ed67c830a959ef708f6167c1ac6edc",
                    TwilioAuthToken = "1c8564ee33609cb1c845831f487e27ac",
                    ToPhoneNumber = "+14108675310",
                    FromPhoneNumber = "+15005550006"
                }
            }
        };

        var result = _validator.Validate(options);
        result.IsValid.ShouldBe(true);
    }
    
    [Fact]
    public void GivenVoIpProviderIsMissingOrInvalidAndEnabledIsTrue_Then_ValidationShouldFail()
    {
        var options = new CallMyTradeOptions()
        {
            Enabled = true,
            VoIpProvidersOptions = new VoIpProvidersOptions()
            {
                Twilio = new Options.Twilio()
                {
                    TwilioAccountSid = "AC70ed67c830a959ef708f6167c1ac6edc",
                    TwilioAuthToken = "1c8564ee33609cb1c845831f487e27ac",
                    ToPhoneNumber = "+14108675310",
                    FromPhoneNumber = "+15005550006"
                }
            }
        };

        var result = _validator.Validate(options);
        result.IsValid.ShouldBe(false);
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorCode.ShouldBe(Constants.VoIpProviderInvalidErrorCode);
        result.Errors[0].ErrorMessage.ShouldBe(Constants.VoIpProviderInvalidErrorMessage);
        result.Errors[0].Severity.ShouldBe(Severity.Error);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GivenTwilioAccountSidMissing_Then_ValidationShouldFail(string? twilioAccountSid)
    {
        var options = new CallMyTradeOptions()
        {
            Enabled = true,
            VoIpProvider = VoIPProvider.Twilio,
            VoIpProvidersOptions = new VoIpProvidersOptions()
            {
                Twilio = new Options.Twilio()
                {
                    TwilioAccountSid = twilioAccountSid,
                    TwilioAuthToken = "1c8564ee33609cb1c845831f487e27ac",
                    ToPhoneNumber = "+14108675310",
                    FromPhoneNumber = "+15005550006"
                }
            }
        };

        var result = _validator.Validate(options);
        result.IsValid.ShouldBe(false);
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorCode.ShouldBe(Constants.TwilioAccountSidMissingErrorCode);
        result.Errors[0].ErrorMessage.ShouldBe(Constants.TwilioAccountSidMissingErrorMessage);
        result.Errors[0].Severity.ShouldBe(Severity.Error);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GivenTwilioAuthTokenMissing_Then_ValidationShouldFail(string? twilioAuthToken)
    {
        var options = new CallMyTradeOptions()
        {
            Enabled = true,
            VoIpProvider = VoIPProvider.Twilio,
            VoIpProvidersOptions = new VoIpProvidersOptions()
            {
                Twilio = new Options.Twilio()
                {
                    TwilioAccountSid = "AC70ed67c830a959ef708f6167c1ac6edc",
                    TwilioAuthToken = twilioAuthToken,
                    ToPhoneNumber = "+14108675310",
                    FromPhoneNumber = "+15005550006"
                }
            }
        };

        var result = _validator.Validate(options);
        result.IsValid.ShouldBe(false);
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorCode.ShouldBe(Constants.TwilioAuthTokenMissingErrorCode);
        result.Errors[0].ErrorMessage.ShouldBe(Constants.TwilioAuthTokenMissingErrorMessage);
        result.Errors[0].Severity.ShouldBe(Severity.Error);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GivenTwilioToPhoneNumberMissing_Then_ValidationShouldFail(string? twilioToPhoneNumber)
    {
        var options = new CallMyTradeOptions()
        {
            Enabled = true,
            VoIpProvider = VoIPProvider.Twilio,
            VoIpProvidersOptions = new VoIpProvidersOptions()
            {
                Twilio = new Options.Twilio()
                {
                    TwilioAccountSid = "AC70ed67c830a959ef708f6167c1ac6edc",
                    TwilioAuthToken = "1c8564ee33609cb1c845831f487e27ac",
                    ToPhoneNumber = twilioToPhoneNumber,
                    FromPhoneNumber = "+15005550006"
                }
            }
        };

        var result = _validator.Validate(options);
        result.IsValid.ShouldBe(false);
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorCode.ShouldBe(Constants.TwilioToPhoneNumberMissingErrorCode);
        result.Errors[0].ErrorMessage.ShouldBe(Constants.TwilioToPhoneNumberMissingErrorMessage);
        result.Errors[0].Severity.ShouldBe(Severity.Error);
    }
    
    [Theory]
    [InlineData("+230005550006")]
    [InlineData("+15")]
    [InlineData("+1500")]
    public void GivenTwilioToPhoneNumberInvalid_Then_ValidationShouldFail(string? twilioToPhoneNumber)
    {
        var options = new CallMyTradeOptions()
        {
            Enabled = true,
            VoIpProvider = VoIPProvider.Twilio,
            VoIpProvidersOptions = new VoIpProvidersOptions()
            {
                Twilio = new Options.Twilio()
                {
                    TwilioAccountSid = "AC70ed67c830a959ef708f6167c1ac6edc",
                    TwilioAuthToken = "1c8564ee33609cb1c845831f487e27ac",
                    ToPhoneNumber = twilioToPhoneNumber,
                    FromPhoneNumber = "+15005550006"
                }
            }
        };

        var result = _validator.Validate(options);
        result.IsValid.ShouldBe(false);
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorCode.ShouldBe(Constants.TwilioToPhoneNumberInvalidErrorCode);
        result.Errors[0].ErrorMessage.ShouldBe(Constants.TwilioToPhoneNumberInvalidErrorMessage);
        result.Errors[0].Severity.ShouldBe(Severity.Error);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GivenTwilioFromPhoneNumberMissing_Then_ValidationShouldFail(string? twilioFromPhoneNumber)
    {
        var options = new CallMyTradeOptions()
        {
            Enabled = true,
            VoIpProvider = VoIPProvider.Twilio,
            VoIpProvidersOptions = new VoIpProvidersOptions()
            {
                Twilio = new Options.Twilio()
                {
                    TwilioAccountSid = "AC70ed67c830a959ef708f6167c1ac6edc",
                    TwilioAuthToken = "1c8564ee33609cb1c845831f487e27ac",
                    ToPhoneNumber = "+14108675310",
                    FromPhoneNumber = twilioFromPhoneNumber
                }
            }
        };

        var result = _validator.Validate(options);
        result.IsValid.ShouldBe(false);
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorCode.ShouldBe(Constants.TwilioFromPhoneNumberMissingErrorCode);
        result.Errors[0].ErrorMessage.ShouldBe(Constants.TwilioFromPhoneNumberMissingErrorMessage);
        result.Errors[0].Severity.ShouldBe(Severity.Error);
    }
    
    [Theory]
    [InlineData("+230005550006")]
    [InlineData("+15")]
    [InlineData("+1500")]
    public void GivenTwilioFromPhoneNumberInvalid_Then_ValidationShouldFail(string? twilioFromPhoneNumber)
    {
        var options = new CallMyTradeOptions()
        {
            Enabled = true,
            VoIpProvider = VoIPProvider.Twilio,
            VoIpProvidersOptions = new VoIpProvidersOptions()
            {
                Twilio = new Options.Twilio()
                {
                    TwilioAccountSid = "AC70ed67c830a959ef708f6167c1ac6edc",
                    TwilioAuthToken = "1c8564ee33609cb1c845831f487e27ac",
                    ToPhoneNumber = "+14108675310",
                    FromPhoneNumber = twilioFromPhoneNumber
                }
            }
        };

        var result = _validator.Validate(options);
        result.IsValid.ShouldBe(false);
        result.Errors.Count.ShouldBe(1);
        result.Errors[0].ErrorCode.ShouldBe(Constants.TwilioFromPhoneNumberInvalidErrorCode);
        result.Errors[0].ErrorMessage.ShouldBe(Constants.TwilioFromPhoneNumberInvalidErrorMessage);
        result.Errors[0].Severity.ShouldBe(Severity.Error);
    }
    
}