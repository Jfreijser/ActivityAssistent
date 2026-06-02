# Testing Guide

## Unit Tests (xUnit)
- Project: ActivityAssistent.Api.Tests
- Command: `dotnet test ActivityAssistent.Api.Tests/ActivityAssistent.Api.Tests.csproj`

## Integration Tests (xUnit + WebApplicationFactory)
- Project: ActivityAssistent.Api.IntegrationTests
- Command: `dotnet test ActivityAssistent.Api.IntegrationTests/ActivityAssistent.Api.IntegrationTests.csproj`

## MAUI UI/Device Tests (Appium)
These tests validate the unauthorized state in the MAUI Blazor UI.

### Prerequisites
- Appium server running: `appium` (default at http://127.0.0.1:4723/wd/hub)
- Built MAUI app package (APK for Android or .app/.ipa for iOS)
- Environment variables:
  - `MAUI_PLATFORM` = `android` or `ios`
  - `MAUI_APP_PATH` = path to the app package
  - `MAUI_DEVICE_NAME` = device/emulator name (optional, default: Android Emulator)

### Run
- Project: ActivityAssistent.App.UITests
- Command: `dotnet test ActivityAssistent.App.UITests/ActivityAssistent.App.UITests.csproj`

## Definition of Done Checklist
- All acceptance criteria are met
- Documentation is completed
- No known defects
- Unit tests passed
- Integration tests passed
- Functional tests (UI/device tests) passed
- Design patterns are applied
- No build failures
- Satisfies coding & architecture standards
- Code check-in
