using System;
using System.ComponentModel;

namespace EnumsNET.Tests.TestEnums;

// Flag example
[Flags]
public enum ColorFlagEnum : sbyte
{
    Black = 0,
    Red = 1,
    Green = 2,
    //Yellow = Red | Green,
    Blue = 4,
    //Magenta = Red | Blue,
    //Cyan = Green | Blue,
    //White = Red | Green | Blue,
    [Description("Ultra-Violet")]
    UltraViolet = 8,
    All = 15
}
