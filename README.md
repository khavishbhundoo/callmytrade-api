# CallMyTrade API
API service that will respond to `TradingView` webhooks and ring your mobile phone. Never miss an alert again

## Motivation
Trading is one of the hardest way to make money and one of the reasons for failure is FOMO leading to a bad entry.
Setting an alert at a price point is one of the ways to avoid FOMO by not having to continuously watch the charts and 
stick to your trading plan. Unfortunately it's very easy to miss a TradingView 
phone notification especially when sleeping.

CallMyTrade is an API service that provide you with a webhook endpoint that you must set in TradingView.When an alert 
occurs, CallMyTrade will then use Twilio as a VOIP provider to call your mobile phone and voice out the contents of 
the alert.

## Tech Stack

- API Framework: .NET 8
- Containerization: Docker with Alpine Linux
- Architectures: `linux/amd64`, `linux/arm64`
- Testing Framework: xUnit 
- Testing: Unit tests, Integration tests & Acceptance tests
- Package repository : GitHub Container Repository(GHCR)
- Image URI: `ghcr.io/khavishbhundoo/callmytrade-api:latest`
- TradingView Webhook endpoint: `/webhook/tradingview`
- Health check endpoint: `/_system/health`

## Requirements

### System Requirements

CallMyTrade was built with a focus on high performance with minimal resource usage. 1 vCPU and around 256 MB of RAM should be enough and the app uses around ~60 MB of RAM when idle.

### Twilio
1. Register on Twilio and reserve a phone number. The free trial will also work here.The phone number you get from Twilio  is the `FromPhoneNumber`.
2. Get Twilio API Live credentials(Account-> API keys & tokens). You will need `Account SID` & `Auth token`. 
3. Make sure the country from which your phone number is based of is selected in `Voice Geographic Permissions` found under the `Develop->Voice->Setting->Geo permissions`.
4. Your phone number in full international format is the `ToPhoneNumber`.

### TradingView
1. Ensure you have two-factor authentication enabled in [Profile Settings](https://www.tradingview.com/u/#settings-profile) because it is mandatory to be able to set webhooks in alerts.

## Features

1. Webhook API service for TradingView with built-in support for `Twilio` as VOIP provider
2. Optimized images, based on Alpine Linux, supporting multiple architectures including `linux/amd64` and `linux/arm64` for seamless deployment.
 
## Deployment 
The recommended way to deploy the API with your own VOIP details is through Docker.

### Method 1 : Docker Compose
Some cloud providers support `docker-compose.yml` deployments.For example in AWS can use Elastic Beanstalk service. 
Create a `docker-compose.yml` as shown below and change details with your own.   
```
services:
  callmytrade.api:
    image: ghcr.io/khavishbhundoo/callmytrade-api:latest
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - CallMyTrade__Enabled=true
      - CallMyTrade__VoIpProvider=Twilio
       # REPLACE WITH YOUR OWN DETAILS
      - CallMyTrade__VoIpProvidersOptions__Twilio__TwilioAccountSid=AC70ed67c830a959ef708f6167c1ac6edc
      - CallMyTrade__VoIpProvidersOptions__Twilio__TwilioAuthToken=1c8564ee33609cb1c845831f487e27ac
      - CallMyTrade__VoIpProvidersOptions__Twilio__ToPhoneNumber=+14108675310
      - CallMyTrade__VoIpProvidersOptions__Twilio__FromPhoneNumber=+15005550006
      # REPLACE WITH YOUR OWN DETAILS
    ports:
      - 80:8080
```
### Method 2 : Fly.io (Preferred Approach)

Fly.io is a docker first serverless  Iaas platform that is very affordable and perfect for small projects like CallMyTrade since it will fit in [free allowances](https://fly.io/docs/about/pricing/#free-allowances) 
The `fly.toml` is a sample deployment config you can use to deploy your own version of CallMyTrade in minutes. Fly.io will scale to ERO 

1. Register on fly.io and add your credit card to get $5 worth of free credit
2. Install [flyctl](https://fly.io/docs/hands-on/install-flyctl/) for your OS of choice
3. Run `fly auth login` to login to fly.io via commandline
4. Run `fly launch --no-deploy` and use of the `fly.toml` config when prompted
5. Run the following commands below to set environment variables in secrets and replace the sample values with your own details
```
fly secrets set ASPNETCORE_ENVIRONMENT=Production
fly secrets set CallMyTrade__Enabled=true
fly secrets set CallMyTrade__VoIpProvider=Twilio
# REPLACE WITH YOUR OWN DETAILS
fly secrets set CallMyTrade__VoIpProvidersOptions__Twilio__FromPhoneNumber=+14108675310
fly secrets set CallMyTrade__VoIpProvidersOptions__Twilio__ToPhoneNumber=+15005550006
fly secrets set CallMyTrade__VoIpProvidersOptions__Twilio__TwilioAccountSid=AC70ed67c830a959ef708f6167c1ac6edc
fly secrets set CallMyTrade__VoIpProvidersOptions__Twilio__TwilioAuthToken=1c8564ee33609cb1c845831f487e27ac
# REPLACE WITH YOUR OWN DETAILS
```
6. Deploy the app with the `fly deploy`

## LICENSE

```
MIT License

Copyright (c) 2024 Khavish Anshudass Bhundoo

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
