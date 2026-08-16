using System.Text.Json;

namespace PixelRecolor.Core;

public static class CreatureAppearanceLoader
{
    public static CreatureAppearanceDefinition Load(
        string json)
    {
        return JsonSerializer.Deserialize<CreatureAppearanceDefinition>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })
            ?? throw new InvalidOperationException(
                "Could not deserialize creature appearance.");
    }
}