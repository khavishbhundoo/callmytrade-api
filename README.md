[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=callmytrade-api&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=callmytrade-api)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=callmytrade-api&metric=coverage)](https://sonarcloud.io/summary/new_code?id=callmytrade-api)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=callmytrade-api&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=callmytrade-api)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=callmytrade-api&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=callmytrade-api)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=callmytrade-api&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=callmytrade-api)

# CallMyTrade API
API service that will act as a webhook and respond to `TradingView` request and give you a phone call. Never miss an alert again

## Motivation
Trading is one of the hardest way to make money and one of the reasons for failure is FOMO leading to a bad entry.
Setting an alert at a price point is one of the ways to avoid FOMO by not having to continuously watch the charts and 
stick to your trading plan. Unfortunately it's very easy to miss a TradingView 
phone notification especially when sleeping.

CallMyTrade is an API service that provide you with a webhook endpoint that you must set in TradingView.When an alert 
occurs, CallMyTrade will then use Twilio as a VOIP provider to call your mobile phone and voice out the contents of 
the alert.

Existing online services like `Callhookpro` were not a good fit for multiple reasons:
1. It does not have support for my country
2. Rates were high considering it's a recurring monthly fee

### Goals

1. Pay only for what you use model
2. Configurable with different VOIP Providers(Only Twilio is support for now)
3. Docker first approach with minimal image size for performance and security reasons
4. Easy to deploy and keep up to date in an automated manner with no downtime during deployment

## Tech Stack

- API Framework: .NET 9
- Containerization: Docker with Alpine Linux
- Architectures: `linux/amd64`, `linux/arm64`
- Testing Framework: xUnit 
- Testing: Unit tests, Integration tests & Acceptance tests. Twilio `Test credentials` were used in integration and acceptance tests.
- Package repository : GitHub Container Repository(GHCR)
- Image URI: `ghcr.io/khavishbhundoo/callmytrade-api:latest`
- TradingView Webhook endpoint: `/webhook/tradingview`
- Health check endpoint: `/_system/health`

## Requirements

### System Requirements

CallMyTrade was built with a focus on high performance with minimal resource usage. 1 vCPU and around 256 MB of RAM should be enough and the app uses around ~35 MB of RAM when idle.

### Twilio
1. Register on Twilio and reserve a phone number. The free trial will also work here.The phone number you get from Twilio  is the `FromPhoneNumber`. It is strongly recommended that you save this number as in "Emergency contacts" list so your phone will ring even in sleep / silent mode.   
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

### Method 1  : Fly.io (Preferred Approach)

Fly.io is a docker first serverless  Iaas platform that is very affordable and perfect for small projects like CallMyTrade since it will fit in [free allowances](https://fly.io/docs/about/pricing/#free-allowances)
The `fly.toml` is a sample deployment config you can use to deploy your own version of CallMyTrade in minutes. It uses the blue/green deployment to ensure no downtime during deployment using the shared `shared-cpu-1x` instance.

1. Register on fly.io and add your credit card to get $5 worth of free credit
2. Install [flyctl](https://fly.io/docs/hands-on/install-flyctl/) for your OS of choice
3. Run `fly auth login` to login to fly.io via commandline
4. Run `fly launch --no-deploy` and use of the `fly.toml` config when prompted
5. Run the following commands below to set environment variables in secrets and replace the sample values with your own details
```
fly secrets set ASPNETCORE_ENVIRONMENT=FlyIO
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

To view logs: `fly logs`
Destroy app: `fly destroy` 

Fly.io will scale to zero meaning that it will stop the app automatically when not in use which is 99.9% of the time in our use case to save costs. 
When a trading view alert hits our webhook endpoint, fly.io will provision the infra again and CallMyTrade will then process the alert. The downside is that there will be a small delay in processing the request.

```
2024-05-17T07:50:30Z runner[91852903c72378] lax [info]Machine started in 498ms
2024-05-17T07:50:31Z proxy[91852903c72378] lax [info]machine started in 1.503470705s
2024-05-17T07:50:31Z proxy[91852903c72378] lax [info]machine became reachable in 6.093029ms
2024-05-17T07:50:32Z app[91852903c72378] lax [info][07:50:32 INF] Making phoneCall to Twilio completed in 623.7 ms
2024-05-17T07:50:32Z app[91852903c72378] lax [info][07:50:32 INF] HTTP POST /webhook/tradingview responded 201 in 734.0599 ms
2024-05-17T07:50:37Z app[91852903c72378] lax [info][07:50:37 INF] HTTP GET /_system/health responded 200 in 5.9734 ms
2024-05-17T07:50:38Z health[91852903c72378] lax [info]Health check on port 8080 is now passing.
2024-05-17T07:50:57Z app[91852903c72378] lax [info][07:50:57 INF] HTTP GET /_system/health responded 200 in 0.2726 ms
2024-05-17T07:50:58Z health[91852903c72378] lax [info]Health check on port 8080 is now passing.
```

From the logs we can see that the time taken to spin up the machine and process the request took ~2.2 seconds in the worst case scenario(1.5 s + 6.0 ms + 734 ms).As per [TradingView webhook docs](https://www.tradingview.com/support/solutions/43000529348-about-webhooks/), webhooks should not take more than 3 seconds to return a response.  
In case this behaviour is not acceptable for your trading strategy then modify `fly.toml` to set `min_machines_running = 1`.

### How to update the app ?

1. Find your app hostname with `fly status`
2. Go to `https://YOUR_FLY_HOSTNAME/_system/health` to find out what version you are running.
3. Check the [CHANGELOG](https://github.com/khavishbhundoo/callmytrade-api/blob/main/CHANGELOG.md) to see what versions have been released and the respective changes
4. Run `fly deloy` to update to latest version


### Method 2 : Docker Compose
Some cloud providers support `docker-compose.yml` deployments.For example in AWS you can use Elastic Beanstalk or Fargate service. 
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


## How to use the webhook
Set the [webhook URL](https://www.tradingview.com/support/solutions/43000529348-about-webhooks/) in notifications in the alert window in Tradingview. Assuming your web host url is `https://awesomehost.com/` then the Webhook URL should be `https://awesomehost.com/webhook/tradingview`.

# Running locally

1. Clone the repository
2. Run `docker-compose up -d --build`

# Running acceptance tests

1. Clone the repository
2. Run `docker-compose up -d --build`
3. Navigate to `test/CallMyTrade.Api.AcceptanceTests`
4. Run `npm install`
5. Run `npm test`

- To test with another HOST other than localhost
 
`HOST=https://awesomehost.com npm test`

- Run a single test or tests that match a specific filename (for example [tradingview.test.ts](https://vitest.dev/guide/filtering.html#test-filtering))

`npm test tradingview`

- Run tests in [UI watch mode](https://vitest.dev/guide/ui.html):

`npm run test:ui`



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
