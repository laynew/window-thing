namespace WindowThing;

internal class WindowWrapper
{
    public IntPtr Handle { get; }
    public Position Position { get; }
    public Size Size => Position.Size;

    public WindowWrapper(IntPtr handle, Position position)
    {
        Handle = handle;
        Position = position;
    }

    public WindowWrapper WithSize(Size newSize)
    {
        return new WindowWrapper(
            Handle,
            new Position(Position.Origin, newSize));
    }
}
