using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace EfNavGuard.Tests.Utils;

internal class CSharpSourceText : SourceText
{
    public override Encoding? Encoding => null;
    public override int Length => _source.Length;
    public override char this[int position] => _source[position];

    private readonly string _source;

    private CSharpSourceText(string source)
    {
        _source = source;
    }

    public static CSharpSourceText From([StringSyntax("csharp")] string source) => new(source);
    public override void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count) => _source.CopyTo(sourceIndex, destination, destinationIndex, count);
}