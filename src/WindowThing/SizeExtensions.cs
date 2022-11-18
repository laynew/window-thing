namespace WindowThing;

internal static class SizeExtensions
{
    public static Size MatchProportions(this Size size, Size other)
    {
        var targetAspectRatio = (decimal)other.Width / other.Height;

        var myAspectRatio = (decimal)size.Width / size.Height;

        if (myAspectRatio > targetAspectRatio)
        {
            var newHeight = size.Width / targetAspectRatio;
            return new Size(size.Width, (int)newHeight);
        }

        if (myAspectRatio < targetAspectRatio)
        {
            var newWidth = size.Height * targetAspectRatio;
            return new Size((int)newWidth, size.Height);
        }

        return size;

    }
}
