namespace VenetStudio
{
    public static partial class Utility
    {
        public static int WrapAround(this int v, int delta, int min, int max)
        {
            int mod = max + 1 - min;
            v += delta - min;
            v += (1 - v / mod) * mod;
            return v % mod + min;
        }
    }
}