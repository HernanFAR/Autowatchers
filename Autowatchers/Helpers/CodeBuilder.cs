using Autowatchers.Models;
using System.Text;

namespace Autowatchers.Helpers;

internal class CodeBuilder
{
    private readonly ClassSymbol _classSymbol;
    private readonly StringBuilder _stringBuilder;
    private readonly PropertyCodeGenerator _propertyCodeGenerator;

    public CodeBuilder(ClassSymbol classSymbol, PropertyCodeGenerator propertyCodeGenerator)
    {
        _classSymbol = classSymbol;
        _propertyCodeGenerator = propertyCodeGenerator;
        _stringBuilder = new StringBuilder();
    }

    public CodeBuilder AppendReadOnlyPropertyCode()
    {
        _stringBuilder.Append($@"
        /// <summary>
        /// The instance to observe
        /// </summary>
        public {_classSymbol.TypedClassData.FullTypeName} Observed {{ get; }}
        ");

        return this;
    }

    public CodeBuilder AppendConstructorCode()
    {
        _stringBuilder.Append($@"
        /// <summary>
        /// A <see cref=""{_classSymbol.FullName}"" /> constructor
        /// </summary>
        public {_classSymbol.ClassName}({_classSymbol.TypedClassData.FullTypeName} observed) 
        {{
            Observed = observed;
        }}
        ");

        return this;
    }

    public CodeBuilder AppendPropertyCode()
    {
        _stringBuilder.AppendLine(_propertyCodeGenerator[_classSymbol.ClassData.ClassType](_classSymbol));

        return this;
    }

    public override string ToString() => _stringBuilder.ToString();
}
