using Autowatchers.FuncTests.Factories;
using Microsoft.Playwright;

// ReSharper disable once CheckNamespace
namespace Autowatchers.FuncTests;

public enum EBrowser
{
    Chromium, Firefox
}

public class BrowserAssertionBuilder
{
    private readonly SolutionFixture _solutionFixture;
    private readonly BrowserTypeLaunchOptions _launchOptions;
    private readonly BrowserNewContextOptions _contextOptions;
    private Task<IBrowser>? _browserLauncher;
    private Func<IPage, Task>? _toAssert;

    public BrowserAssertionBuilder(SolutionFixture solutionFixture,
        BrowserTypeLaunchOptions launchOptions, BrowserNewContextOptions contextOptions)
    {
        _solutionFixture = solutionFixture;
        _launchOptions = launchOptions;
        _contextOptions = contextOptions;
    }

    public BrowserAssertionBuilder WithBrowser(EBrowser browser)
    {
        return browser switch
        {
            EBrowser.Chromium => WithChromium(),
            EBrowser.Firefox => WithFirefox(),
            _ => throw new InvalidOperationException(nameof(browser))
        };
    }

    public BrowserAssertionBuilder WithChromium()
    {
        _browserLauncher = _solutionFixture.Playwright
            .Chromium
            .LaunchAsync(_launchOptions);
        return this;
    }

    public BrowserAssertionBuilder WithFirefox()
    {
        _browserLauncher = _solutionFixture.Playwright
            .Firefox
            .LaunchAsync(_launchOptions);

        return this;
    }

    public BrowserAssertionBuilder Assert(Func<IPage, Task> toAssert)
    {
        _toAssert = toAssert;

        return this;
    }

    public async Task StartAssertion()
    {
        if (_toAssert is null) throw new InvalidOperationException(nameof(_toAssert));
        if (_browserLauncher is null) throw new InvalidOperationException(nameof(_browserLauncher));

        var browser = await _browserLauncher;

        var context = await browser
            .NewContextAsync(_contextOptions);

        var page = await context.NewPageAsync();

        await _toAssert!.Invoke(page);

        await browser.CloseAsync();
        await browser.DisposeAsync();

    }
}

public static class IntegratedSolutionFixtureExtensions
{
    public static BrowserAssertionBuilder StartAssertionWithConfig(this SolutionFixture @this,
        BrowserTypeLaunchOptions launchOptions,
        BrowserNewContextOptions contextOptions)
    {
        return new BrowserAssertionBuilder(@this, launchOptions, contextOptions);
    }
}
