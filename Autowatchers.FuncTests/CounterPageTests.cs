using Autowatchers.FuncTests.Poms;
using FluentAssertions;
using Microsoft.Playwright;

namespace Autowatchers.FuncTests;

public class CounterPageTests : IClassFixture<SolutionFixture>
{
    private readonly SolutionFixture _solutionFixture;

    public CounterPageTests(SolutionFixture solutionFixture)
    {
        _solutionFixture = solutionFixture;
    }

    [Theory]
    [InlineData(EBrowser.Chromium)]
    [InlineData(EBrowser.Firefox)]
    public async Task TestCounterPage_CountIsTen(EBrowser browser)
    {
        var browserLaunchOptions = new BrowserTypeLaunchOptions
        {
            Headless = true
        };

        var browserContextOptions = new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true
        };

        await _solutionFixture
            .StartAssertionWithConfig(browserLaunchOptions, browserContextOptions)
            .WithBrowser(browser)
            .Assert(async page =>
            {
                // Arrange
                var baseUrl = _solutionFixture.Blazor.ServerAddress.ToString();
                var counterPom = new CounterPom(page, baseUrl);

                await counterPom.InitializeAsync();


                // Act
                await counterPom.IncrementCount();
                await counterPom.IncrementCount();
                await counterPom.IncrementCount();
                await counterPom.IncrementCount();
                await counterPom.IncrementCount();
                await counterPom.IncrementCount();
                await counterPom.IncrementCount();
                await counterPom.IncrementCount();
                await counterPom.IncrementCount();
                await counterPom.IncrementCount();


                // Assert
                (await counterPom.IsCountAtLimit())
                    .Should().BeTrue();


            })
            .StartAssertion();
    }


    [Theory]
    [InlineData(EBrowser.Chromium)]
    [InlineData(EBrowser.Firefox)]
    public async Task TestCounterPage_CountIsAboveTen(EBrowser browser)
    {
        var browserLaunchOptions = new BrowserTypeLaunchOptions
        {
            Headless = true
        };

        var browserContextOptions = new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true
        };

        await _solutionFixture
            .StartAssertionWithConfig(browserLaunchOptions, browserContextOptions)
            .WithBrowser(browser)
            .Assert(async page =>
            {
                // Arrange
                var baseUrl = _solutionFixture.Blazor.ServerAddress.ToString();
                var counterPom = new CounterPom(page, baseUrl);

                await counterPom.InitializeAsync();


                // Act
                await counterPom.IncrementCount();
                await counterPom.IncrementCount();
                await counterPom.IncrementCount();
                await counterPom.IncrementCount();
                await counterPom.IncrementCount();
                await counterPom.IncrementCount();
                await counterPom.IncrementCount();
                await counterPom.IncrementCount();
                await counterPom.IncrementCount();
                await counterPom.IncrementCount();
                await counterPom.IncrementCount();


                // Assert
                (await counterPom.IsCountAboveLimit())
                    .Should().BeTrue();


            })
            .StartAssertion();
    }

    [Theory]
    [InlineData(EBrowser.Chromium)]
    [InlineData(EBrowser.Firefox)]
    public async Task TestCounterComponent_CountIsTen(EBrowser browser)
    {
        var browserLaunchOptions = new BrowserTypeLaunchOptions
        {
            Headless = true
        };

        var browserContextOptions = new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true
        };

        await _solutionFixture
            .StartAssertionWithConfig(browserLaunchOptions, browserContextOptions)
            .WithBrowser(browser)
            .Assert(async page =>
            {
                // Arrange
                var baseUrl = _solutionFixture.Blazor.ServerAddress.ToString();

                var counterPom = new CounterPom(page, baseUrl);
                await counterPom.InitializeAsync();

                var counterComponentPom = counterPom.GetCounterComponentPom();


                // Act
                await counterComponentPom.IncrementCount();
                await counterComponentPom.IncrementCount();
                await counterComponentPom.IncrementCount();
                await counterComponentPom.IncrementCount();
                await counterComponentPom.IncrementCount();
                await counterComponentPom.IncrementCount();
                await counterComponentPom.IncrementCount();
                await counterComponentPom.IncrementCount();
                await counterComponentPom.IncrementCount();
                await counterComponentPom.IncrementCount();


                // Assert
                (await counterComponentPom.IsCountAtLimit())
                    .Should().BeTrue();


            })
            .StartAssertion();
    }


    [Theory]
    [InlineData(EBrowser.Chromium)]
    [InlineData(EBrowser.Firefox)]
    public async Task TestCounterComponent_CountIsAboveTen(EBrowser browser)
    {
        var browserLaunchOptions = new BrowserTypeLaunchOptions
        {
            Headless = true
        };

        var browserContextOptions = new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true
        };

        await _solutionFixture
            .StartAssertionWithConfig(browserLaunchOptions, browserContextOptions)
            .WithBrowser(browser)
            .Assert(async page =>
            {
                // Arrange
                var baseUrl = _solutionFixture.Blazor.ServerAddress.ToString();

                var counterPom = new CounterPom(page, baseUrl);
                await counterPom.InitializeAsync();

                var counterComponentPom = counterPom.GetCounterComponentPom();


                // Act
                await counterComponentPom.IncrementCount();
                await counterComponentPom.IncrementCount();
                await counterComponentPom.IncrementCount();
                await counterComponentPom.IncrementCount();
                await counterComponentPom.IncrementCount();
                await counterComponentPom.IncrementCount();
                await counterComponentPom.IncrementCount();
                await counterComponentPom.IncrementCount();
                await counterComponentPom.IncrementCount();
                await counterComponentPom.IncrementCount();
                await counterComponentPom.IncrementCount();


                // Assert
                (await counterComponentPom.IsCountAboveLimit())
                    .Should().BeTrue();


            })
            .StartAssertion();
    }
}