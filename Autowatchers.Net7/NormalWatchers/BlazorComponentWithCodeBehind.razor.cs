using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Autowatchers.Net7.NormalWatchers;

public partial class BlazorComponentWithCodeBehind : IDisposable
{
    [Inject] private ILogger<BlazorComponentWithCodeBehind> Logger { get; set; } = default!;

    public BlazorComponentWithCodeBehind()
    {
        DummyWatch = new BlazorComponentWithCodeBehind.Watch(new DummyClass());

        DummyWatch.GetSetStringChanged += StringFunc;
    }

    public void StringFunc(string newValue, string oldValue)
    {
        Logger.LogInformation("Se ha escrito la propiedad GetSetString: Nuevo valor: {NewValue} y antiguo valor: {OldValue}.",
            newValue, oldValue);
    }

    public void Test()
    {
        DummyWatch.GetSetString = "NewName";

    }

    [AutoWatch(typeof(DummyClass))]
    public partial class Watch { }

    public Watch DummyWatch { get; set; }

    public void Dispose()
    {
        DummyWatch.GetSetStringChanged -= StringFunc;
    }
}
