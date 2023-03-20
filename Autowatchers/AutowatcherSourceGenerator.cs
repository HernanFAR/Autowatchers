using Autowatchers.FileGenerators;
using Autowatchers.SyntaxReceiver;
using Autowatchers.Wrappers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Autowatchers;

[Generator]
public class AutowatcherSourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new AutowatcherSyntaxReceiver());

    }

    public void Execute(GeneratorExecutionContext context)
    {
        var contextWrapper = new GeneratorExecutionContextWrapper(context);

        try
        {
            InjectGeneratedClasses(contextWrapper);

            if (context.SyntaxReceiver is not AutowatcherSyntaxReceiver receiver)
            {
                return;
            }

            InjectFluentBuilderClasses(contextWrapper, receiver);
        }
        catch (Exception exception)
        {
            GenerateError(contextWrapper, exception);
        }
    }

    private static void InjectGeneratedClasses(GeneratorExecutionContextWrapper context)
    {
        var generators = new IFileGenerator[]
        {
            new AutowatcherAttributeGenerator(context.SupportsNullable)
        };

        foreach (var generator in generators)
        {
            var data = generator.GenerateFile();
            context.AddSource(data.FileName, SourceText.From(data.Text, Encoding.UTF8));
        }
    }

    private static void InjectFluentBuilderClasses(GeneratorExecutionContextWrapper context, AutowatcherSyntaxReceiver receiver)
    {
        var generator = new AutowatcherClassesGenerator(context, receiver);

        foreach (var data in generator.GenerateFiles())
        {
            context.AddSource(data.FileName, SourceText.From(data.Text, Encoding.UTF8));
        }
    }

    private static void GenerateError(GeneratorExecutionContextWrapper context, Exception exception)
    {
        var message = $"/*\r\n{nameof(AutowatcherSourceGenerator)}\r\n\r\n[Exception]\r\n{exception}\r\n\r\n[StackTrace]\r\n{exception.StackTrace}*/";
        context.AddSource("Error.g.cs", SourceText.From(message, Encoding.UTF8));
    }
}