using System.Reflection;

namespace WindowThing.Diagnostics;

public static class DebugExtensions
{
    public static string PrintDebug<T>(this T obj) where T : struct
    {
        var debugStringBuilder = new DebugStringBuilder();
        PrintStruct(obj, debugStringBuilder);
        return debugStringBuilder.ToString();
    }

    private static void PrintStruct(object? obj, DebugStringBuilder sb, string label = "", int level = 0)
    {
        if (obj == null)
        {
            sb.AppendLine($"{label}: <null>");
            return;
        }

        if (obj.GetType().IsPrimitive || obj is string)
        {
            sb.AppendLine($"{label}: {obj}");
            return;
        }

        var fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        sb.AppendLine(level > 0 ? $"{label}:" : $"{obj.GetType().FullName}:");

        sb.Indent();

        foreach (var fieldInfo in fields)
        {
            PrintStruct(
                fieldInfo.GetValue(obj),
                sb, $"{fieldInfo.Name} ({fieldInfo.FieldType.FullName})",
                level + 1);
        }

        sb.Unindent();
    }
}
