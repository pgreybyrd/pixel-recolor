using PixelRecolor.Core;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PixelRecolor.Wpf;

public static class BitmapRecolorer
{
    public static BitmapSource RecolorGrayscale(
        BitmapSource source,
        double hue,
        double saturation)
    {
        var formatted = new FormatConvertedBitmap(
            source,
            PixelFormats.Bgra32,
            null,
            0);

        int width = formatted.PixelWidth;
        int height = formatted.PixelHeight;
        int stride = width * 4;

        byte[] pixels =
            new byte[height * stride];

        formatted.CopyPixels(
            pixels,
            stride,
            0);

        for (int i = 0; i < pixels.Length; i += 4)
        {
            byte b = pixels[i];
            byte g = pixels[i + 1];
            byte r = pixels[i + 2];
            byte a = pixels[i + 3];

            if (a == 0)
                continue;

            var recolored =
                PixelRecolorer.RecolorGrayscale(
                    new RgbColor(r, g, b, a),
                    hue,
                    saturation);

            pixels[i] = recolored.B;
            pixels[i + 1] = recolored.G;
            pixels[i + 2] = recolored.R;
            pixels[i + 3] = recolored.A;
        }

        var result =
            BitmapSource.Create(
                width,
                height,
                formatted.DpiX,
                formatted.DpiY,
                PixelFormats.Bgra32,
                null,
                pixels,
                stride);

        result.Freeze();

        return result;
    }

    public static BitmapSource RecolorGrayscale(
        BitmapSource source,
        BitmapSource mask,
        double hue,
        double saturation)
    {
        var formattedSource =
            new FormatConvertedBitmap(
                source,
                PixelFormats.Bgra32,
                null,
                0);

        var formattedMask =
            new FormatConvertedBitmap(
                mask,
                PixelFormats.Bgra32,
                null,
                0);

        if (formattedSource.PixelWidth != formattedMask.PixelWidth ||
            formattedSource.PixelHeight != formattedMask.PixelHeight)
        {
            throw new ArgumentException(
                "Source image and mask must have matching dimensions.");
        }

        int width =
            formattedSource.PixelWidth;

        int height =
            formattedSource.PixelHeight;

        int stride =
            width * 4;

        byte[] pixels =
            new byte[height * stride];

        byte[] maskPixels =
            new byte[height * stride];

        formattedSource.CopyPixels(
            pixels,
            stride,
            0);

        formattedMask.CopyPixels(
            maskPixels,
            stride,
            0);

        for (int i = 0; i < pixels.Length; i += 4)
        {
            byte b = pixels[i];
            byte g = pixels[i + 1];
            byte r = pixels[i + 2];
            byte a = pixels[i + 3];

            if (a == 0)
                continue;

            // Mask is grayscale, so R/G/B should match.
            // Use red as the mask strength.
            double maskStrength =
                maskPixels[i + 2] / 255.0;

            var recolored =
                PixelRecolorer.RecolorGrayscale(
                    new RgbColor(
                        r,
                        g,
                        b,
                        a),
                    hue,
                    saturation,
                    maskStrength);

            pixels[i] =
                recolored.B;

            pixels[i + 1] =
                recolored.G;

            pixels[i + 2] =
                recolored.R;

            pixels[i + 3] =
                recolored.A;
        }

        var result =
            BitmapSource.Create(
                width,
                height,
                formattedSource.DpiX,
                formattedSource.DpiY,
                PixelFormats.Bgra32,
                null,
                pixels,
                stride);

        result.Freeze();

        return result;
    }

    public static BitmapSource RecolorChannels(
        BitmapSource source,
        BitmapSource mask,
        RecolorSettings red,
        RecolorSettings green,
        RecolorSettings blue)
    {
        var formattedSource =
            new FormatConvertedBitmap(
                source,
                PixelFormats.Bgra32,
                null,
                0);

        var formattedMask =
            new FormatConvertedBitmap(
                mask,
                PixelFormats.Bgra32,
                null,
                0);

        if (formattedSource.PixelWidth != formattedMask.PixelWidth ||
            formattedSource.PixelHeight != formattedMask.PixelHeight)
        {
            throw new ArgumentException(
                "Source image and mask must have matching dimensions.");
        }

        int width = formattedSource.PixelWidth;
        int height = formattedSource.PixelHeight;
        int stride = width * 4;

        byte[] pixels = new byte[height * stride];
        byte[] maskPixels = new byte[height * stride];

        formattedSource.CopyPixels(
            pixels,
            stride,
            0);

        formattedMask.CopyPixels(
            maskPixels,
            stride,
            0);

        for (int i = 0; i < pixels.Length; i += 4)
        {
            byte b = pixels[i];
            byte g = pixels[i + 1];
            byte r = pixels[i + 2];
            byte a = pixels[i + 3];

            if (a == 0)
                continue;

            // WPF BGRA → our normal RGBA representation
            var sourcePixel =
                new RgbColor(
                    r,
                    g,
                    b,
                    a);

            var maskPixel =
                new RgbColor(
                    maskPixels[i + 2], // R
                    maskPixels[i + 1], // G
                    maskPixels[i],     // B
                    maskPixels[i + 3]);

            var recolored =
                PixelRecolorer.RecolorChannels(
                    sourcePixel,
                    maskPixel,
                    red,
                    green,
                    blue);

            pixels[i] = recolored.B;
            pixels[i + 1] = recolored.G;
            pixels[i + 2] = recolored.R;
            pixels[i + 3] = recolored.A;
        }

        var result =
            BitmapSource.Create(
                width,
                height,
                formattedSource.DpiX,
                formattedSource.DpiY,
                PixelFormats.Bgra32,
                null,
                pixels,
                stride);

        result.Freeze();

        return result;
    }
}