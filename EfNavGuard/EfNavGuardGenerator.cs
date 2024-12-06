using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace EfNavGuard;

[Generator]
public class EfNavGuardGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // TODO: check if TFM is .NET 9 or higher (partial properties are supported from .NET 9)
        var generatorOptions = context.AnalyzerConfigOptionsProvider
            .Select((provider, _) =>
            {
                var nullableEnabled = provider.GlobalOptions.TryGetValue("build_property.nullable", out var nullable) && nullable.Equals("enable", StringComparison.InvariantCultureIgnoreCase);
                return new GeneratorOptions(nullableEnabled: nullableEnabled);
            });

        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            $"{SourceGenHelpers.MarkerAttributeName}.g.cs",
            SourceText.From(SourceGenHelpers.GuardedNavigationAttributeCode, Encoding.UTF8)));

        var propertiesToAugment = context.SyntaxProvider.ForAttributeWithMetadataName
            (
                fullyQualifiedMetadataName: $"EfNavGuard.{SourceGenHelpers.MarkerAttributeName}",
                predicate: static (node, _) => IsValidSyntaxNodeForAugmentation(node),
                transform: static (syntaxCtx, _) => TransformPropertyDeclaration(syntaxCtx)
            )
            .Where(x => x is not null);

        var propertiesAndOptions = propertiesToAugment.Combine(generatorOptions);

        context.RegisterSourceOutput(propertiesAndOptions, Generate!);
    }

    private static void Generate(SourceProductionContext ctx, (PropertyAugmentationData AugmentationData, GeneratorOptions Options) args)
    {
        var (augmentationData, options) = args;

        var (
                propertyName,
                propertyFullType,
                containingTypeName,
                namespaceName,
                fileName,
                backingFieldName,
                backingFieldType,
                propertyInitializer
                ) =
            (
                augmentationData.PropertyNameStr,
                augmentationData.PropertyFullTypeStr,
                augmentationData.ContainingTypeNameStr,
                augmentationData.NamespaceNameStr,
                augmentationData.FileName,
                $"_{augmentationData.PropertyNameStr}",
                $"{augmentationData.PropertyFullTypeStr}{(options.NullableEnabled ? "?" : string.Empty)}",
                augmentationData.PropertyInitializer
            );

        var code =
            $$"""
              namespace {{namespaceName}}
              {
                  partial class {{containingTypeName}}
                  {
                      private {{backingFieldType}} {{backingFieldName}};
                  
                      public partial {{propertyFullType}} {{propertyName}}
                      {
                          get => {{backingFieldName}} ?? throw new System.InvalidOperationException("{{propertyName}} was not loaded. Make sure to include it in the query.");
                          {{propertyInitializer}} => {{backingFieldName}} = value;
                      }
                  }
              }
              """;

        ctx.AddSource(fileName, SourceText.From(code, Encoding.UTF8));
    }

    private static bool IsValidSyntaxNodeForAugmentation(SyntaxNode node)
    {
        return node is PropertyDeclarationSyntax prop && prop.AttributeLists.Count != 0 && prop.Modifiers.Any(SyntaxKind.PartialKeyword);
    }

    private static PropertyAugmentationData? TransformPropertyDeclaration(GeneratorAttributeSyntaxContext syntaxContext)
    {
        if (syntaxContext.TargetSymbol is not IPropertySymbol propertySymbol)
        {
            return null;
        }

        var containingType = propertySymbol.ContainingType;
        var containingTypeNameStr = containingType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
        var propertyFullType = propertySymbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var namespaceName = containingType.ContainingNamespace.ToDisplayString();
        // TODO: what if the property is readonly?
        var propertyInitializer = propertySymbol.SetMethod?.IsInitOnly ?? false ? "init" : "set";

        var fileName = $"{containingTypeNameStr}_{propertySymbol.Name}_Augmented.g.cs";

        return new PropertyAugmentationData(
            fileName: fileName,
            containingTypeNameStr: containingTypeNameStr,
            propertyFullTypeStr: propertyFullType,
            propertyNameStr: propertySymbol.Name,
            namespaceNameStr: namespaceName,
            propertyInitializer: propertyInitializer);
    }
}