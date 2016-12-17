Tier I
```diff
namespace System {
    public class Enum {
+       [Obsolete("Use HasAllFlags or HasAnyFlags instead")]
+       [EditorBrowsable(EditorBrowsableState.Never)]
        public bool HasFlag(Enum flag);

        // Generic versions of existing methods
+       public static Type GetUnderlyingType<TEnum>() where TEnum : struct, Enum;
+       public static IEnumerable<string> GetNames<TEnum>() where TEnum : struct, Enum;
+       public static IEnumerable<TEnum> GetValues<TEnum>() where TEnum : struct, Enum;
+       public static TEnum ToObject<TEnum>(object value) where TEnum : struct, Enum;
+       public static TEnum ToObject<TEnum>(sbyte value) where TEnum : struct, Enum;
+       public static TEnum ToObject<TEnum>(byte value) where TEnum : struct, Enum;
+       public static TEnum ToObject<TEnum>(short value) where TEnum : struct, Enum;
+       public static TEnum ToObject<TEnum>(ushort value) where TEnum : struct, Enum;
+       public static TEnum ToObject<TEnum>(int value) where TEnum : struct, Enum;
+       public static TEnum ToObject<TEnum>(uint value) where TEnum : struct, Enum;
+       public static TEnum ToObject<TEnum>(long value) where TEnum : struct, Enum;
+       public static TEnum ToObject<TEnum>(ulong value) where TEnum : struct, Enum;
+       public static bool IsDefined<TEnum>(this TEnum value) where TEnum : struct, Enum;
+       public static string Format<TEnum>(TEnum value, string format) where TEnum : struct, Enum;
+       public static sbyte ToSByte<TEnum>(TEnum value) where TEnum : struct, Enum;
+       public static byte ToByte<TEnum>(TEnum value) where TEnum : struct, Enum;
+       public static short ToInt16<TEnum>(TEnum value) where TEnum : struct, Enum;
+       public static ushort ToUInt16<TEnum>(TEnum value) where TEnum : struct, Enum;
+       public static int ToInt32<TEnum>(TEnum value) where TEnum : struct, Enum;
+       public static uint ToUInt32<TEnum>(TEnum value) where TEnum : struct, Enum;
+       public static long ToInt64<TEnum>(TEnum value) where TEnum : struct, Enum;
+       public static ulong ToUInt64<TEnum>(TEnum value) where TEnum : struct, Enum;
+       public static bool Equals<TEnum>(TEnum value, TEnum other) where TEnum : struct, Enum;
+       public static int CompareTo<TEnum>(TEnum value, TEnum other) where TEnum : struct, Enum;
+       public static string GetName<TEnum>(this TEnum value) where TEnum : struct, Enum;
+       public static TEnum Parse<TEnum>(string value) where TEnum : struct, Enum;
+       public static TEnum Parse<TEnum>(string value, bool ignoreCase) where TEnum : struct, Enum;
+       public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct, Enum;
+       public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result) where TEnum : struct, Enum;

        // New API
+       public static IEnumerable<EnumMember<TEnum>> GetMembers<TEnum>() where TEnum : struct, Enum;
+       public static bool IsValid<TEnum>(this TEnum value) where TEnum : struct, Enum;
+       public static EnumMember<TEnum> GetMember<TEnum>(this TEnum value) where TEnum : struct, Enum;
+       public static EnumMember<TEnum> GetMember<TEnum>(string name) where TEnum : struct, Enum;
+       public static EnumMember<TEnum> GetMember<TEnum>(string name, bool ignoreCase) where TEnum : struct, Enum;
+       public static TEnum GetAllFlags<TEnum>() where TEnum : struct, Enum;
+       public static bool IsValidFlagCombination<TEnum>(TEnum value) where TEnum : struct, Enum;
+       public static IEnumerable<TEnum> GetFlags<TEnum>(this TEnum value) where TEnum : struct, Enum;
+       public static bool HasAnyFlags<TEnum>(this TEnum value) where TEnum : struct, Enum;
+       public static bool HasAnyFlags<TEnum>(this TEnum value, TEnum flags) where TEnum : struct, Enum;
+       public static bool HasAllFlags<TEnum>(this TEnum value) where TEnum : struct, Enum;
+       public static bool HasAllFlags<TEnum>(this TEnum value, TEnum flags) where TEnum : struct, Enum;
+       public static bool CommonFlags<TEnum>(this TEnum value, TEnum flags) where TEnum : struct, Enum;
+       public static TEnum CombineFlags<TEnum>(this TEnum value, TEnum flags) where TEnum : struct, Enum;
+       public static TEnum CombineFlags<TEnum>(TEnum flag0, TEnum flag1, TEnum flag2) where TEnum : struct, Enum;
+       public static TEnum CombineFlags<TEnum>(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3) where TEnum : struct, Enum;
+       public static TEnum CombineFlags<TEnum>(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4) where TEnum : struct, Enum;
+       public static TEnum CombineFlags<TEnum>(params TEnum[] flags) where TEnum : struct, Enum;
+       public static TEnum RemoveFlags<TEnum>(this TEnum value, TEnum flags) where TEnum : struct, Enum;
    }
+   public abstract class EnumMember : IEquatable<EnumMember>, IConvertible, IFormattable {
+       public object Value { get; }
+       public string Name { get; }
+       public AttributeCollection Attributes { get; }
+       public override string ToString();
+       public string ToString(string format);
+       public sbyte ToSByte();
+       public byte ToByte();
+       public short ToInt16();
+       public ushort ToUInt16();
+       public int ToInt32();
+       public uint ToUInt32();
+       public long ToInt64();
+       public ulong ToUInt64();
+       public override int GetHashCode();
+       public bool Equals(EnumMember other);
+       public override bool Equals(object other);
    }
+   public sealed class EnumMember<TEnum> : EnumMember, IEquatable<EnumMember<TEnum>> where TEnum : struct, Enum {
+       public new TEnum Value { get; }
+       public bool Equals(EnumMember<TEnum> other);
    }
+   public sealed class AttributeCollection : IList<Attribute>, IReadOnlyList<Attribute> {
+       public int Count { get; }
+       public Attribute this[int index] { get; }
+       public bool Has<TAttribute>() where TAttribute : Attribute;
+       public bool Has(Type attributeType);
+       public TAttribute Get<TAttribute>() where TAttribute : Attribute;
+       public Attribute Get(Type enumType);
+       public IEnumerator<Attribute> GetEnumerator();
    }
+   public sealed class PrimaryEnumMemberAttribute : Attribute {
    }
+   public sealed class EnumComparer<TEnum> : IEqualityComparer<TEnum>, IComparer<TEnum>, IEqualityComparer, IComparer where TEnum : struct, Enum {
+       public static EnumComparer<TEnum> Instance { get; }
+       public int Compare(TEnum x, TEnum y);
+       public bool Equals(TEnum x, TEnum y);
+       public int GetHashCode(TEnum obj);
    }
}
```

Tier II
```diff
namespace System {
    public class Enum {
+       public static EnumFormat RegisterCustomEnumFormat(Func<EnumMember, string> enumMemberFormatter);
+       public static int GetMemberCount<TEnum>() where TEnum : struct, Enum;
+       public static bool TryToObject<TEnum>(object value, out TEnum result) where TEnum : struct, Enum;
+       public static bool TryToObject<TEnum>(sbyte value, out TEnum result) where TEnum : struct, Enum;
+       public static bool TryToObject<TEnum>(byte value, out TEnum result) where TEnum : struct, Enum;
+       public static bool TryToObject<TEnum>(short value, out TEnum result) where TEnum : struct, Enum;
+       public static bool TryToObject<TEnum>(ushort value, out TEnum result) where TEnum : struct, Enum;
+       public static bool TryToObject<TEnum>(int value, out TEnum result) where TEnum : struct, Enum;
+       public static bool TryToObject<TEnum>(uint value, out TEnum result) where TEnum : struct, Enum;
+       public static bool TryToObject<TEnum>(long value, out TEnum result) where TEnum : struct, Enum;
+       public static bool TryToObject<TEnum>(ulong value, out TEnum result) where TEnum : struct, Enum;
+       public static string ToString<TEnum>(this TEnum value, EnumFormat format) where TEnum : struct, Enum;
+       public static string ToString<TEnum>(this TEnum value, EnumFormat format0, EnumFormat format1) where TEnum : struct, Enum;
+       public static string ToString<TEnum>(this TEnum value, EnumFormat format0, EnumFormat format1, EnumFormat format2) where TEnum : struct, Enum;
+       public static string ToString<TEnum>(this TEnum value, params EnumFormat[] formats) where TEnum : struct, Enum;
+       public static string Format<TEnum>(TEnum value, params EnumFormat[] formats) where TEnum : struct, Enum;
+       public static object GetUnderlyingValue<TEnum>(TEnum value) where TEnum : struct, Enum;
+       public static EnumMember<TEnum> GetMember<TEnum>(string value, params EnumFormat[] formats) where TEnum : struct, Enum;
+       public static EnumMember<TEnum> GetMember<TEnum>(string value, bool ignoreCase, params EnumFormat[] formats) where TEnum : struct, Enum;
+       public static TEnum Parse<TEnum>(string value, params EnumFormat[] formats) where TEnum : struct, Enum;
+       public static TEnum Parse<TEnum>(string value, bool ignoreCase, params EnumFormat[] formats) where TEnum : struct, Enum;
+       public static bool TryParse<TEnum>(string value, out TEnum result, params EnumFormat[] formats) where TEnum : struct, Enum;
+       public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result, params EnumFormat[] formats) where TEnum : struct, Enum;
+       public static bool IsFlagEnum<TEnum>() where TEnum : struct, Enum;
+       public static IEnumerable<EnumMember<TEnum>> GetFlagMembers<TEnum>(TEnum value) where TEnum : struct, Enum;
+       public static TEnum ToggleFlags<TEnum>(TEnum value) where TEnum : struct, Enum;
+       public static TEnum ToggleFlags<TEnum>(TEnum value, TEnum flags) where TEnum : struct, Enum;
    }
    public abstract class EnumMember {
+       public string ToString(EnumFormat format);
+       public string ToString(EnumFormat format0, EnumFormat format1);
+       public string ToString(EnumFormat format0, EnumFormat format1, EnumFormat format2);
+       public string ToString(params EnumFormat[] formats);
+       public object GetUnderlyingValue();
    }
+   public enum EnumFormat {
+       DecimalValue,
+       HexadecimalValue,
+       Name
    }
    public sealed class AttributeCollection : IList<Attribute>, IReadOnlyList<Attribute> {
+       public IEnumerable<TAttribute> GetAll<TAttribute>() where TAttribute : Attribute;
+       public IEnumerable<Attribute> GetAll(Type enumType);
    }
}
```

Tier III
```diff
namespace System {
    public class Enum {
+       public static int GetMemberCount<TEnum>(EnumMemberSelection selection) where TEnum : struct, Enum;
+       public static IEnumerable<EnumMember<TEnum>> GetMembers<TEnum>(EnumMemberSelection selection) where TEnum : struct, Enum;
+       public static IEnumerable<string> GetNames<TEnum>(EnumMemberSelection selection) where TEnum : struct, Enum;
+       public static IEnumerable<TEnum> GetValues<TEnum>(EnumMemberSelection selection) where TEnum : struct, Enum;
    }
+   public enum EnumMemberSelection {
+       All,
+       Distinct,
+       Flags
    }
+   public interface IEnumValidatorAttribute<TEnum> where TEnum : struct, Enum {
+       bool IsValid(TEnum value);
    }
}
```