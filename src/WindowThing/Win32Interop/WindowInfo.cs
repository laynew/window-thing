using System.Runtime.InteropServices;

namespace WindowThing.Win32Interop;

[StructLayout(LayoutKind.Sequential)]
public struct WindowInfo
{
    public uint cbSize;
    public Rect rcWindow;
    public Rect rcClient;
    public WindowStyles dwStyle;
    public uint dwExStyle;
    public uint dwWindowStatus;
    public uint cxWindowBorders;
    public uint cyWindowBorders;
    public ushort atomWindowType;
    public ushort wCreatorVersion;

    public WindowInfo(Boolean? filler) :
        this()
    {
        cbSize = (uint)(Marshal.SizeOf(typeof(WindowInfo)));
    }
}
