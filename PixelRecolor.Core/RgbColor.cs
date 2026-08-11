namespace PixelRecolor.Core;

public readonly record struct RgbColor(
    byte R,
    byte G,
    byte B,
    byte A = 255);