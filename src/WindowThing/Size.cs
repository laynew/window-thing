namespace WindowThing;

internal readonly struct Size
{
    public readonly int Width;
    public readonly int Height;

    public Size(int width, int height)
    {
        Width = width;
        Height = height;
    }
    public static bool operator ==(Size left, Size right) => left.Equals(right);

    public static bool operator !=(Size left, Size right) => !(left == right);

    public static Size operator *(Size left, double right) => new((int)(left.Width * right), (int)(left.Height * right));

    public override bool Equals(object? obj)
    {
        return obj is Size size && Equals(size);
    }

    public bool Equals(Size other)
    {
        return Width == other.Width && Height == other.Height;
    }

    public override int GetHashCode() => HashCode.Combine(Width, Height);
}
