namespace Autowatchers.Net6
{
    public static class Program
    {
        public static void Main(string[] _)
        {
            var dummyClass = new DummyClass();
            var dummyWatch = new DummyClassWatch(dummyClass);

            Action<string, string> nameFunc = (oldValue, newValue) =>
            {
                Console.WriteLine($"Se ha escrito la propiedad Name: Nuevo valor: {newValue} y antiguo valor: {oldValue}.");
            };

            Action<string, string> emailFunc = (oldValue, newValue) =>
            {
                Console.WriteLine($"Se ha escrito la propiedad Email: Nuevo valor: {newValue} y antiguo valor: {oldValue}.");
            };

            Action<string, string> phoneNumberFunc = (oldValue, newValue) =>
            {
                Console.WriteLine($"Se ha escrito la propiedad PhoneNumber: Nuevo valor: {newValue} y antiguo valor: {oldValue}.");
            };

            dummyWatch.NameChanged += nameFunc;
            dummyWatch.EmailChanged += emailFunc;
            dummyWatch.PhoneNumberChanged += phoneNumberFunc;

            dummyWatch.Name = "NewName";
            dummyWatch.Email = "NewEmail";
            dummyWatch.PhoneNumber = "NewPhoneNumber";


            dummyWatch.NameChanged -= nameFunc;
            dummyWatch.EmailChanged -= emailFunc;
            dummyWatch.PhoneNumberChanged -= phoneNumberFunc;
        }
    }


    [Watch(typeof(DummyClass))]
    public partial class DummyClassWatch { }

    public class DummyClass
    {
        public string Name { get; set; } = "OldName";
        public string Email { get; set; } = "OldEmail";
        public string PhoneNumber { get; set; } = "OldPhoneNumber";

    }
}
