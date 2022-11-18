using System.Text;

namespace WindowThing.Diagnostics;

public class DebugStringBuilder
{
    private readonly StringBuilder _stringBuilder = new();
    private int _indentLevel = 0;

    public void AppendLine(string value)
    {
        for (int i = 0; i < _indentLevel; i++)
        {
            _stringBuilder.Append('\t');
        }

        _stringBuilder.AppendLine(value);
    }

    public void Indent()
    {
        _indentLevel++;
    }

    public void Unindent()
    {
        _indentLevel--;
    }

    public override string ToString()
    {
        return _stringBuilder.ToString();
    }
}