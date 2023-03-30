namespace Autowatchers.Net6;

public class DummyClass
{
    public DummyClass()
    {
        GetSetString = nameof(GetSetString);
        GetProtectedSetString = nameof(GetProtectedSetString);
        GetPrivateSetString = nameof(GetPrivateSetString);
        GetInitString = nameof(GetInitString);
        GetString = nameof(GetString);

        NestedClass = new NestedClass();

    }

    public NestedClass NestedClass { get; set; }

    public string GetSetString { get; set; }

    public string GetProtectedSetString { get; protected set; }

    public string GetPrivateSetString { get; private set; }

    public string GetInitString { get; init; }

    public string GetString { get; }

}

public class NestedClass
{
    public int GetSetInt { get; set; }
    
    public double GetSetDouble { get; set; }

    public decimal GetSetDecimal { get; set; }

    public Guid GetSetGuid { get; set; }

}
