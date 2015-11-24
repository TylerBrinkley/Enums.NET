using System;

namespace EnumsNET
{
    public interface IEnumMemberInfo
    {
        Attribute[] Attributes { get; }
        string Description { get; }
        string EnumMemberValue { get; }
        string Name { get; }
        object UnderlyingValue { get; }
        object Value { get; }

        string AsString();
        string AsString(string format);
        string AsString(params EnumFormat[] formats);
        string Format(string format);
        string Format(params EnumFormat[] formats);
        TAttribute GetAttribute<TAttribute>() where TAttribute : Attribute;
        TAttribute[] GetAttributes<TAttribute>() where TAttribute : Attribute;
        TResult GetAttributeSelect<TAttribute, TResult>(Func<TAttribute, TResult> selector, TResult defaultValue = default(TResult)) where TAttribute : Attribute;
        string GetDescriptionOrName();
        string GetDescriptionOrName(Func<string, string> nameFormatter);
        bool HasAttribute<TAttribute>() where TAttribute : Attribute;
        byte ToByte();
        short ToInt16();
        int ToInt32();
        long ToInt64();
        sbyte ToSByte();
        string ToString();
        string ToString(string format);
        string ToString(params EnumFormat[] formats);
        ushort ToUInt16();
        uint ToUInt32();
        ulong ToUInt64();
        bool TryGetAttributeSelect<TAttribute, TResult>(Func<TAttribute, TResult> selector, out TResult result) where TAttribute : Attribute;
    }
}