# Changelog

## [0.3.0](https://github.com/khavishbhundoo/callmytrade-api/compare/0.2.1...0.3.0) (2024-05-17)


### Features

* Get real client IP addresses behind proxies / loadbalancers ([#48](https://github.com/khavishbhundoo/callmytrade-api/issues/48)) ([6ba664c](https://github.com/khavishbhundoo/callmytrade-api/commit/6ba664cbf3ff76fb3e6dadb1ad960c9bec1d9e60))


### Build

* **deps:** bump AWSSDK.CloudWatchLogs from 3.7.305.32 to 3.7.305.33 ([#50](https://github.com/khavishbhundoo/callmytrade-api/issues/50)) ([81dad28](https://github.com/khavishbhundoo/callmytrade-api/commit/81dad287dd72a967af7f083611d856c576942a8c))
* **deps:** bump libphonenumber-csharp from 8.13.36 to 8.13.37 ([#51](https://github.com/khavishbhundoo/callmytrade-api/issues/51)) ([8f332ac](https://github.com/khavishbhundoo/callmytrade-api/commit/8f332ac68db0adfe4331c6e774b9f2edcd57d429))
* **deps:** bump linear-b/gitstream-github-action from 1 to 2 ([#49](https://github.com/khavishbhundoo/callmytrade-api/issues/49)) ([c31b04b](https://github.com/khavishbhundoo/callmytrade-api/commit/c31b04bf3ceb2d1aadff06a3f94e19c51c456d15))

## [0.2.1](https://github.com/khavishbhundoo/callmytrade-api/compare/0.2.0...0.2.1) (2024-05-16)


### Build

* **deps:** bump AWSSDK.CloudWatchLogs from 3.7.305.30 to 3.7.305.32 ([#36](https://github.com/khavishbhundoo/callmytrade-api/issues/36)) ([f501b32](https://github.com/khavishbhundoo/callmytrade-api/commit/f501b328e6f7e221ba88f145ef8f914ed8ba482f))
* **deps:** bump AWSSDK.Core from 3.7.303.29 to 3.7.304.1 ([#41](https://github.com/khavishbhundoo/callmytrade-api/issues/41)) ([07c8d7f](https://github.com/khavishbhundoo/callmytrade-api/commit/07c8d7fe221192d3a026a468e8b1fd34f51035b6))
* **deps:** bump Microsoft.AspNetCore.Mvc.Testing from 8.0.4 to 8.0.5 ([#40](https://github.com/khavishbhundoo/callmytrade-api/issues/40)) ([d085021](https://github.com/khavishbhundoo/callmytrade-api/commit/d085021512467898af9555d9b69c7468b1057164))
* **deps:** bump Microsoft.Extensions.TimeProvider.Testing ([#38](https://github.com/khavishbhundoo/callmytrade-api/issues/38)) ([24a6af0](https://github.com/khavishbhundoo/callmytrade-api/commit/24a6af061ef933a81e250a22a3a99c91fc678df7))
* **deps:** bump Microsoft.NET.Test.Sdk from 17.8.0 to 17.9.0 ([#30](https://github.com/khavishbhundoo/callmytrade-api/issues/30)) ([97423cb](https://github.com/khavishbhundoo/callmytrade-api/commit/97423cb8a08801144b10e3130ae9bf051d6a55ce))
* **deps:** bump Swashbuckle.AspNetCore from 6.5.0 to 6.6.1 ([#39](https://github.com/khavishbhundoo/callmytrade-api/issues/39)) ([1879476](https://github.com/khavishbhundoo/callmytrade-api/commit/18794765381b2a4157d10029ba5b059a32c9b7b5))
* **deps:** bump Swashbuckle.AspNetCore.Annotations from 6.5.0 to 6.6.1 ([#37](https://github.com/khavishbhundoo/callmytrade-api/issues/37)) ([9930fbc](https://github.com/khavishbhundoo/callmytrade-api/commit/9930fbcc4628f23c7a9351480ecf89c8ac350e59))
* **deps:** bump xunit from 2.5.3 to 2.8.0 ([#29](https://github.com/khavishbhundoo/callmytrade-api/issues/29)) ([32e8e69](https://github.com/khavishbhundoo/callmytrade-api/commit/32e8e6977149df75b194f5631dfc41f469310b9a))
* **deps:** bump xunit.runner.visualstudio from 2.5.3 to 2.8.0 ([#31](https://github.com/khavishbhundoo/callmytrade-api/issues/31)) ([a1f1792](https://github.com/khavishbhundoo/callmytrade-api/commit/a1f1792749c94014c11e7824e6c7f1e516bdde48))

## [0.2.0](https://github.com/khavishbhundoo/callmytrade-api/compare/v0.1.1...v0.2.0) (2024-05-12)


### Features

* Add health check endpoint ([#27](https://github.com/khavishbhundoo/callmytrade-api/issues/27)) ([544d4a4](https://github.com/khavishbhundoo/callmytrade-api/commit/544d4a40333ce261574dae7717f668fca9dfc665))

### Miscellaneous Chores
* Add sample docker compose files for production

## [0.1.1](https://github.com/khavishbhundoo/callmytrade-api/compare/v0.1.0...v0.1.1) (2024-05-07)


### Miscellaneous Chores

* release 0.1.0 ([f7d0725](https://github.com/khavishbhundoo/callmytrade-api/commit/f7d07258ca980b26df608f5a61fee7ebcde65aa2))
* release 0.1.1 ([#18](https://github.com/khavishbhundoo/callmytrade-api/issues/18)) ([2bef9c5](https://github.com/khavishbhundoo/callmytrade-api/commit/2bef9c5e37a0ce9c56ea038b97b875b27d0022c9))

## [0.1.0](https://github.com/khavishbhundoo/callmytrade-api/tree/0.1.0) (2024-05-06)

### Features

* Introduced `/webhook/tradingview` endpoint to be used as webhook for TradingView with support for both text and json inputs.
* Add support for `Twilio` as a VOIP provider.
* Validate `CallMyTrade` options on startup rather than at runtime
* Allow only TradingView IP addresses to access `/webhook/tradingview` webhook endpoint in production
* Allow updates of `appsettings` without redeployment 
* Built in swagger in development environments
* Multi-arch docker image support(amd64 & arm64)
* Introduced serilog logging with timings support and built-in support for the following sinks:
  1. Console(Default)
  2. Seq
  3. Aws Cloudwatch
  4. Datadog
  5. File

### Performance
* Optimize docker image size with alpine images

### Tests
* Add basic unit and integration tests

### Miscellaneous Chores

* Initial CI/CD setup 
  1. Publish packages to GHCR via Github actions workflows
  2. Introduced release please to automate CHANGELOG generation and version bumps for future releases
  3. Add `pre-push` git hook to prevent final version tagging
  4. Add pre-commit hooks to enforce conventional commits
  5. Add dependabot for dependencies management and enforce conventional commits
  6. Build and run all tests on every PR on main branch with Github actions workflows
