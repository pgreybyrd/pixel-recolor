using System.Text.Json;

namespace PixelRecolor.Core;

public static class RegionPaletteLoader
{
    public static RegionPalette Load(
        string json)
    {
        var data =
            JsonSerializer.Deserialize<RegionPaletteData>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })
            ?? throw new InvalidOperationException(
                "Could not deserialize region palette.");

        var palette =
            new RegionPalette();

        foreach (var region in data.Regions)
        {
            palette.Set(
                new RegionId(region.Key),
                new RecolorSettings(
                    region.Value.Hue,
                    region.Value.Saturation,
                    region.Value.Brightness));
        }

        return palette;
    }
}

internal sealed class RegionPaletteData
{
    public string Name { get; set; } = "";

    public Dictionary<string, RegionPaletteEntry>
        Regions
    { get; set; } = new();
}

internal sealed class RegionPaletteEntry
{
    public double Hue { get; set; }

    public double Saturation { get; set; }

    public double Brightness { get; set; } = 1.0;
}