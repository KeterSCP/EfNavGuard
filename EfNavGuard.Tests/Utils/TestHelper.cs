using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace EfNavGuard.Tests.Utils;

internal static class TestHelper
{
    public static IEnumerable<SyntaxTree> GetAllSyntaxTrees(
        ImmutableArray<CSharpSourceText> sourceTexts,
        NullableContextOptions nullableContextOptions = NullableContextOptions.Disable,
        LanguageVersion languageVersion = LanguageVersion.Default)
    {
        var cSharpParseOptions = CSharpParseOptions.Default.WithLanguageVersion(languageVersion);

        var compilation = CSharpCompilation.Create(
            assemblyName: "EfNavGuard.Tests",
            syntaxTrees: sourceTexts.Select(st => CSharpSyntaxTree.ParseText(st, cSharpParseOptions)).ToArray(),
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: nullableContextOptions));

        var generator = new EfNavGuardGenerator();

        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
        driver = driver.WithUpdatedParseOptions(cSharpParseOptions);
        _ = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out _);

        return outputCompilation.SyntaxTrees;
    }
}