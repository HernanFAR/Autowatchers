﻿namespace Autowatchers.BlazorWasm.Watchers;

public class CounterVm
{
    public const int CounterLimit = 10;
    public int CurrentValue { get; set; }

}

[AutoWatch(typeof(CounterVm))]
public partial class AutoGeneratedCountWatcher
{

}