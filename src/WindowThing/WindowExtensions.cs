namespace WindowThing;

internal static class WindowExtensions
{
    public static Position CenterIn(this WindowWrapper window, MonitorWrapper monitor)
    {
        var newX = monitor.WorkArea.Origin.X + (monitor.WorkArea.Size.Width / 2) - (window.Size.Width / 2);
        var newY = monitor.WorkArea.Origin.Y + (monitor.WorkArea.Size.Height / 2) - (window.Size.Height / 2);

        return new Position(new Point(newX, newY), window.Size);
    }
}
