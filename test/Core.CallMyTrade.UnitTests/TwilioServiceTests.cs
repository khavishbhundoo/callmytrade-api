using Core.CallMyTrade.Model;
using Core.CallMyTrade.Options;
using Core.CallMyTrade.Services;
using Microsoft.Extensions.Options;
using NSubstitute;
using Serilog;
using Shouldly;


namespace Core.CallMyTrade.UnitTests;

public class TwilioServiceTests
{
    private readonly IVoIPService _sut;
    private readonly IOptionsMonitor<CallMyTradeOptions> _options;
    private readonly ILogger _logger;
    
    public TwilioServiceTests()
    {
        _options = Substitute.For<IOptionsMonitor<CallMyTradeOptions>>();
        _options.CurrentValue.Returns(new CallMyTradeOptions()
        {
            Enabled = true,
            VoIpProvider = VoIPProvider.Twilio,
            VoIpProvidersOptions = new VoIpProvidersOptions()
            {
                Twilio = new Options.Twilio()
                {
                    TwilioAccountSid = "AC70ed67c830a959ef708f6167c1ac6edc", 
                    TwilioAuthToken  = "1c8564ee33609cb1c845831f487e27ac",
                    ToPhoneNumber = "+14108675310",
                    FromPhoneNumber = "+15005550006"
                    
                }
            }
        });
        _logger = Substitute.For<ILogger>();
        _logger = _logger.ForContext<TwilioService>();
        
        _sut = new TwilioService(_options, _logger);
    }
    [Fact]
    public void GivenValidRequestWithValidTestCredentials_ThenShouldSendPhoneCall()
    {
        var result = _sut.SendCallAsync(new CallRequest()
        {
            Message = "BTCUSD Greater Than 9000"
        });
        result.ShouldNotBeNullOrEmpty();
    }
    
    [Fact]
    public void GivenValidRequestWithInvalidPhoneNumber_ThenShouldThrowException()
    {
        var options = Substitute.For<IOptionsMonitor<CallMyTradeOptions>>();
        options.CurrentValue.Returns(new CallMyTradeOptions()
        {
            Enabled = true,
            VoIpProvider = VoIPProvider.Twilio,
            VoIpProvidersOptions = new VoIpProvidersOptions()
            {
                Twilio = new Options.Twilio()
                {
                    TwilioAccountSid = "AC70ed67c830a959ef708f6167c1ac6edc", 
                    TwilioAuthToken  = "1c8564ee33609cb1c845831f487e27ac",
                    ToPhoneNumber = "+15005550001",
                    FromPhoneNumber = "+15005550001"
                    
                }
            }
        });
        var sut = new TwilioService(options, _logger);
        
        Should.Throw<Exception>(() => sut.SendCallAsync(new CallRequest()
        {
            Message = "BTCUSD Greater Than 9000"
        }));
    }
}