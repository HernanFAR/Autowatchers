using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Autowatchers.Net6.DeepWatchers
{
    public class WithBlockNamespace : IDisposable
    {
        private readonly ILogger<WithBlockNamespace> _logger;

        private WithBlockNamespaceWatch DummyWatch { get; }

        public WithBlockNamespace(ILogger<WithBlockNamespace> logger)
        {
            _logger = logger;

            DummyWatch = new WithBlockNamespaceWatch(new DummyClass());

            DummyWatch.PropertyChanged += NameFunc;

        }

        public void NameFunc(DummyClass oldValue, DummyClass newValue, string? propertyName)
        {
            _logger.LogInformation("Se ha escrito la propiedad {PropertyName}: Nuevo valor: {NewValue} y antiguo valor: {OldValue}.",
                propertyName, JsonSerializer.Serialize(newValue), JsonSerializer.Serialize(oldValue));
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
    public partial class WithBlockNamespaceWatch { }

}
