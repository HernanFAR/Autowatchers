using Autowatchers.BlazorWasm.Pages;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Playwright;

namespace Autowatchers.FuncTests.Poms;

internal class CounterPom
{
    public string PageUrl { get; }

    private readonly IPage _page;

    public CounterPom(IPage page, string serverAddress)
    {
        _page = page;

        PageUrl = new UriBuilder(serverAddress)
        {
            Path = Counter.PageUrl
        }.ToString();
    }


    public async Task InitializeAsync()
    {
        await _page.GotoAsync(PageUrl,
            new PageGotoOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle
            });

        await _page.WaitForSelectorAsync("#app");
        
    }

    public async Task<int> GetCurrentCount()
    {
        var currentValueSpan = await _page.QuerySelectorAsync("#PageCurrentValue");

        if (currentValueSpan is null) throw new InvalidOperationException(nameof(currentValueSpan));

        var text = await currentValueSpan.TextContentAsync();

        return string.IsNullOrEmpty(text) ? default : int.Parse(text);
    }

    public async Task<bool> IsCountAtLimit()
    {
        var limitText = await _page.InnerTextAsync("#PageCountIsAtLimit");

        return !string.IsNullOrEmpty(limitText);
    }

    public async Task<bool> IsCountAboveLimit()
    {
        var limitText = await _page.InnerTextAsync("#PageCountIsAboveLimit");
        return !string.IsNullOrEmpty(limitText);
    }

    public async Task IncrementCount()
    {
        await _page.ClickAsync("#PageButton");
    }

    public CounterComponentPom GetCounterComponentPom()
    {
        return new CounterComponentPom(_page);
    }
}

public class CounterComponentPom
{
    private readonly IPage _page;

    public CounterComponentPom(IPage page)
    {
        _page = page;
    }

    public async Task<int> GetCurrentCount()
    {
        var currentValueSpan = await _page.QuerySelectorAsync("#ComponentCurrentValue");

        if (currentValueSpan is null) throw new InvalidOperationException(nameof(currentValueSpan));

        var text = await currentValueSpan.TextContentAsync();

        return string.IsNullOrEmpty(text) ? default : int.Parse(text);
    }

    public async Task<bool> IsCountAtLimit()
    {
        var limitText = await _page.InnerTextAsync("#ComponentCountIsAtLimit");
        return !string.IsNullOrEmpty(limitText);
    }

    public async Task<bool> IsCountAboveLimit()
    {
        var limitText = await _page.InnerTextAsync("#ComponentCountIsAboveLimit");
        return !string.IsNullOrEmpty(limitText);
    }

    public async Task IncrementCount()
    {
        await _page.ClickAsync("#ComponentButton");
    }
}