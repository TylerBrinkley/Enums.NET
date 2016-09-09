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

namespace EnumsNET
{
    internal interface IEnumMember : IConvertible, IFormattable
    {
        string Name { get; }
        IEnumerable<Attribute> Attributes { get; }
        string Description { get; }
        object UnderlyingValue { get; }

        string ToString();
        string ToString(string format);
        string ToString(params EnumFormat[] formatOrder);
        string AsString();
        string AsString(string format);
        string AsString(params EnumFormat[] formatOrder);
        string Format(string format);
        string AsString(EnumFormat format);
        string AsString(EnumFormat format0, EnumFormat format1);
        string AsString(EnumFormat format0, EnumFormat format1, EnumFormat format2);
        string Format(params EnumFormat[] formatOrder);
        string GetDescriptionOrName();
        string GetDescriptionOrName(Func<string, string> nameFormatter);
        bool HasAttribute<TAttribute>() where TAttribute : Attribute;
        TAttribute GetAttribute<TAttribute>() where TAttribute : Attribute;
        TResult GetAttributeSelect<TAttribute, TResult>(Func<TAttribute, TResult> selector, TResult defaultValue = default(TResult)) where TAttribute : Attribute;
        IEnumerable<TAttribute> GetAttributes<TAttribute>() where TAttribute : Attribute;
        byte ToByte();
        short ToInt16();
        int ToInt32();
        long ToInt64();
        sbyte ToSByte();
        ushort ToUInt16();
        uint ToUInt32();
        ulong ToUInt64();
        int GetHashCode();

        bool IsValidFlagCombination();
        bool HasAnyFlags();
        bool HasAllFlags();
    }
}