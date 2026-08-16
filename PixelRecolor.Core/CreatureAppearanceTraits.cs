namespace PixelRecolor.Core;

public sealed record CreatureAppearanceTraits(
    string Palette,
    List<PatternDefinition> Patterns,
    List<AccessoryDefinition> Accessories,
    List<OverlayDefinition> Effects);