using System.Text;

namespace EfNavGuard;

internal ref struct IndentedStringBuilder
{
    private const byte IndentSize = 4;
    private int _indent;
    private bool _indentPending = true;

    private readonly StringBuilder _stringBuilder = new();

    public IndentedStringBuilder()
    {
        _indent = 0;
    }

    public void AppendLine(string value)
    {
        if (value.Length != 0)
        {
            DoIndent();
        }

        _stringBuilder.AppendLine(value);

        _indentPending = true;
    }

    public void AppendLine()
    {
        _stringBuilder.AppendLine();

        _indentPending = true;
    }

    public void IncrementIndent()
    {
        _indent++;
    }

    public void DecrementIndent()
    {
        if (_indent > 0)
        {
            _indent--;
        }
    }

    public override string ToString() => _stringBuilder.ToString();

    private void DoIndent()
    {
        if (_indentPending && _indent > 0)
        {
            _stringBuilder.Append(' ', _indent * IndentSize);
        }

        _indentPending = false;
    }
}