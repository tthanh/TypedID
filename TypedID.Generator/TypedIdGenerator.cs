using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace TypedID.Generator;

[Generator]
public class TypedIdGenerator : IIncrementalGenerator
{

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        throw new NotImplementedException();
    }

    public void Execute(GeneratorExecutionContext context)
    {
        // Find all syntax nodes with the TypedIdAttribute
        var syntaxTrees = context.Compilation.SyntaxTrees;
        foreach (var tree in syntaxTrees)
        {
            var root = tree.GetRoot();
            foreach (var classNode in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                foreach (var attributeList in classNode.AttributeLists)
                {
                    foreach (var attribute in attributeList.Attributes)
                    {
                        var name = attribute.Name.ToString();
                        if (name.Contains("TypedId"))
                        {
                            var idName = classNode.Identifier.Text + "Id";
                            var source = $@"
public readonly record struct {idName}(Guid Value)
{{
    public static {idName} New() => new(Guid.NewGuid());
    public static {idName} New(Guid value) => new(value);
    public static {idName} Empty() => new(Guid.Empty);
    public static bool operator ==({idName} left, {idName} right) => left.Value == right.Value;
    public static bool operator !=({idName} left, {idName} right) => left.Value != right.Value;
}}
";
                            context.AddSource($"{idName}.g.cs", SourceText.From(source, Encoding.UTF8));
                        }
                    }
                }
            }
        }
    }
}