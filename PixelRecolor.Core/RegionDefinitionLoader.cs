using System.Text.Json;

namespace PixelRecolor.Core;

public static class RegionDefinitionLoader
{
    public static List<RegionDefinition> Load(
        string json)
    {
        var data =
            JsonSerializer.Deserialize<RegionDefinitionData>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })
            ?? throw new InvalidOperationException(
                "Could not deserialize region definitions.");

        return data.Regions
            .Select(region =>
                new RegionDefinition(
                    new RegionId(
                        region.Key),
                    new RgbColor(
                        region.Value.R,
                        region.Value.G,
                        region.Value.B)))
            .ToList();
    }
}

internal sealed class RegionDefinitionData
{
    public Dictionary<string, RegionColorData> Regions
    {
        get;
        set;
    } = new();
}

internal sealed class RegionColorData
{
    public byte R { get; set; }

    public byte G { get; set; }

    public byte B { get; set; }
}