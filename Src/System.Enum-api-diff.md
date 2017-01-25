# Enum Improvements
Enums are essential commonly used types but have several areas in need of improvement. For each non-generic `Enum` method there should be an equivalent generic version. There should be better support for flag enums, enums with the `FlagsAttribute` applied. The performance of `Enum`'s methods that utilize reflection should be improved. Support for enum validation should be improved to support flag enums. Additionally, there should be added direct support for the retrieval of `Attribute`s applied to enum members.

## Rationale and Usage
1. Nearly all of `Enum`'s static methods are non-generic leading to the following issues.
   * Requires boxing for methods with enum input parameters losing type-safe, eg. `IsDefined` and `GetName`.
   * Requires casting/unboxing for methods with an enum return value, eg. `ToObject`, `Parse`, and `GetValues`.
   * Requires the enum type to be explicitly specified as an argument.
   * Requires invocation using static method syntax.
2. Support for flag enums is limited to just the `HasFlag` method which isn't type-safe, is very inefficient, and is ambiguous as to whether it indicates if the value has all or any of the specified flags.
3. Many of `Enum`'s methods use reflection on each invocation instead of caching the reflection results leading to very poor performance.
4. Built-in support for enum validation is currently limited to the `IsDefined` method which doesn't support flag enum combinations and is inefficient due to a lack of caching of reflection results.
5. The pattern to associate extra data with an enum member using `Attribute`s is not supported and instead requires users to manually retrieve the `Attribute`s via reflection. This pattern is commonly used on enums with the `DescriptionAttribute` and `EnumMemberAttribute`.

This proposal stems from my work on the library [Enums.NET](https://github.com/TylerBrinkley/Enums.NET) which addresses each of these issues and will be the basis for an implementation. This proposal requires two `C#` language features to be added in order for this proposal to proceed. The first one is that `Enum` needs to be added as a valid type argument constraint in `C#` as is proposed in [roslyn#262](https://github.com/dotnet/roslyn/issues/262). Additionally, this proposal specifies extension methods within `System.Enum` and as such requires `C#` to allow extension methods within non-static classes, a proposal of which needs to be worked up.

With this proposal implemented what used to be this to validate a flag enum value

```cs
MyFlagEnum value = ???;
bool isValid = (value & Enum.GetValues(typeof(MyFlagEnum))
    .Cast<MyFlagEnum>()
    .Aggregate((working, next) => working | next)) == value;
```

now becomes just this which works for validating both flag enum and standard enum values

```cs
MyFlagEnum value = ???;
bool isValid = value.IsValid();
```

and what used to be this to retrieve the `DescriptionAttribute.Description`

```cs
MyEnum value = ???;
string description = ((DescriptionAttribute)Attribute.GetCustomAttribute(
    typeof(MyEnum).GetField(value.ToString()),
    typeof(DescriptionAttribute),
    false))?.Description;
```

now becomes this

```cs
MyEnum value = ???;
string description = value.GetMember()?.Attributes.Get<DescriptionAttribute>()?.Description;
```

or this

```cs
MyEnum value = ???;
string description = value.GetAttributes()?.Get<DescriptionAttribute>()?.Description;
```

## Proposed API
```diff
 namespace System {
     public abstract class Enum : ValueType, IComparable, IConvertible, IFormattable {
-        public bool HasFlag(Enum flag);
+        [Obsolete("Use FlagEnum.HasAllFlags or FlagEnum.HasAnyFlags instead"), EditorBrowsable(EditorBrowsableState.Never)] public bool HasFlag(Enum flag);
-        public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct;
+        public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct, Enum;
-        public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result) where TEnum : struct;
+        public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result) where TEnum : struct, Enum;

         // Generic versions of existing methods
+        public static int CompareTo<TEnum>(this TEnum value, TEnum other) where TEnum : struct, Enum;
+        public static bool Equals<TEnum>(this TEnum value, TEnum other) where TEnum : struct, Enum;
+        public static string Format<TEnum>(TEnum value, string format) where TEnum : struct, Enum;
+        public static string GetName<TEnum>(this TEnum value) where TEnum : struct, Enum;
+        public static IEnumerable<string> GetNames<TEnum>() where TEnum : struct, Enum;
+        public static Type GetUnderlyingType<TEnum>() where TEnum : struct, Enum;
+        public static IEnumerable<TEnum> GetValues<TEnum>() where TEnum : struct, Enum;
+        public static bool IsDefined<TEnum>(this TEnum value) where TEnum : struct, Enum;
+        public static TEnum Parse<TEnum>(string value) where TEnum : struct, Enum;
+        public static TEnum Parse<TEnum>(string value, bool ignoreCase) where TEnum : struct, Enum;
+        public static byte ToByte<TEnum>(TEnum value) where TEnum : struct, Enum;
+        public static short ToInt16<TEnum>(TEnum value) where TEnum : struct, Enum;
+        public static int ToInt32<TEnum>(TEnum value) where TEnum : struct, Enum;
+        public static long ToInt64<TEnum>(TEnum value) where TEnum : struct, Enum;
+        public static TEnum ToObject<TEnum>(object value) where TEnum : struct, Enum;
+        public static TEnum ToObject<TEnum>(sbyte value) where TEnum : struct, Enum;
+        public static TEnum ToObject<TEnum>(byte value) where TEnum : struct, Enum;
+        public static TEnum ToObject<TEnum>(short value) where TEnum : struct, Enum;
+        public static TEnum ToObject<TEnum>(ushort value) where TEnum : struct, Enum;
+        public static TEnum ToObject<TEnum>(int value) where TEnum : struct, Enum;
+        public static TEnum ToObject<TEnum>(uint value) where TEnum : struct, Enum;
+        public static TEnum ToObject<TEnum>(long value) where TEnum : struct, Enum;
+        public static TEnum ToObject<TEnum>(ulong value) where TEnum : struct, Enum;
+        public static sbyte ToSByte<TEnum>(TEnum value) where TEnum : struct, Enum;
+        public static ushort ToUInt16<TEnum>(TEnum value) where TEnum : struct, Enum;
+        public static uint ToUInt32<TEnum>(TEnum value) where TEnum : struct, Enum;
+        public static ulong ToUInt64<TEnum>(TEnum value) where TEnum : struct, Enum;

         // Non-Generic versions of existing methods
+        public static bool TryParse(Type enumType, string value, out object result);
+        public static bool TryParse(Type enumType, string value, bool ignoreCase, out object result);

         // New Generic API
+        public static Enums.AttributeCollection GetAttributes<TEnum>(this TEnum value) where TEnum : struct, Enum;
+        public static Enums.EnumMember<TEnum> GetMember<TEnum>(this TEnum value) where TEnum : struct, Enum;
+        public static Enums.EnumMember<TEnum> GetMember<TEnum>(string name) where TEnum : struct, Enum;
+        public static Enums.EnumMember<TEnum> GetMember<TEnum>(string name, bool ignoreCase) where TEnum : struct, Enum;
+        public static IEnumerable<Enums.EnumMember<TEnum>> GetMembers<TEnum>() where TEnum : struct, Enum;
+        public static object GetUnderlyingValue<TEnum>(TEnum value) where TEnum : struct, Enum;
+        public static bool IsValid<TEnum>(this TEnum value) where TEnum : struct, Enum;

         // New Non-Generic API
+        public static Enums.AttributeCollection GetAttributes(Type enumType, object value);
+        public static Enums.EnumMember GetMember(Type enumType, object value)
+        public static Enums.EnumMember GetMember(Type enumType, string name);
+        public static Enums.EnumMember GetMember(Type enumType, string name, bool ignoreCase);
+        public static IEnumerable<Enums.EnumMember> GetMembers(Type enumType);
+        public static object GetUnderlyingValue(Type enumType, object value);
+        public static bool IsValid(Type enumType, object value)
     }
 }
 // Separate namespace to make flag enum extension methods visibility optional
+namespace System.Flags {
+    public static class FlagEnum {
         // Generic API
+        public static TEnum CombineFlags<TEnum>(this TEnum value, TEnum flags) where TEnum : struct, Enum;
+        public static TEnum CombineFlags<TEnum>(TEnum flag0, TEnum flag1, TEnum flag2) where TEnum : struct, Enum;
+        public static TEnum CombineFlags<TEnum>(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3) where TEnum : struct, Enum;
+        public static TEnum CombineFlags<TEnum>(TEnum flag0, TEnum flag1, TEnum flag2, TEnum flag3, TEnum flag4) where TEnum : struct, Enum;
+        public static TEnum CombineFlags<TEnum>(params TEnum[] flags) where TEnum : struct, Enum;
+        public static TEnum CommonFlags<TEnum>(this TEnum value, TEnum flags) where TEnum : struct, Enum;
+        public static TEnum GetAllFlags<TEnum>() where TEnum : struct, Enum;
+        public static IEnumerable<TEnum> GetFlags<TEnum>(this TEnum value) where TEnum : struct, Enum;
+        public static bool HasAllFlags<TEnum>(this TEnum value) where TEnum : struct, Enum;
+        public static bool HasAllFlags<TEnum>(this TEnum value, TEnum flags) where TEnum : struct, Enum;
+        public static bool HasAnyFlags<TEnum>(this TEnum value) where TEnum : struct, Enum;
+        public static bool HasAnyFlags<TEnum>(this TEnum value, TEnum flags) where TEnum : struct, Enum;
+        public static bool IsFlagEnum<TEnum>() where TEnum : struct, Enum;
+        public static bool IsValidFlagCombination<TEnum>(TEnum value) where TEnum : struct, Enum;
+        public static TEnum RemoveFlags<TEnum>(this TEnum value, TEnum flags) where TEnum : struct, Enum;

         // Non-Generic API
+        public static object CombineFlags(Type enumType, object value, object flags);
+        public static object CombineFlags(Type enumType, object flag0, object flag1, object flag2);
+        public static object CombineFlags(Type enumType, object flag0, object flag1, object flag2, object flag3);
+        public static object CombineFlags(Type enumType, object flag0, object flag1, object flag2, object flag3, object flag4);
+        public static object CombineFlags(Type enumType, params object[] flags);
+        public static object CommonFlags(Type enumType, object value, object flags);
+        public static object GetAllFlags(Type enumType);
+        public static IEnumerable<object> GetFlags(Type enumType, object value);
+        public static bool HasAllFlags(Type enumType, object value);
+        public static bool HasAllFlags(Type enumType, object value, object flags);
+        public static bool HasAnyFlags(Type enumType, object value);
+        public static bool HasAnyFlags(Type enumType, object value, object flags);
+        public static bool IsFlagEnum(Type enumType);
+        public static bool IsValidFlagCombination(Type enumType, object value);
+        public static object RemoveFlags(Type enumType, object value, object flags);
+    }
+}
 // Separate namespace to avoid cluttering the root System namespace
+namespace System.Enums {
+    public abstract class EnumMember : IEquatable<EnumMember>, IConvertible, IFormattable {
+        public AttributeCollection Attributes { get; }
+        public string Name { get; }
+        public object Value { get; }
+        public bool Equals(EnumMember other);
+        public override bool Equals(object other);
+        public override int GetHashCode();
+        public object GetUnderlyingValue();
+        public byte ToByte();
+        public short ToInt16();
+        public int ToInt32();
+        public long ToInt64();
+        public sbyte ToSByte();
+        public override string ToString();
+        public string ToString(string format);
+        public ushort ToUInt16();
+        public uint ToUInt32();
+        public ulong ToUInt64();
+    }
+    public abstract class EnumMember<TEnum> : EnumMember, IEquatable<EnumMember<TEnum>> {
+        public new TEnum Value { get; }
+        public bool Equals(EnumMember<TEnum> other);
+    }
+    public sealed class AttributeCollection : IList<Attribute>, IReadOnlyList<Attribute> {
+        public int Count { get; }
+        public Attribute this[int index] { get; }
+        public TAttribute Get<TAttribute>() where TAttribute : Attribute;
+        public Attribute Get(Type enumType);
+        public IEnumerable<TAttribute> GetAll<TAttribute>() where TAttribute : Attribute;
+        public IEnumerable<Attribute> GetAll(Type enumType);
+        public IEnumerator<Attribute> GetEnumerator();
+        public bool Has<TAttribute>() where TAttribute : Attribute;
+        public bool Has(Type attributeType);
+    }
+    public sealed class PrimaryEnumMemberAttribute : Attribute {
+        public PrimaryEnumMemberAttribute();
+    }
+    public sealed class EnumComparer<TEnum> : IEqualityComparer<TEnum>, IComparer<TEnum>, IEqualityComparer, IComparer where TEnum : struct, Enum {
+        public EnumComparer();
+        public static EnumComparer<TEnum> Instance { get; }
+        public int Compare(TEnum x, TEnum y);
+        public bool Equals(TEnum x, TEnum y);
+        public int GetHashCode(TEnum obj);
+    }
+}
```

## Details

## Open Questions