using Autowatchers.FuncTests.Factories;
using Microsoft.Playwright;

namespace Autowatchers.FuncTests;

public class SolutionFixture : IAsyncLifetime
{
    public IPlaywright Playwright { get; private set; } = default!;

    public AppFactory Blazor { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        InstallPlaywright();

        Blazor = new AppFactory();
        await Blazor.InitializeAsync();

        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();

    }

    public async Task DisposeAsync()
    {
        Playwright.Dispose();
        await Blazor.DisposeAsync();

    }

    private static void InstallPlaywright()
    {
        var exitCode = Program.Main(
            new[] { "install-deps" });
        if (exitCode != 0)
        {
            throw new Exception(
                $"Playwright exited with code {exitCode} on install-deps");
        }

        exitCode = Program.Main(new[] { "install" });
        if (exitCode != 0)
        {
            throw new Exception(
                $"Playwright exited with code {exitCode} on install");
        }
    }
}
