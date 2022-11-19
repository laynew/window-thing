namespace WindowThing;

internal class WindowContext
{
    public bool IsWindowMaximized => Window.IsMaximized;
    public WindowWrapper Window { get; }
    public MonitorWrapper Monitor { get; }

    public WindowContext(WindowWrapper window, MonitorWrapper monitor)
    {
        Window = window;
        Monitor = monitor;
    }
}
