using System.Runtime.InteropServices;

namespace WindowThing.Win32Interop;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
internal struct MonitorInfo
{
    public uint Size;
    public Rect Monitor;
    public Rect WorkArea;
    public uint Flags;

    public MonitorInfo()
    {
        Size = (uint)(Marshal.SizeOf(typeof(MonitorInfo)));
    }
}
