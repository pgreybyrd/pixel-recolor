namespace PixelRecolor.Core;

public sealed record CreatureAppearanceDefinition(
    string Id,
    string Palette,
    List<PatternDefinition> Patterns,
    List<AccessoryDefinition> Accessories,
    List<OverlayDefinition> Effects);