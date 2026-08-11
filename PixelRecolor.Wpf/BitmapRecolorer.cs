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
}