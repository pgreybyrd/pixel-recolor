namespace PixelRecolor.Core
{
    public readonly record struct RegionId(string Value)
    {
        public override string ToString() => Value;
    }
}
