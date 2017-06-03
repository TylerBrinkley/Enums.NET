namespace EnumsNET.Tests.TestEnums
{
    [EnumUnderlyingType(typeof(char))]
    public enum CharEnum : ushort // Will be replaced with char
    {
        A = 'a',
        B = 'b',
        C = 'c'
    }
}