using Microsoft.Extensions.Logging;

namespace Autowatchers.Net6.DeepWatchers;

public class WithScopedNamespaceNested : IDisposable
{
    private readonly ILogger<WithScopedNamespaceNested> _logger;

    private WithNested.ScopedNamespaceWatch DummyWatch { get; }

    public WithScopedNamespaceNested(ILogger<WithScopedNamespaceNested> logger)
    {
        _logger = logger;

        DummyWatch = new WithNested.ScopedNamespaceWatch(new DummyClass());

        DummyWatch.PropertyChanged += NameFunc;

    }

    public void NameFunc(DummyClass oldValue, DummyClass newValue, string? propertyName)
    {
        _logger.LogInformation("Se ha escrito la propiedad {PropertyName}: Nuevo valor: {NewValue} y antiguo valor: {OldValue}.",
            propertyName, newValue, oldValue);
    }
        
    public void Test()
    {
        DummyWatch.GetSetString = "NewName";

    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        DummyWatch.PropertyChanged -= NameFunc;
    }
}

public partial class WithNested
{
    [AutoWatch(typeof(DummyClass), DeepWatch = true)]
    public partial class ScopedNamespaceWatch { }
}