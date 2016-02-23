using System;
using System.ComponentModel;

namespace EnumsNET.Tests.TestEnums
{
    public enum StringFilterOperator
    {
        [StringFilterOperatorType(StringFilterOperatorType.OneValue)]
        Is = 0,
        [StringFilterOperatorType(StringFilterOperatorType.OneValue)]
        IsNot,
        [StringFilterOperatorType(StringFilterOperatorType.OneValue)]
        IsExact,
        [StringFilterOperatorType(StringFilterOperatorType.OneValue)]
        IsNotExact,
        [StringFilterOperatorType(StringFilterOperatorType.OneValue)]
        StartsWith,
        [StringFilterOperatorType(StringFilterOperatorType.OneValue)]
        [Description("Doesn't Start With")]
        DoesntStartWith,
        [StringFilterOperatorType(StringFilterOperatorType.OneValue)]
        Contains,
        [StringFilterOperatorType(StringFilterOperatorType.OneValue)]
        [Description("Doesn't Contain")]
        DoesntContain,
        [StringFilterOperatorType(StringFilterOperatorType.MultipleValues)]
        IsAny,
        [StringFilterOperatorType(StringFilterOperatorType.MultipleValues)]
        IsNotAny,
        [StringFilterOperatorType(StringFilterOperatorType.MultipleValues)]
        IsExactAny,
        [StringFilterOperatorType(StringFilterOperatorType.MultipleValues)]
        IsNotExactAny,
        [StringFilterOperatorType(StringFilterOperatorType.MultipleValues)]
        StartsWithAny,
        [StringFilterOperatorType(StringFilterOperatorType.MultipleValues)]
        [Description("Doesn't Start With Any")]
        DoesntStartWithAny,
        [StringFilterOperatorType(StringFilterOperatorType.MultipleValues)]
        ContainsAny,
        [StringFilterOperatorType(StringFilterOperatorType.MultipleValues)]
        [Description("Doesn't Contain Any")]
        DoesntContainAny
    }

    public enum StringFilterOperatorType
    {
        OneValue = 0,
        MultipleValues
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class StringFilterOperatorTypeAttribute : Attribute
    {
        public StringFilterOperatorType Type { get; }

        public StringFilterOperatorTypeAttribute(StringFilterOperatorType type)
        {
            Type = type;
        }
    }
}
