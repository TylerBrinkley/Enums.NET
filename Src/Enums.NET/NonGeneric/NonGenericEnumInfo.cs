#region License
// Copyright (c) 2016 Tyler Brinkley
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using EnumsNET.Numerics;

namespace EnumsNET.NonGeneric
{
    internal sealed class NonGenericEnumInfo<TEnum, TInt, TIntProvider> : EnumInfo<TEnum, TInt, TIntProvider>, IEnumInfo
        where TEnum : struct
        where TInt : struct, IFormattable, IConvertible, IComparable<TInt>, IEquatable<TInt>
        where TIntProvider : struct, INumericProvider<TInt>
    {
        private static TEnum ToEnum(object value) => value is TEnum || value is TEnum? ? (TEnum)value : EnumInfo<TEnum, TInt, TIntProvider>.ToEnum(Cache.ToObject(value, false));

        object IEnumInfo.AllFlags => AllFlags;

        string IEnumInfo.AsString(object value) => AsString(ToEnum(value));

        string IEnumInfo.AsString(object value, string format) => AsString(ToEnum(value), format);

        string IEnumInfo.AsString(object value, EnumFormat[] formatOrder) => AsString(ToEnum(value), formatOrder);

        object IEnumInfo.ExcludeFlags(object value, object otherFlags) => ExcludeFlags(ToEnum(value), ToEnum(otherFlags));

        object IEnumInfo.CommonFlags(object value, object otherFlags) => CommonFlags(ToEnum(value), ToEnum(otherFlags));

        int IEnumInfo.CompareTo(object value, object other) => CompareTo(ToEnum(value), ToEnum(other));

        bool IEnumInfo.Equals(object value, object other) => Equals(ToEnum(value), ToEnum(other));

        string IEnumInfo.Format(object value, string format) => Format(ToEnum(value), format);

        string IEnumInfo.Format(object value, EnumFormat[] formatOrder) => Format(ToEnum(value), formatOrder);

        string IEnumInfo.Format(object value, EnumFormat format) => AsString(ToEnum(value), format);

        string IEnumInfo.Format(object value, EnumFormat format0, EnumFormat format1) => AsString(ToEnum(value), format0, format1);

        string IEnumInfo.Format(object value, EnumFormat format0, EnumFormat format1, EnumFormat format2) => AsString(ToEnum(value), format0, format1, format2);

        string IEnumInfo.FormatFlags(object value, string delimiter, EnumFormat[] formatOrder) => FormatFlags(ToEnum(value), delimiter, formatOrder);

        IEnumerable<Attribute> IEnumInfo.GetAttributes(object value) => GetAttributes(ToEnum(value));

        string IEnumInfo.GetDescription(object value) => GetDescription(ToEnum(value));

        string IEnumInfo.GetDescriptionOrName(object value) => GetDescriptionOrName(ToEnum(value));

        string IEnumInfo.GetDescriptionOrName(object value, Func<string, string> nameFormatter) => GetDescriptionOrName(ToEnum(value), nameFormatter);

        EnumMember IEnumInfo.GetEnumMember(object value) => GetEnumMember(ToEnum(value));

        EnumMember IEnumInfo.GetEnumMember(string name, bool ignoreCase) => GetEnumMember(name, ignoreCase);

        IEnumerable<EnumMember> IEnumInfo.GetEnumMembers(bool uniqueValued) => GetEnumMembers(uniqueValued)
#if NET20 || NET35
            .Select(member => (EnumMember)member)
#endif
            ;

        IEnumerable<object> IEnumInfo.GetFlags(object value) => GetFlags(ToEnum(value)).Select(flag => (object)flag);

        IEnumerable<EnumMember> IEnumInfo.GetFlagMembers(object value) => GetFlagMembers(ToEnum(value))
#if NET20 || NET35
            .Select(flag => (EnumMember)flag)
#endif
            ;

        string IEnumInfo.GetName(object value) => GetName(ToEnum(value));

        object IEnumInfo.GetUnderlyingValue(object value) => GetUnderlyingValue(ToEnum(value));

        IEnumerable<object> IEnumInfo.GetValues(bool uniqueValued) => GetValues(uniqueValued).Select(value => (object)value);

        bool IEnumInfo.HasAllFlags(object value) => HasAllFlags(ToEnum(value));

        bool IEnumInfo.HasAllFlags(object value, object otherFlags) => HasAllFlags(ToEnum(value), ToEnum(otherFlags));

        bool IEnumInfo.HasAnyFlags(object value) => HasAnyFlags(ToEnum(value));

        bool IEnumInfo.HasAnyFlags(object value, object otherFlags) => HasAnyFlags(ToEnum(value), ToEnum(otherFlags));

        bool IEnumInfo.IsValidFlagCombination(object value) => IsValidFlagCombination(ToEnum(value));

        object IEnumInfo.Parse(string value, bool ignoreCase, EnumFormat[] parseFormatOrder) => Parse(value, ignoreCase, parseFormatOrder);

        object IEnumInfo.ParseFlags(string value, bool ignoreCase, string delimiter, EnumFormat[] parseFormatOrder) => ParseFlags(value, ignoreCase, delimiter, parseFormatOrder);

        EnumFormat IEnumInfo.RegisterCustomEnumFormat(Func<EnumMember, string> formatter) => RegisterCustomEnumFormat(
#if NET20 || NET35
            member => formatter(member));
#else
            formatter);
#endif

        object IEnumInfo.CombineFlags(IEnumerable<object> flags) => CombineFlags(flags.Select(flag => ToEnum(flag)));

        object IEnumInfo.CombineFlags(object value, object otherFlags) => CombineFlags(ToEnum(value), ToEnum(otherFlags));

        byte IEnumInfo.ToByte(object value) => ToByte(ToEnum(value));

        object IEnumInfo.ToggleFlags(object value) => ToggleFlags(ToEnum(value));

        object IEnumInfo.ToggleFlags(object value, object otherFlags) => ToggleFlags(ToEnum(value), ToEnum(otherFlags));

        short IEnumInfo.ToInt16(object value) => ToInt16(ToEnum(value));

        int IEnumInfo.ToInt32(object value) => ToInt32(ToEnum(value));

        long IEnumInfo.ToInt64(object value) => ToInt64(ToEnum(value));

        object IEnumInfo.ToObject(ulong value, bool validate) => ToObject(value, validate);

        object IEnumInfo.ToObject(object value, bool validate) => ToObject(value, validate);

        object IEnumInfo.ToObject(long value, bool validate) => ToObject(value, validate);

        sbyte IEnumInfo.ToSByte(object value) => ToSByte(ToEnum(value));

        ushort IEnumInfo.ToUInt16(object value) => ToUInt16(ToEnum(value));

        uint IEnumInfo.ToUInt32(object value) => ToUInt32(ToEnum(value));

        ulong IEnumInfo.ToUInt64(object value) => ToUInt64(ToEnum(value));

        bool IEnumInfo.TryParse(string value, bool ignoreCase, out object result, EnumFormat[] parseFormatOrder)
        {
            TEnum resultAsTEnum;
            var success = TryParse(value, ignoreCase, out resultAsTEnum, parseFormatOrder);
            result = resultAsTEnum;
            return success;
        }

        bool IEnumInfo.TryParseFlags(string value, bool ignoreCase, string delimiter, out object result, EnumFormat[] parseFormatOrder)
        {
            TEnum resultAsTEnum;
            var success = TryParseFlags(value, ignoreCase, delimiter, out resultAsTEnum, parseFormatOrder);
            result = resultAsTEnum;
            return success;
        }

        bool IEnumInfo.TryToObject(ulong value, out object result, bool validate)
        {
            TEnum resultAsTEnum;
            var success = TryToObject(value, out resultAsTEnum, validate);
            result = resultAsTEnum;
            return success;
        }

        bool IEnumInfo.TryToObject(object value, out object result, bool validate)
        {
            TEnum resultAsTEnum;
            var success = TryToObject(value, out resultAsTEnum, validate);
            result = resultAsTEnum;
            return success;
        }

        bool IEnumInfo.TryToObject(long value, out object result, bool validate)
        {
            TEnum resultAsTEnum;
            var success = TryToObject(value, out resultAsTEnum, validate);
            result = resultAsTEnum;
            return success;
        }

        object IEnumInfo.Validate(object value, string paramName) => Validate(ToEnum(value), paramName);
    }
}
