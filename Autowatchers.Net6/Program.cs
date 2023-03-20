namespace Autowatchers.Net6
{
    public static class Program
    {
        public static void Main(string[] _)
        {
            var dummyClass = new DummyClass();
            var dummyWatch = new DummyClassWatch(dummyClass);

            var nameFunc = (string oldValue, string newValue) =>
            {
                Console.WriteLine($"Se ha escrito la propiedad Name: Nuevo valor: {newValue} y antiguo valor: {oldValue}.");
            };

            var emailFunc = (string oldValue, string newValue) =>
            {
                Console.WriteLine($"Se ha escrito la propiedad Email: Nuevo valor: {newValue} y antiguo valor: {oldValue}.");
            };

            var phoneNumberFunc = (string oldValue, string newValue) =>
            {
                Console.WriteLine($"Se ha escrito la propiedad PhoneNumber: Nuevo valor: {newValue} y antiguo valor: {oldValue}.");
            };
            
            dummyWatch.NameChanged += nameFunc;
            dummyWatch.EmailChanged += emailFunc;
            dummyWatch.PhoneNumberChanged += phoneNumberFunc;

            Console.WriteLine("Probando escritura en clase propiedades sin AutoWatchIgnore");
            dummyWatch.Name = "NewName";
            dummyWatch.Email = "NewEmail";
            dummyWatch.PhoneNumber = "NewPhoneNumber";
            Console.WriteLine("");

            dummyWatch.NameChanged -= nameFunc;
            dummyWatch.EmailChanged -= emailFunc;
            dummyWatch.PhoneNumberChanged -= phoneNumberFunc;

            var dummyClassWithIgnoreClass = new DummyClassWithIgnore();
            var dummyClassWithIgnoreWatch = new DummyClassWithIgnoreWatch(dummyClassWithIgnoreClass);
            
            var emailFunc2 = (string oldValue, string newValue) =>
            {
                Console.WriteLine($"Se ha escrito la propiedad Email: Nuevo valor: {newValue} y antiguo valor: {oldValue}.");
            };

            var phoneNumberFunc2 = (string oldValue, string newValue) =>
            {
                Console.WriteLine($"Se ha escrito la propiedad PhoneNumber: Nuevo valor: {newValue} y antiguo valor: {oldValue}.");
            };
            
            dummyClassWithIgnoreWatch.Email2Changed += emailFunc2;
            dummyClassWithIgnoreWatch.PhoneNumber2Changed += phoneNumberFunc2;

            Console.WriteLine("Probando escritura en clase propiedades con AutoWatchIgnore");
            dummyClassWithIgnoreWatch.Email2 = "NewEmail";
            dummyClassWithIgnoreWatch.PhoneNumber2 = "NewPhoneNumber";
            Console.WriteLine("");
            
            dummyClassWithIgnoreWatch.Email2Changed -= emailFunc;
            dummyClassWithIgnoreWatch.PhoneNumber2Changed -= phoneNumberFunc;

            Console.ReadKey();
        }
    }

    [AutoWatch(typeof(DummyClass))]
    public partial class DummyClassWatch { }

    public class DummyClass
    {
        public string Name { get; set; } = "OldName";
        public string Email { get; set; } = "OldEmail";
        public string PhoneNumber { get; set; } = "OldPhoneNumber";

    }

    [AutoWatch(typeof(DummyClassWithIgnore))]
    public partial class DummyClassWithIgnoreWatch { }

    public class DummyClassWithIgnore
    {
        [AutoWatchIgnore]
        public string Name2 { get; set; } = "OldName2";
        public string Email2 { get; set; } = "OldEmail2";
        public string PhoneNumber2 { get; set; } = "OldPhoneNumber2";

    }
}
