using System.Runtime.InteropServices;

namespace WindowThing.Win32Interop;

internal static class Kernal32
{
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern IntPtr GetModuleHandle(string lpModuleName);
}
