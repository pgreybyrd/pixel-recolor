namespace PixelRecolor.Core
{
    public sealed class RegionPalette
    {
        private readonly Dictionary<RegionId, RecolorSettings> _regions =
            new();

        public void Set(
            RegionId region,
            RecolorSettings settings)
        {
            _regions[region] = settings;
        }

        public bool TryGet(
            RegionId region,
            out RecolorSettings settings)
        {
            return _regions.TryGetValue(
                region,
                out settings);
        }
    }
}
