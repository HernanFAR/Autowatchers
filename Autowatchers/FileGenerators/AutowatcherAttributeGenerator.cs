// This source code is partly based on https://justsimplycode.com/2020/12/06/auto-generate-builders-using-source-generator-in-net-5

using Autowatchers.Models;

namespace Autowatchers.FileGenerators;

internal class AutowatcherAttributeGenerator : IFileGenerator
{
    private const string Name = "Autowatcher.Attributes.g.cs";
    private readonly bool _supportsNullable;

    internal static readonly string[] AutowatcherAttributeClassNames =
    {
        "Autowatchers.AutoWatchAttribute",
        "AutoWatchAttribute",
        "Autowatchers.AutoWatch",
        "AutoWatch"
    };

    internal static readonly string[] AutowatcherExcludeAttributePropertyNames =
    {
        "Autowatchers.AutoWatchIgnoreAttribute",
        "AutoWatchIgnoreAttribute",
        "Autowatchers.AutoWatchIgnore",
        "AutoWatchIgnore"
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
    public sealed class AutoWatchAttribute : Attribute
    {{
        public Type ToWatchType {{ get; }}
        public bool DeepWatch {{ get; set; }}

        public AutoWatchAttribute(Type toWatchType)
        {{
            ToWatchType = toWatchType;
        }}
    }}

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AutoWatchIgnoreAttribute : Attribute
    {{
    }}

}}
{(_supportsNullable ? "#nullable disable" : string.Empty)}"
        );
    }
}