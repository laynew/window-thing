using System.Diagnostics;
using System.Text;
using WindowThing.Diagnostics;
using WindowThing.InputHandling;
using WindowThing.Win32Interop;

namespace WindowThing;

internal class WindowSnapCommand
{
    private bool _bound;
    private readonly KeyHook _keyHook;

    public WindowSnapCommand(KeyHook keyHook)
    {
        _keyHook = keyHook;
    }

    private void HandleIfShortcut(object? sender, KeyHookKeyEventArgs e)
    {
        e.Handled = IsShortcut(e);
    }

    private bool IsShortcut(KeyHookKeyEventArgs e)
    {
        return e.KeyCode == Keys.G && e.IsWindowsKeyPressed && e.IsWindowsKeyPressed;
    }

    private void KeyHookOnKeyPressed(object? sender, KeyHookKeyEventArgs e)
    {
        if (IsShortcut(e))
        {
            Debug.WriteLine($"SHIFT+WIN+G pressed");
            CenterActiveWindow();
        }
    }

    private void CenterActiveWindow()
    {
        var activeWindow = User32.GetForegroundWindow();

        if (activeWindow == IntPtr.Zero)
        {
            Debug.WriteLine($"Active window is ZERO: {(int)activeWindow}");
            return;
        }

        var stringBuilder = new StringBuilder(1024);
        User32.GetWindowText(activeWindow, stringBuilder, stringBuilder.Capacity);

        WindowPlacement windowsPlacement = default;
        User32.GetWindowPlacement(activeWindow, ref windowsPlacement);

        WindowInfo windowInfo = default;
        User32.GetWindowInfo(activeWindow, ref windowInfo);

        Debug.WriteLine($"Active window ({(int)activeWindow:X}): {stringBuilder}");
        Debug.Indent();
        Debug.WriteLine(windowInfo.PrintDebug());
        Debug.WriteLine($"Maximised: {(windowInfo.dwStyle & WindowStyles.WS_MAXIMIZE) > 0}");
        Debug.Unindent();

        // Get monitor from Window (if window is not in monitor, can return nearest or default based on flag)
        var monitor = User32.MonitorFromWindow(activeWindow, 1);
        Debug.WriteLine($"MonitorFromWindow found window: 0x{monitor:X}");

        // Get monitor coordinates
        var monitorInfo = new MonitorInfo();
        var getMonitorInfoSuccess = User32.GetMonitorInfo(monitor, ref monitorInfo);

        if (!getMonitorInfoSuccess)
        {
            Debug.WriteLine($"Failed to get monitor info for monitor 0x{monitor:X}");
            return;
        }

        Debug.WriteLine($"MonitorInfo found window: 0x{monitor:X}");
        Debug.Indent();
        Debug.WriteLine(monitorInfo.PrintDebug());
        Debug.Unindent();

        // Get window size from rect
        var windowWidth = windowInfo.rcWindow.Right - windowInfo.rcWindow.Left;
        var windowHeight = windowInfo.rcWindow.Bottom - windowInfo.rcWindow.Top;

        var screenHeight = monitorInfo.WorkArea.Bottom - monitorInfo.WorkArea.Top;
        var screenWidth = monitorInfo.WorkArea.Right - monitorInfo.WorkArea.Left;

        // Determine centered position based on screen size and window size
        var newX = monitorInfo.WorkArea.Left + (screenWidth / 2) - (windowWidth / 2);
        var newY = monitorInfo.WorkArea.Top + (screenHeight / 2) - (windowHeight / 2);

        // Set new window position
        Debug.WriteLine($"New window positions is Left:{newX}, Top: {newY}");
        User32.SetWindowPos(
            activeWindow,
            IntPtr.Zero,
            newX,
            newY,
            windowInfo.rcWindow.Width,
            windowInfo.rcWindow.Height,
            1
        );
    }

    public void BindShortcut()
    {
        if (!_bound)
        {
            _keyHook.KeyPressed += KeyHookOnKeyPressed;
            _keyHook.KeyDown += HandleIfShortcut;
            _keyHook.KeyUp += HandleIfShortcut;
        }

        _bound = true;
    }
}
