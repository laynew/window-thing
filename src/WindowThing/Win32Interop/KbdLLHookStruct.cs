using System.Runtime.InteropServices;

namespace WindowThing.Win32Interop;

[StructLayout(LayoutKind.Sequential)]
public struct KbdLlHookStruct
{
    public int vkCode;
    public int scanCode;
    public int flags;
    public int time;
    public int dwExtraInfo;
}
