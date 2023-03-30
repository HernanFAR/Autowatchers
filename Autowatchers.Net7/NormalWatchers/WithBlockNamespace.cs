using Microsoft.Extensions.Logging;

namespace Autowatchers.Net7.NormalWatchers
{
    public class WithBlockNamespace : IDisposable
    {
        private readonly ILogger<WithBlockNamespace> _logger;

        private WithBlockNamespaceWatch DummyWatch { get; }
        public DummyClass DummyClass { get; }

        public WithBlockNamespace(ILogger<WithBlockNamespace> logger)
        {
            _logger = logger;

            DummyClass = new DummyClass();
            DummyWatch = new WithBlockNamespaceWatch(DummyClass);

            DummyWatch.GetSetStringChanged += StringFunc;

        }

        public void StringFunc(string oldValue, string newValue)
        {
            _logger.LogInformation("Se ha escrito la propiedad GetSetString: Nuevo valor: {NewValue} y antiguo valor: {OldValue}.", 
                newValue, oldValue);
        }
        
        public void TestThatModifies()
        {
            DummyWatch.GetSetString = "NewName";

        }

        public void TestThatDoNotModifies()
        {
            DummyWatch.GetSetString = DummyWatch.GetSetString;

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
