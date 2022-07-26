using Rollout.Lib.Interfaces;

namespace Rollout.Lib.Implementations;

internal class UniformStringToDecimalProvider : IStringToDecimalProvider
{
    public decimal Transform(string value)
    {
        return SetTo32BitFnv1AHash(value) % 100;
    }

    // this code was adapted from https://stackoverflow.com/a/12272613/5184059
    private uint SetTo32BitFnv1AHash(string toHash)
    {
        var bytesToHash = toHash.ToCharArray().Select(Convert.ToByte);

        //this is the actual hash function; very simple
        var hash = FnvConstants.FnvOffset32;

        foreach (var chunk in bytesToHash)
        {
            hash ^= chunk;
            hash *= FnvConstants.FnvPrime32;
        }

        return hash;
    }

    private static class FnvConstants
    {
        public const uint FnvPrime32 = 16777619;
        public const uint FnvOffset32 = 2166136261;
    }
}
