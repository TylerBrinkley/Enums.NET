namespace EnumsNET.Test
{
    // Non-contiguous example
    public enum NonContiguousEnum : long
    {
        Dog = 100,
        Cat = long.MinValue,
        Chimp = -13,
        Elephant = 54,
        Whale = 51,
        Eagle = long.MaxValue
    }
}
