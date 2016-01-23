using System;
using System.ComponentModel;

namespace EnumsNET.Test
{
    // Contiguous example
    public enum DateFilterOperator : short
    {
        [DateFilterOperatorType(DateFilterOperatorType.NoParams)]
        Today = 0,
        [DateFilterOperatorType(DateFilterOperatorType.NoParams)]
        CurrentWeek,
        [DateFilterOperatorType(DateFilterOperatorType.NoParams)]
        CurrentMonth,
        [DateFilterOperatorType(DateFilterOperatorType.NoParams)]
        CurrentQuarter,
        [DateFilterOperatorType(DateFilterOperatorType.NoParams)]
        YearToDate,
        [DateFilterOperatorType(DateFilterOperatorType.NoParams)]
        PreviousWeek,
        [DateFilterOperatorType(DateFilterOperatorType.NoParams)]
        PreviousMonth,
        [DateFilterOperatorType(DateFilterOperatorType.NoParams)]
        PreviousQuarter,
        [DateFilterOperatorType(DateFilterOperatorType.NoParams)]
        PreviousYear,
        [DateFilterOperatorType(DateFilterOperatorType.NoParams)]
        NextWeek,
        [DateFilterOperatorType(DateFilterOperatorType.NoParams)]
        NextMonth,
        [DateFilterOperatorType(DateFilterOperatorType.NoParams)]
        NextYear,
        [DateFilterOperatorType(DateFilterOperatorType.NoParams)]
        [Description("Empty Date")]
        EmptyDateField,
        [DateFilterOperatorType(DateFilterOperatorType.NoParams)]
        [Description("Non-Empty Date")]
        NonEmptyDateField,
        [DateFilterOperatorType(DateFilterOperatorType.OneDate)]
        Is,
        [DateFilterOperatorType(DateFilterOperatorType.OneDate)]
        IsNot,
        [DateFilterOperatorType(DateFilterOperatorType.OneDate)]
        Before,
        [DateFilterOperatorType(DateFilterOperatorType.OneDate)]
        OnOrBefore,
        [DateFilterOperatorType(DateFilterOperatorType.OneDate)]
        After,
        [DateFilterOperatorType(DateFilterOperatorType.OneDate)]
        OnOrAfter,
        [DateFilterOperatorType(DateFilterOperatorType.TwoDates)]
        DateBetween,
        [DateFilterOperatorType(DateFilterOperatorType.TwoDates)]
        DateNotBetween,
        [DateFilterOperatorType(DateFilterOperatorType.NumberOfDays)]
        IsNumberOfDays,
        [DateFilterOperatorType(DateFilterOperatorType.NumberOfDays)]
        IsNotNumberOfDays,
        [DateFilterOperatorType(DateFilterOperatorType.NumberOfDays)]
        BeforeNumberOfDays,
        [DateFilterOperatorType(DateFilterOperatorType.NumberOfDays)]
        OnOrBeforeNumberOfDays,
        [DateFilterOperatorType(DateFilterOperatorType.NumberOfDays)]
        AfterNumberOfDays,
        [DateFilterOperatorType(DateFilterOperatorType.NumberOfDays)]
        OnOrAfterNumberOfDays,
        [DateFilterOperatorType(DateFilterOperatorType.NumberOfDays)]
        LastNumberOfDays,
        [DateFilterOperatorType(DateFilterOperatorType.NumberOfDays)]
        NextNumberOfDays,
        [DateFilterOperatorType(DateFilterOperatorType.NumberOfDays)]
        IsNumberOfBusinessDays,
        [DateFilterOperatorType(DateFilterOperatorType.NumberOfDays)]
        IsNotNumberOfBusinessDays,
        [DateFilterOperatorType(DateFilterOperatorType.NumberOfDays)]
        BeforeNumberOfBusinessDays,
        [DateFilterOperatorType(DateFilterOperatorType.NumberOfDays)]
        OnOrBeforeNumberOfBusinessDays,
        [DateFilterOperatorType(DateFilterOperatorType.NumberOfDays)]
        AfterNumberOfBusinessDays,
        [DateFilterOperatorType(DateFilterOperatorType.NumberOfDays)]
        OnOrAfterNumberOfBusinessDays,
        [DateFilterOperatorType(DateFilterOperatorType.NumberOfDays)]
        LastNumberOfBusinessDays,
        [DateFilterOperatorType(DateFilterOperatorType.NumberOfDays)]
        NextNumberOfBusinessDays
    }

    public enum DateFilterOperatorType
    {
        NoParams = 0,
        OneDate,
        TwoDates,
        NumberOfDays
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class DateFilterOperatorTypeAttribute : Attribute
    {
        public DateFilterOperatorType Type { get; }

        public DateFilterOperatorTypeAttribute(DateFilterOperatorType type)
        {
            Type = type;
        }
    }
}
