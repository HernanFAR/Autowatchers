 using Autowatchers.Models;
using System.Text;

namespace Autowatchers.Helpers;

internal class PropertyCodeGenerator
{
    private readonly Dictionary<EClassType, Func<ClassSymbol, string>> _dictionary;

    public PropertyCodeGenerator()
    {
        _dictionary = new Dictionary<EClassType, Func<ClassSymbol, string>>
        {
            { EClassType.Deep, GenerateDeepWatchProperty },
            { EClassType.Normal, GenerateNormalWatchProperty }
        };
    }

    private static string GenerateNormalWatchProperty(ClassSymbol classSymbol)
    {
        var stringBuilder = new StringBuilder();

        foreach (var property in classSymbol.TypedClassData.Properties)
        {
            stringBuilder.AppendLine($@"
        /// <summary>
        /// Adds a function to <see cref=""{classSymbol.FullName}.{property.Name}Changed"" /> 
        /// event listener and execute it immediately
        /// </summary>
        public void AddImmediate{property.Name}ChangedEvent(System.Action<{property.FullTypeName}, {property.FullTypeName}> callback) 
        {{
            callback.Invoke(Observed.{property.Name}, Observed.{property.Name});

            {property.Name}Changed += callback;
        }}

        /// <summary>
        /// Listener to the property <see cref=""{classSymbol.TypedClassData.FullTypeName}.{property.Name}"" /> 
        /// of the observed class
        /// </summary>
        public event System.Action<{property.FullTypeName}, {property.FullTypeName}>? {property.Name}Changed;

        /// <summary>
        /// Proxy property with get and set to <see cref=""{classSymbol.TypedClassData.FullTypeName}.{property.Name}"" 
        /// member of the observed class /> 
        /// </summary>
        public {property.FullTypeName} {property.Name} 
        {{
            get => Observed.{property.Name};
            set 
            {{
                if (!Observed.{property.Name}.Equals(value))
                {{
                    {property.Name}Changed?.Invoke(Observed.{property.Name}, value);
                }}

                Observed.{property.Name} = value;
            }}
        }}
        ");
        }

        return stringBuilder.ToString();
    }

    private static string GenerateDeepWatchProperty(ClassSymbol classSymbol)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine($@"
        /// <summary>
        /// Adds a function to the <see cref=""{classSymbol.FullName}.PropertyChanged"" /> 
        /// event listener and execute it immediately
        /// </summary>
        public void AddImmediatePropertyChangedEvent(System.Action<{classSymbol.TypedClassData.FullTypeName}, {classSymbol.TypedClassData.FullTypeName}, string?> callback) 
        {{
            callback.Invoke(Observed, Observed, null);

            PropertyChanged += callback;
        }}

        /// <summary>
        /// Listener to property changes of the observed class
        /// </summary>
        public event System.Action<{classSymbol.TypedClassData.FullTypeName}, {classSymbol.TypedClassData.FullTypeName}, string?>? PropertyChanged;
        ");

        foreach (var property in classSymbol.TypedClassData.Properties)
        {
            stringBuilder.AppendLine($@"
        /// <summary>
        /// Proxy property with get and set to <see cref=""{classSymbol.TypedClassData.FullTypeName}.{property.Name}"" 
        /// member of the observed class /> 
        /// </summary>
        public {property.FullTypeName} {property.Name} 
        {{
            get => Observed.{property.Name};
            set 
            {{
                if (!Observed.{property.Name}.Equals(value))
                {{
                    var beforeCopy = System.Text.Json.JsonSerializer.Deserialize<{classSymbol.TypedClassData.FullTypeName}>(System.Text.Json.JsonSerializer.Serialize(Observed));

                    Observed.{property.Name} = value;

                    var afterCopy = System.Text.Json.JsonSerializer.Deserialize<{classSymbol.TypedClassData.FullTypeName}>(System.Text.Json.JsonSerializer.Serialize(Observed));
    
                    if (beforeCopy is null) throw new InvalidOperationException(nameof(beforeCopy));
                    if (afterCopy is null) throw new InvalidOperationException(nameof(afterCopy));

                    
                    PropertyChanged?.Invoke(beforeCopy, afterCopy, nameof({property.Name}));
                }}
                else 
                {{                
                    Observed.{property.Name} = value;
                }}
            }}
        }}
        ");
        }

        return stringBuilder.ToString();
    }

    public Func<ClassSymbol, string> this[EClassType classType]
        => _dictionary[classType];
}
