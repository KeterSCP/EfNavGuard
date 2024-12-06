using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace EfNavGuard.Tests.Utils;

public class MyAnalyzerConfigOptionsProvider : AnalyzerConfigOptionsProvider
{
    private readonly ImmutableDictionary<object, AnalyzerConfigOptions> _treeDict;

    public static MyAnalyzerConfigOptionsProvider Empty { get; }
        = new(
            ImmutableDictionary<object, AnalyzerConfigOptions>.Empty,
            MyAnalyzerConfigOptions.Empty);

    internal MyAnalyzerConfigOptionsProvider(
        ImmutableDictionary<object, AnalyzerConfigOptions> treeDict,
        AnalyzerConfigOptions globalOptions)
    {
        _treeDict = treeDict;
        GlobalOptions = globalOptions;
    }

    public override AnalyzerConfigOptions GetOptions(SyntaxTree tree)
        => _treeDict.TryGetValue(tree, out var options) ? options : MyAnalyzerConfigOptions.Empty;

    public override AnalyzerConfigOptions GetOptions(AdditionalText textFile)
        => _treeDict.TryGetValue(textFile, out var options) ? options : MyAnalyzerConfigOptions.Empty;

    public override AnalyzerConfigOptions GlobalOptions { get; }
}

internal sealed class MyAnalyzerConfigOptions : AnalyzerConfigOptions
{
    public static MyAnalyzerConfigOptions Empty { get; } = new(ImmutableDictionary.Create<string, string>(KeyComparer));

    private readonly ImmutableDictionary<string, string> _backing;

    public MyAnalyzerConfigOptions(ImmutableDictionary<string, string> properties)
    {
        _backing = properties;
    }

    public override bool TryGetValue(string key, [NotNullWhen(true)] out string? value) => _backing.TryGetValue(key, out value);
}