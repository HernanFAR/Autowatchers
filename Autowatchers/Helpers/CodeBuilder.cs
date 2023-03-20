using Autowatchers.Models;
using System.Text;

namespace Autowatchers.Helpers;

internal class CodeBuilder
{
    private readonly StringBuilder _stringBuilder;

    public CodeBuilder(StringBuilder stringBuilder)
    {
        _stringBuilder = stringBuilder;
    }

    public CodeBuilder AppendReadOnlyPropertyCode(ClassSymbol classSymbol)
    {
        _stringBuilder.Append($@"
        public {classSymbol.NamedTypeSymbol.Name} Observed {{ get; }}
        ");

        return this;
    }

    public CodeBuilder AppendConstructorCode(ClassSymbol classSymbol)
    {
        _stringBuilder.Append($@"
        public {classSymbol.ClassName}({classSymbol.NamedTypeSymbol.Name} observed) 
        {{
            Observed = observed;
        }}");

        return this;
    }

    public CodeBuilder AppendPropertyCode(ClassSymbol classSymbol)
    {
        foreach (var property in classSymbol.Properties)
        {
            _stringBuilder.AppendLine($@"
        public event Action<{property.FullTypeName}, {property.FullTypeName}>? {property.Name}Changed;

        public {property.FullTypeName} {property.Name} 
        {{
            get => Observed.{property.Name};
            set 
            {{
                {property.Name}Changed?.Invoke(Observed.{property.Name}, value);

                Observed.{property.Name} = value;
            }}
        }}");
        }

        return this;
    }

    public override string ToString() => _stringBuilder.ToString();
}
