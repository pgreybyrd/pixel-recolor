using PixelRecolor.Core;
using System.Windows.Media.Imaging;

namespace PixelRecolor.Wpf
{
    public static class CreatureAppearanceRenderer
    {
        public static BitmapSource Build(
            BitmapSource source,
            BitmapSource regionMask,
            IReadOnlyList<RegionDefinition> regions,
            RegionPalette palette,
            CreatureAppearanceTraits traits,
            Func<string, BitmapSource> patternLoader,
            Func<string, BitmapSource> accessoryLoader,
            Func<string, BitmapSource> effectLoader)
        {
            var result =
                BitmapRecolorer.RecolorRegions(
                    source,
                    regionMask,
                    regions,
                    palette);

            foreach (var pattern in traits.Patterns)
            {
                result =
                    ApplyOverlay(
                        result,
                        patternLoader(pattern.Id),
                        pattern.Recolor);
            }

            foreach (var accessory in traits.Accessories)
            {
                result =
                    ApplyOverlay(
                        result,
                        accessoryLoader(accessory.Id),
                        accessory.Recolor);
            }

            foreach (var effect in traits.Effects)
            {
                result =
                    ApplyOverlay(
                        result,
                        effectLoader(effect.Id),
                        effect.Recolor);
            }

            return result;
        }

        private static BitmapSource ApplyOverlay(
            BitmapSource current,
            BitmapSource source,
            RecolorSettings? recolor)
        {
            BitmapSource overlay = source;

            if (recolor is not null)
            {
                overlay =
                    BitmapRecolorer.RecolorPattern(
                        source,
                        recolor.Value);
            }

            return BitmapRecolorer.Composite(
                current,
                overlay);
        }
    }
}
