using System;
using System.Collections.Generic;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using Xunit;
using Xunit.Sdk;

namespace ActivityAssistent.App.UITests
{
    public class MauiAppFixture : IDisposable
    {
        public string Platform { get; }
        public AppiumDriver App { get; }

        public MauiAppFixture()
        {
            Platform = Environment.GetEnvironmentVariable("MAUI_PLATFORM")?.ToLowerInvariant() ?? string.Empty;
            var appPath = Environment.GetEnvironmentVariable("MAUI_APP_PATH");
            var deviceName = Environment.GetEnvironmentVariable("MAUI_DEVICE_NAME") ?? "Android Emulator";

            if (string.IsNullOrWhiteSpace(Platform) || string.IsNullOrWhiteSpace(appPath))
            {
                throw new XunitException("Set MAUI_PLATFORM and MAUI_APP_PATH to run UI tests.");
            }

            var options = new AppiumOptions
            {
                DeviceName = deviceName,
                App = appPath
            };
            options.PlatformName = Platform == "ios" ? "iOS" : "Android";
            options.AutomationName = Platform == "ios" ? "XCUITest" : "UiAutomator2";

            App = Platform switch
            {
                "android" => new AndroidDriver(new Uri("http://127.0.0.1:4723/"), options),
                "ios" => new IOSDriver(new Uri("http://127.0.0.1:4723/"), options),
                _ => throw new XunitException("Unsupported MAUI_PLATFORM value. Use 'android' or 'ios'.")
            };
        }

        public void Dispose()
        {
            App?.Quit();
        }
    }
}
