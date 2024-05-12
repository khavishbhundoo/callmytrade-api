# Changelog

## [0.2.0](https://github.com/khavishbhundoo/callmytrade-api/compare/v0.1.1...v0.2.0) (2024-05-12)


### Features

* add health check endpoint ([#27](https://github.com/khavishbhundoo/callmytrade-api/issues/27)) ([544d4a4](https://github.com/khavishbhundoo/callmytrade-api/commit/544d4a40333ce261574dae7717f668fca9dfc665))

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
