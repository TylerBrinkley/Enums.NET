namespace EnumsNET.Tests
{
    // Contiguous UInt64 example with values past Int64.MaxValue
    public enum ContiguousUInt64Enum : ulong
    {
        A = long.MaxValue - 2,
        B = A + 1,
        C = A + 2,
        D = A + 3,
        E = A + 4,
        F = A + 5
    }
}
