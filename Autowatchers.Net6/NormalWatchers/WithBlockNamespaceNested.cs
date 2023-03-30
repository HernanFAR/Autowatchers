using Microsoft.Extensions.Logging;

namespace Autowatchers.Net6.NormalWatchers
{
    public class WithBlockNamespaceNested : IDisposable
    {
        private readonly ILogger<WithBlockNamespaceNested> _logger;

        private WithNested.BlockNamespaceWatch DummyWatch { get; }

        public WithBlockNamespaceNested(ILogger<WithBlockNamespaceNested> logger)
        {
            _logger = logger;

            DummyWatch = new WithNested.BlockNamespaceWatch(new DummyClass());

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

    public partial class WithNested
    {
        [AutoWatch(typeof(DummyClass))]
        public partial class BlockNamespaceWatch { }
    }
    
}
