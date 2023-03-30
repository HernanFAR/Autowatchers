using Microsoft.Extensions.Logging;

namespace Autowatchers.Net6.NormalWatchers;

public class WithScopedNamespaceNested : IDisposable
{
    private readonly ILogger<WithScopedNamespaceNested> _logger;

    private WithNested.ScopedNamespaceWatch DummyWatch { get; }

    public WithScopedNamespaceNested(ILogger<WithScopedNamespaceNested> logger)
    {
        _logger = logger;

        DummyWatch = new WithNested.ScopedNamespaceWatch(new DummyClass());

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

        DummyWatch.GetSetStringChanged -= StringFunc;
    }
}

public partial class WithNested
{
    [AutoWatch(typeof(DummyClass))]
    public partial class ScopedNamespaceWatch { }
}