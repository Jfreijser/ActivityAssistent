using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using Xunit;

namespace ActivityAssistent.App.UITests
{
    public class UnauthorizedUiTests : IClassFixture<MauiAppFixture>
    {
        private readonly MauiAppFixture _fixture;

        public UnauthorizedUiTests(MauiAppFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Home_Unauthorized_ShowsLoginCallToAction()
        {
            Assert.NotEmpty(FindByText("Go to login"));
        }

        [Fact]
        public void Home_Unauthorized_ShowsWelcomeMessage()
        {
            Assert.NotEmpty(FindByText("Welcome to ActivityAssistent"));
        }

        [Fact]
        public void Home_Unauthorized_ShowsSignInDescription()
        {
            Assert.NotEmpty(FindByText("Sign in to view your conversations, action points, and AI summaries."));
        }

        private IReadOnlyCollection<IWebElement> FindByText(string text)
        {
            var selectors = _fixture.Platform switch
            {
                "ios" => new List<By>
                {
                    By.XPath($"//*[@label='{text}']"),
                    By.XPath($"//*[@name='{text}']"),
                    By.XPath($"//*[@value='{text}']")
                },
                _ => new List<By>
                {
                    By.XPath($"//*[@text='{text}']"),
                    By.XPath($"//*[@content-desc='{text}']")
                }
            };

            foreach (var selector in selectors)
            {
                var matches = _fixture.App.FindElements(selector);
                if (matches.Count > 0)
                {
                    return matches;
                }
            }

            return new List<IWebElement>();
        }
    }
}
