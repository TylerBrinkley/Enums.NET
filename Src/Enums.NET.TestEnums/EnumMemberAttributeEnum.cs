using System.Runtime.Serialization;

namespace EnumsNET.Tests.TestEnums
{
    public enum EnumMemberAttributeEnum
    {
        [EnumMember(Value = "aye")]
        A,
        [EnumMember(Value = "bee")]
        B,
        [EnumMember(Value = "cee")]
        C,
        [EnumMember(Value = "dee")]
        D
    }
}