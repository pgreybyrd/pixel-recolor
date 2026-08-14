namespace PixelRecolor.Core;

public readonly record struct RecolorSettings(
    double Hue,
    double Saturation,
    double Brightness = 1.0);