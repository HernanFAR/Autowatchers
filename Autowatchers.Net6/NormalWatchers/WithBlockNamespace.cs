using Microsoft.Extensions.Logging;

namespace Autowatchers.Net6.NormalWatchers
{
    public class WithBlockNamespace : IDisposable
    {
        private readonly ILogger<WithBlockNamespace> _logger;

        private WithBlockNamespaceWatch DummyWatch { get; }

        public WithBlockNamespace(ILogger<WithBlockNamespace> logger)
        {
            _logger = logger;

            DummyWatch = new WithBlockNamespaceWatch(new DummyClass());

            DummyWatch.GetSetStringChanged += StringFunc;

        }

        public void StringFunc(string oldValue, string newValue)
        {
            _logger.LogInformation("Se ha escrito la propiedad GetSetString: Nuevo valor: {NewValue} y antiguo valor: {OldValue}.", 
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

    [AutoWatch(typeof(DummyClass))]
    public partial class WithBlockNamespaceWatch { }
    
}
