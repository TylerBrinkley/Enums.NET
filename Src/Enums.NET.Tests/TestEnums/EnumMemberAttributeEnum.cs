using System.Runtime.Serialization;

namespace EnumsNET.Tests.TestEnums
{
    public enum EnumMemberAttributeEnum
    {
        [EnumMember(Value = "a")]
        A,
        [EnumMember(Value = "b")]
        B,
        [EnumMember(Value = "c")]
        C,
        [EnumMember(Value = "d")]
        D
    }
}
