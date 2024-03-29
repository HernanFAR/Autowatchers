﻿using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Autowatchers.Net7.DeepWatchers
{
    public class WithBlockNamespace : IDisposable
    {
        private readonly ILogger<WithBlockNamespace> _logger;

        private WithBlockNamespaceWatch DummyWatch { get; }
        public DummyClass DummyClass { get; }

        public const string OldName = nameof(Net7.DummyClass.GetSetString);
        public const string NewName = nameof(NewName);

        public static readonly NestedClass OldNestedValue = DummyClass.CurrentNestedClassValue;
        public static readonly NestedClass NewNestedValue = new ()
        {
            GetSetDecimal = 10,
            GetSetDouble = 20,
            GetSetGuid = Guid.NewGuid(),
            GetSetInt = 30
        };

        public WithBlockNamespace(ILogger<WithBlockNamespace> logger)
        {
            _logger = logger;

            DummyClass = new DummyClass();
            DummyWatch = new WithBlockNamespaceWatch(DummyClass);

            DummyWatch.PropertyChanged += NameFunc;

        }

        private void NameFunc(DummyClass oldValue, DummyClass newValue, string? propertyName)
        {
            _logger.LogInformation("Se ha escrito la propiedad {PropertyName}: Nuevo valor: {NewValue} y antiguo valor: {OldValue}.",
                propertyName, JsonSerializer.Serialize(newValue), JsonSerializer.Serialize(oldValue));
        }
        
        public void Test1ThatModifies()
        {
            DummyWatch.GetSetString = NewName;

        }
        
        public void Test1ThatDoNotModifies()
        {
            DummyWatch.GetSetString = DummyWatch.GetSetString;

        }
        
        public void Test2ThatModifies()
        {
            DummyWatch.NestedClass = NewNestedValue;

        }

        public void Test2ThatDoNotModifies()
        {
            DummyWatch.NestedClass = DummyWatch.NestedClass;

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            DummyWatch.PropertyChanged -= NameFunc;
        }
    }
    
    [AutoWatch(typeof(DummyClass), DeepWatch = true)]
    public partial class WithBlockNamespaceWatch { }

}
