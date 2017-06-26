using System;
using System.ComponentModel;

namespace EnumsNET.Tests.TestEnums
{
    public enum MultipleAttributeEnum
    {
        None,
        [Option("Mono")]
        [Description("One")]
        Single,
        [Option("Poly")]
        [Option("Plural")]
        [Description("Many")]
        Multi
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class OptionAttribute : Attribute
    {
        public string Value { get; }

        public OptionAttribute(string value)
        {
            Value = value;
        }
    }
}
