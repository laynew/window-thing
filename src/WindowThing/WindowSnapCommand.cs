using System.Diagnostics;
using System.Text;
using WindowThing.Diagnostics;
using WindowThing.InputHandling;
using WindowThing.Win32Interop;

namespace WindowThing;

internal sealed class WindowSnapCommand : IDisposable
{
    private bool _bound;
    private readonly KeyHook _keyHook;

    public WindowSnapCommand(KeyHook keyHook)
    {
        _keyHook = keyHook;
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

    public void UnbindShortcut()
    {
        if (_bound)
        {
            _keyHook.KeyPressed -= KeyHookOnKeyPressed;
            _keyHook.KeyDown -= HandleIfShortcut;
            _keyHook.KeyUp -= HandleIfShortcut;
        }

        _bound = false;
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
        var windowContextResult = GetWindowContext();

        if (!windowContextResult.IsSuccess)
        {
            Debug.WriteLine(windowContextResult.Error);
            return;
        }

        var windowContext = windowContextResult.Value!;

        if (windowContext.IsWindowMaximized)
        {
            Debug.WriteLine($"Active window is maximized. No further action being taken in {nameof(CenterActiveWindow)}");
            return;
        }

        var nextPosition = GetNextWindowPosition(windowContext);

        SetWindowPosition(windowContext, nextPosition);
    }

    private Result<WindowContext> GetWindowContext()
    {
        var activeWindow = User32.GetForegroundWindow();

        if (activeWindow == IntPtr.Zero)
        {
            Debug.WriteLine($"Active window is ZERO: {(int)activeWindow}");
            return Result<WindowContext>.Fail("Could not find active window");
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
            return Result<WindowContext>.Fail($"Failed to get monitor info for monitor 0x{monitor:X}");
        }

        var windowWrapper = new WindowWrapper(
            activeWindow,
            new Position(
                new Point(windowInfo.rcWindow.Left, windowInfo.rcWindow.Top),
                new Size(windowInfo.rcWindow.Width, windowInfo.rcWindow.Height)
            ),
            windowsPlacement.ShowCmd == ShowWindowEnum.Maximize
        );

        var monitorWrapper = new MonitorWrapper(
            new Position(
                new Point(monitorInfo.WorkArea.Left, monitorInfo.WorkArea.Top),
                new Size(monitorInfo.WorkArea.Width, monitorInfo.WorkArea.Height)
            )
        );

        return Result.Success(new WindowContext(windowWrapper, monitorWrapper));
    }

    private Position GetNextWindowPosition(WindowContext windowContext)
    {
        var positionCentered = windowContext.Window.CenterIn(windowContext.Monitor);

        if (positionCentered != windowContext.Window.Position)
        {
            return positionCentered;
        }

        var sizeProportional = windowContext.Window.Size.MatchProportions(windowContext.Monitor.Size);

        if (windowContext.Window.Size != sizeProportional)
        {
            return windowContext.Window.WithSize(sizeProportional).CenterIn(windowContext.Monitor);
        }

        var sizes = new [] { 0.5, 0.75, 0.9 };

        var size = sizes
            .Select(size => windowContext.Monitor.Size * size)
            .FirstOrDefault(size => windowContext.Window.Size.Width < size.Width);
            
        var nextSize = size != default ? size : windowContext.Monitor.Size * sizes[0];

        return windowContext.Window.WithSize(nextSize).CenterIn(windowContext.Monitor);
    }

    private void SetWindowPosition(WindowContext windowContext, Position nextPosition)
    {
        User32.SetWindowPos(
            windowContext.Window.Handle,
            IntPtr.Zero,
            nextPosition.Origin.X,
            nextPosition.Origin.Y,
            nextPosition.Size.Width,
            nextPosition.Size.Height,
            0
        );
    }

    public void Dispose()
    {
        UnbindShortcut();
    }
}
