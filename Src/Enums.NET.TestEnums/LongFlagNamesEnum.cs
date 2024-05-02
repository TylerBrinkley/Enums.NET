using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumsNET.Tests.TestEnums;

[Flags]
public enum LongFlagNamesEnum
{
    None = 0,
    FirstReallyLongFlagName = 1,
    SecondReallyLongFlagName = 2,
    ThirdReallyLongFlagName = 4,
    FourthReallyLongFlagName = 8,
    FifthReallyLongFlagName = 16,
    SixthReallyLongFlagName = 32,
    SeventhReallyLongFlagName = 64,
    EighthReallyLongFlagName = 128,
    NinthReallyLongFlagName = 256,
    TenthReallyLongFlagName = 512,
    EleventhReallyLongFlagName = 1024
}
