namespace WindowThing;

internal class WindowWrapper
{
    public IntPtr Handle { get; }
    public Position Position { get; }
    public bool IsMaximized { get; }
    public Size Size => Position.Size;

    public WindowWrapper(IntPtr handle, Position position, bool isMaximized)
    {
        Handle = handle;
        Position = position;
        IsMaximized = isMaximized;
    }

    public WindowWrapper WithSize(Size newSize)
    {
        return new WindowWrapper(
            Handle,
            new Position(Position.Origin, newSize),
            IsMaximized);
    }
}
