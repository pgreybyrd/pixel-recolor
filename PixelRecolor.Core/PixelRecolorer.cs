namespace PixelRecolor.Core;

public static class PixelRecolorer
{
    public static RgbColor RecolorGrayscale(
        RgbColor source,
        double hue,
        double saturation)
    {
        double value =
            Math.Max(
                source.R,
                Math.Max(source.G, source.B))
            / 255.0;

        var recolored =
            HsvToRgb(
                hue,
                saturation,
                value);

        return recolored with
        {
            A = source.A
        };
    }

    private static RgbColor HsvToRgb(
        double hue,
        double saturation,
        double value)
    {
        hue =
            ((hue % 360) + 360) % 360;

        saturation =
            Math.Clamp(saturation, 0, 1);

        value =
            Math.Clamp(value, 0, 1);

        double chroma =
            value * saturation;

        double x =
            chroma *
            (1 -
             Math.Abs(
                 (hue / 60.0) % 2 - 1));

        double m =
            value - chroma;

        double r1;
        double g1;
        double b1;

        if (hue < 60)
            (r1, g1, b1) = (chroma, x, 0);
        else if (hue < 120)
            (r1, g1, b1) = (x, chroma, 0);
        else if (hue < 180)
            (r1, g1, b1) = (0, chroma, x);
        else if (hue < 240)
            (r1, g1, b1) = (0, x, chroma);
        else if (hue < 300)
            (r1, g1, b1) = (x, 0, chroma);
        else
            (r1, g1, b1) = (chroma, 0, x);

        return new RgbColor(
            ToByte(r1 + m),
            ToByte(g1 + m),
            ToByte(b1 + m));
    }

    private static byte ToByte(double value)
    {
        return (byte)Math.Round(
            Math.Clamp(value, 0, 1) * 255);
    }
}