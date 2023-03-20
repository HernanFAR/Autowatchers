// This source code is partly based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5

using Autowatchers.Models;

namespace Autowatchers.FileGenerators;

internal class AutowatcherAttributeGenerator : IFileGenerator
{
    private const string Name = "Autowatcher.Attributes.g.cs";
    private readonly bool _supportsNullable;

    internal static readonly string[] AutowatcherAttributeClassNames =
    {
        "Autowatchers.WatchAttribute",
        "WatchAttribute",
        "Autowatchers.WatchIgnoreAttribute",
        "WatchIgnoreAttribute"
    };

    public AutowatcherAttributeGenerator(bool supportsNullable)
    {
        _supportsNullable = supportsNullable;
    }

    public FileData GenerateFile()
    {
        return new FileData
        (
            FileDataType.Attribute,
            Name,
            $@"{Header.Text}

{(_supportsNullable ? "#nullable enable" : string.Empty)}
using System;

namespace Autowatchers
{{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class WatchAttribute : Attribute
    {{
        public Type ToWatchType {{ get; }}

        public WatchAttribute(Type toWatchType)
        {{
            ToWatchType = toWatchType;
        }}
    }}

    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class WatchIgnoreAttribute : Attribute
    {{
    }}

}}
{(_supportsNullable ? "#nullable disable" : string.Empty)}"
        );
    }
}