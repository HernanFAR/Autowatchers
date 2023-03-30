﻿using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Autowatchers.Net6.DeepWatchers;

public class WithScopedNamespace : IDisposable
{
    private readonly ILogger<WithScopedNamespace> _logger;

    private WithScopedNamespaceClassWatch DummyWatch { get; }

    public WithScopedNamespace(ILogger<WithScopedNamespace> logger)
    {
        _logger = logger;

        DummyWatch = new WithScopedNamespaceClassWatch(new DummyClass());

        DummyWatch.PropertyChanged += NameFunc;

    }

    public void NameFunc(DummyClass oldValue, DummyClass newValue)
    {
        _logger.LogInformation("Se ha escrito la propiedad GetSetString: Nuevo valor: {NewValue} y antiguo valor: {OldValue}.",
            JsonSerializer.Serialize(newValue), JsonSerializer.Serialize(oldValue));
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

[AutoWatch(typeof(DummyClass), DeepWatch = true)]
public partial class WithScopedNamespaceClassWatch { }
