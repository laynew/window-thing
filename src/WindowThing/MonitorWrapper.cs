namespace WindowThing;

internal class MonitorWrapper
{
    public Position WorkArea { get; }
    public Size Size => WorkArea.Size;

    public MonitorWrapper(Position workArea)
    {
        WorkArea = workArea;
    }
}
