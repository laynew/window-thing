namespace WindowThing;

internal static class AppIcon
{
    public static readonly Icon Icon = GenerateIcon();
    public const int IconSize = 16;

    /// <summary>
    /// Generates a fun little random, symmetrical icon
    /// </summary>
    /// <returns></returns>
    private static Icon GenerateIcon()
    {
        using var bitmap = new Bitmap(IconSize, IconSize);
        var random = new Random();
        for (var x = 0; x < IconSize; x++)
        {
            for (var y = 0; y < IconSize; y++)
            {
                var alpha = 100 + (int)((double)x / 8 * 155);
                var pixelColor = x <= IconSize / 2
                    ? Color.FromArgb(alpha, random.Next(0, 255), random.Next(255), random.Next(255))
                    : bitmap.GetPixel(IconSize / 2 - (x % (IconSize / 2)), y);

                bitmap.SetPixel(x, y, pixelColor);
            }
        }

        return Icon.FromHandle(bitmap.GetHicon());
    }
}
