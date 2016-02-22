using System;

namespace EnumsNET.Tests
{
    // UInt64 Flag example
    [Flags]
    public enum UInt64FlagEnum : ulong
    {
        Stationary = 0,
        Walks = 1,
        Runs = 2,
        Flies = 4,
        // Note missing 8
        Hops = 16,
        Slithers = 32,
        Swims = 64
    }
}
