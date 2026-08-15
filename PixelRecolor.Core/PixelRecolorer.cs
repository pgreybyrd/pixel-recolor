namespace PixelRecolor.Core;

public static class PixelRecolorer
{
    public static RegionId? FindRegion(
        RgbColor maskColor,
        IReadOnlyList<RegionDefinition> regions)
    {
        foreach (var region in regions)
        {
            if (region.MaskColor == maskColor)
                return region.Id;
        }

        return null;
    }

    public static RgbColor RecolorGrayscale(
        RgbColor source,
        double hue,
        double saturation,
        double brightness = 1.0)
    {
        double value =
            Math.Max(
                source.R,
                Math.Max(source.G, source.B))
            / 255.0;

        value =
            Math.Clamp(
                value * brightness,
                0,
                1);

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

    public static RgbColor RecolorGrayscaleMasked(
        RgbColor source,
        double hue,
        double saturation,
        double maskStrength)
    {
        maskStrength =
            Math.Clamp(maskStrength, 0, 1);

        if (maskStrength <= 0)
            return source;

        var recolored =
            RecolorGrayscale(
                source,
                hue,
                saturation);

        return new RgbColor(
            Blend(source.R, recolored.R, maskStrength),
            Blend(source.G, recolored.G, maskStrength),
            Blend(source.B, recolored.B, maskStrength),
            source.A);
    }

    public static RgbColor RecolorRegion(
        RgbColor source,
        RgbColor mask,
        IReadOnlyList<RegionDefinition> regions,
        RegionPalette palette)
    {
        var regionId =
            FindRegion(
                mask,
                regions);

        if (regionId is null)
            return source;

        if (!palette.TryGet(
                regionId.Value,
                out var settings))
        {
            return source;
        }

        return RecolorGrayscale(
            source,
            settings.Hue,
            settings.Saturation,
            settings.Brightness);
    }

    public static RgbColor RecolorChannels(
        RgbColor source,
        RgbColor mask,
        RecolorSettings red,
        RecolorSettings green,
        RecolorSettings blue)
    {
        double redStrength =
            mask.R / 255.0;

        double greenStrength =
            mask.G / 255.0;

        double blueStrength =
            mask.B / 255.0;

        var result = source;

        if (redStrength > 0)
        {
            result =
                RecolorGrayscaleMasked(
                    result,
                    red.Hue,
                    red.Saturation,
                    redStrength);
        }

        if (greenStrength > 0)
        {
            result =
                RecolorGrayscaleMasked(
                    result,
                    green.Hue,
                    green.Saturation,
                    greenStrength);
        }

        if (blueStrength > 0)
        {
            result =
                RecolorGrayscaleMasked(
                    result,
                    blue.Hue,
                    blue.Saturation,
                    blueStrength);
        }

        return result;
    }

    private static byte Blend(
        byte original,
        byte recolored,
        double amount)
    {
        return (byte)Math.Round(
            original +
            (recolored - original) *
            amount);
    }

    public static RgbColor RecolorPattern(
        RgbColor pattern,
        RecolorSettings settings)
    {
        if (pattern.A == 0)
            return pattern;

        return RecolorGrayscale(
            pattern,
            settings.Hue,
            settings.Saturation,
            settings.Brightness);
    }
}