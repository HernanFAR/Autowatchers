using Microsoft.Extensions.Logging;

namespace Autowatchers.Net7.NormalWatchers;

public class WithScopedNamespace : IDisposable
{
    private readonly ILogger<WithScopedNamespace> _logger;

    private WithScopedNamespaceClassWatch DummyWatch { get; }

    public WithScopedNamespace(ILogger<WithScopedNamespace> logger)
    {
        _logger = logger;

        DummyWatch = new WithScopedNamespaceClassWatch(new DummyClass());

        DummyWatch.GetSetStringChanged += StringFunc;

    }

    public void StringFunc(string oldValue, string newValue)
    {
        _logger.LogInformation("Se ha escrito la propiedad Name: Nuevo valor: {NewValue} y antiguo valor: {OldValue}.", 
            newValue, oldValue);
    }

    public void Test()
    {
        DummyWatch.GetSetString = "NewName";
        
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        DummyWatch.GetSetStringChanged += StringFunc;

    }
}

[AutoWatch(typeof(DummyClass))]
public partial class WithScopedNamespaceClassWatch { }
