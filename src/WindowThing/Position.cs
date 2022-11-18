namespace WindowThing;

internal readonly struct Position
{
    public readonly Point Origin;
    public readonly Size Size;

    public Position(Point origin, Size size)
    {
        Origin = origin;
        Size = size;
    }

    public static bool operator ==(Position left, Position right) => left.Equals(right);

    public static bool operator !=(Position left, Position right) => !(left == right);

    public override bool Equals(object? obj)
    {
        return obj is Position position && Equals(position);
    }

    private bool Equals(Position other)
    {
        return Origin == other.Origin
               && Size == other.Size;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Origin, Size);
    }
}
