namespace Autowatchers.Net7.DeepWatchers;

public partial class BlazorComponentWithCodeBehind
{
    public BlazorComponentWithCodeBehind()
    {
        DummyWatch = new BlazorComponentWithCodeBehind.Watch(new DummyClass());
    }

    public void Test()
    {
        DummyWatch.GetSetString = "NewName";

    }

    [AutoWatch(typeof(DummyClass))]
    public partial class Watch { }

    public Watch DummyWatch { get; set; }
    
}
