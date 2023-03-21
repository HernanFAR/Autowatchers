using Microsoft.Extensions.Logging;

namespace Autowatchers.Net6
{
    public class WithBlockNamespace : IDisposable
    {
        private readonly ILogger<WithBlockNamespace> _logger;

        private WithSubClassWatch DummyWatch { get; }

        public WithBlockNamespace(ILogger<WithBlockNamespace> logger)
        {
            _logger = logger;

            DummyWatch = new WithSubClassWatch(new WithBlockNamespaceClass());

            DummyWatch.NameChanged += NameFunc;
            DummyWatch.EmailChanged += EmailFunc;
            DummyWatch.PhoneNumberChanged += PhoneNumberFunc;

        }

        public void NameFunc(string oldValue, string newValue)
        {
            _logger.LogInformation("Se ha escrito la propiedad Name: Nuevo valor: {NewValue} y antiguo valor: {OldValue}.", 
                newValue, oldValue);
        }

        public void EmailFunc(string oldValue, string newValue)
        {
            _logger.LogInformation("Se ha escrito la propiedad Email: Nuevo valor: {NewValue} y antiguo valor: {OldValue}.", 
                newValue, oldValue);
        }

        public void PhoneNumberFunc(string oldValue, string newValue)
        {
            _logger.LogInformation("Se ha escrito la propiedad PhoneNumber: Nuevo valor: {NewValue} y antiguo valor: {OldValue}.", 
                newValue, oldValue);
        }

        public void Test()
        {
            DummyWatch.Name = "NewName";
            DummyWatch.Email = "NewEmail";
            DummyWatch.PhoneNumber = "NewPhoneNumber";

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            DummyWatch.NameChanged -= NameFunc;
            DummyWatch.EmailChanged -= EmailFunc;
            DummyWatch.PhoneNumberChanged -= PhoneNumberFunc;
        }
    }

    [AutoWatch(typeof(WithBlockNamespaceClass))]
    public partial class WithSubClassWatch { }

    public class WithBlockNamespaceClass
    {
        [AutoWatchIgnore]
        public string IgnoreProperty { get; set; } = "Ignore";
        public string Name { get; set; } = "OldName";
        public string Email { get; set; } = "OldEmail";
        public string PhoneNumber { get; set; } = "OldPhoneNumber";

    }
}
