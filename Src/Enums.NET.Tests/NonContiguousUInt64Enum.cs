namespace EnumsNET.Tests
{
    // Non-Contiguous UInt64 example
    public enum NonContiguousUInt64Enum : ulong
    {
        SaintLouis = ulong.MaxValue,
        Chicago = ulong.MinValue,
        Pittsburg = 10000,
        Cincinnati = 1,
        Milwaukee = 2
    }
}
