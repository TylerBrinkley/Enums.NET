namespace EnumsNET.Tests.TestEnums
{
    [EnumUnderlyingType(typeof(bool))]
    public enum BooleanEnum : byte // Will be replaced with bool
    {
        No//,
        //Yes
    }
}
