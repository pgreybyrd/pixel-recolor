namespace PixelRecolor.Core;

public readonly record struct HsvColor(
    double Hue, //0-360
    double Saturation, //0-1
    double Value); //0-1