using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace EfNavGuard.Tests.Utils;

public static class TestHelper
{
    public static IEnumerable<SyntaxTree> GetAllSyntaxTrees(ImmutableArray<SourceText> sourceTexts, Dictionary<string, string>? msbuildProperties = null)
    {
        var compilation = CSharpCompilation.Create(
            assemblyName: "EfNavGuard.Tests",
            syntaxTrees: sourceTexts.Select(st => CSharpSyntaxTree.ParseText(st)).ToArray());

        var generator = new EfNavGuardGenerator();

        var properties = msbuildProperties?.ToImmutableDictionary(x => $"build_property.{x.Key}", x => x.Value);

        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
        if (properties is not null)
        {
            driver = driver.WithUpdatedAnalyzerConfigOptions(new MyAnalyzerConfigOptionsProvider(ImmutableDictionary<object, AnalyzerConfigOptions>.Empty, new MyAnalyzerConfigOptions(properties)));
        }
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

        return outputCompilation.SyntaxTrees;
    }
}