using System.Runtime.InteropServices;

namespace WindowThing.Win32Interop;

[Serializable]
[StructLayout(LayoutKind.Sequential)]
internal struct WindowPlacement
{
    public uint Length;
    public uint Flags;
    public ShowWindowEnum ShowCmd;
    public Point MinPosition;
    public Point MaxPosition;
    public Rect NormalPosition;
}
