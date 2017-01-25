### Tier II
```diff
 namespace System {
     public abstract class Enum : ValueType, IComparable, IConvertible, IFormattable {
         // New API
+        public static EnumFormat RegisterCustomEnumFormat(Func<Enums.EnumMember, string> enumMemberFormatter);

         // New Generic API
+        public static string Format<TEnum>(TEnum value, params Enums.EnumFormat[] formats) where TEnum : struct, Enum;
+        public static Enums.EnumMember<TEnum> GetMember<TEnum>(string value, params Enums.EnumFormat[] formats) where TEnum : struct, Enum;
+        public static Enums.EnumMember<TEnum> GetMember<TEnum>(string value, bool ignoreCase, params Enums.EnumFormat[] formats) where TEnum : struct, Enum;
+        public static TEnum Parse<TEnum>(string value, params Enums.EnumFormat[] formats) where TEnum : struct, Enum;
+        public static TEnum Parse<TEnum>(string value, bool ignoreCase, params Enums.EnumFormat[] formats) where TEnum : struct, Enum;
+        public static string ToString<TEnum>(this TEnum value, Enums.EnumFormat format) where TEnum : struct, Enum;
+        public static string ToString<TEnum>(this TEnum value, Enums.EnumFormat format0, Enums.EnumFormat format1) where TEnum : struct, Enum;
+        public static string ToString<TEnum>(this TEnum value, Enums.EnumFormat format0, Enums.EnumFormat format1, Enums.EnumFormat format2) where TEnum : struct, Enum;
+        public static string ToString<TEnum>(this TEnum value, params Enums.EnumFormat[] formats) where TEnum : struct, Enum;
+        public static bool TryParse<TEnum>(string value, out TEnum result, params Enums.EnumFormat[] formats) where TEnum : struct, Enum;
+        public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result, params Enums.EnumFormat[] formats) where TEnum : struct, Enum;

         // New Non-Generic API
+        public static string Format(Type enumType, object value, params Enums.EnumFormat[] formats);
+        public static Enums.EnumMember GetMember(Type enumType, string value, params Enums.EnumFormat[] formats);
+        public static Enums.EnumMember GetMember(Type enumType, string value, bool ignoreCase, params Enums.EnumFormat[] formats);
+        public static object Parse(Type enumType, string value, params Enums.EnumFormat[] formats);
+        public static object Parse(Type enumType, string value, bool ignoreCase, params Enums.EnumFormat[] formats);
+        public static string ToString(Type enumType, object value, Enums.EnumFormat format);
+        public static string ToString(Type enumType, object value, Enums.EnumFormat format0, Enums.EnumFormat format1);
+        public static string ToString(Type enumType, object value, Enums.EnumFormat format0, Enums.EnumFormat format1, Enums.EnumFormat format2);
+        public static string ToString(Type enumType, object value, params Enums.EnumFormat[] formats);
+        public static bool TryParse(Type enumType, string value, out object result, params Enums.EnumFormat[] formats);
+        public static bool TryParse(Type enumType, string value, bool ignoreCase, out object result, params Enums.EnumFormat[] formats);
     }
 }
 namespace System.Enums {
     public abstract class EnumMember : IEquatable<EnumMember>, IConvertible, IFormattable {
+        public string ToString(EnumFormat format);
+        public string ToString(EnumFormat format0, EnumFormat format1);
+        public string ToString(EnumFormat format0, EnumFormat format1, EnumFormat format2);
+        public string ToString(params EnumFormat[] formats);
     }
+    public enum EnumFormat {
+        DecimalValue = 1,
+        HexadecimalValue = 2,
+        Name = 3,
+        UnderlyingValue = 0
+    }
 }
```

### Tier III
```diff
 namespace System {
     public abstract class Enum : ValueType, IComparable, IConvertible, IFormattable {
         // New Generic API
+        public static int GetMemberCount<TEnum>() where TEnum : struct, Enum;
+        public static int GetMemberCount<TEnum>(Enums.EnumMemberSelection selection) where TEnum : struct, Enum;
+        public static IEnumerable<EnumMember<TEnum>> GetMembers<TEnum>(Enums.EnumMemberSelection selection) where TEnum : struct, Enum;
+        public static IEnumerable<string> GetNames<TEnum>(Enums.EnumMemberSelection selection) where TEnum : struct, Enum;
+        public static IEnumerable<TEnum> GetValues<TEnum>(Enums.EnumMemberSelection selection) where TEnum : struct, Enum;
+        public static bool IsValid<TEnum>(this TEnum value, Enums.EnumValidation validation) where TEnum : struct, Enum;
+        public static TEnum ToObject<TEnum>(object value, Enums.EnumValidation validation) where TEnum : struct, Enum;
+        public static TEnum ToObject<TEnum>(sbyte value, Enums.EnumValidation validation) where TEnum : struct, Enum;
+        public static TEnum ToObject<TEnum>(byte value, Enums.EnumValidation validation) where TEnum : struct, Enum;
+        public static TEnum ToObject<TEnum>(short value, Enums.EnumValidation validation) where TEnum : struct, Enum;
+        public static TEnum ToObject<TEnum>(ushort value, Enums.EnumValidation validation) where TEnum : struct, Enum;
+        public static TEnum ToObject<TEnum>(int value, Enums.EnumValidation validation) where TEnum : struct, Enum;
+        public static TEnum ToObject<TEnum>(uint value, Enums.EnumValidation validation) where TEnum : struct, Enum;
+        public static TEnum ToObject<TEnum>(long value, Enums.EnumValidation validation) where TEnum : struct, Enum;
+        public static TEnum ToObject<TEnum>(ulong value, Enums.EnumValidation validation) where TEnum : struct, Enum;
+        public static bool TryToObject<TEnum>(object value, out TEnum result) where TEnum : struct, Enum;
+        public static bool TryToObject<TEnum>(sbyte value, out TEnum result) where TEnum : struct, Enum;
+        public static bool TryToObject<TEnum>(byte value, out TEnum result) where TEnum : struct, Enum;
+        public static bool TryToObject<TEnum>(short value, out TEnum result) where TEnum : struct, Enum;
+        public static bool TryToObject<TEnum>(ushort value, out TEnum result) where TEnum : struct, Enum;
+        public static bool TryToObject<TEnum>(int value, out TEnum result) where TEnum : struct, Enum;
+        public static bool TryToObject<TEnum>(uint value, out TEnum result) where TEnum : struct, Enum;
+        public static bool TryToObject<TEnum>(long value, out TEnum result) where TEnum : struct, Enum;
+        public static bool TryToObject<TEnum>(ulong value, out TEnum result) where TEnum : struct, Enum;
+        public static bool TryToObject<TEnum>(object value, Enums.EnumValidation validation, out TEnum result) where TEnum : struct, Enum;
+        public static bool TryToObject<TEnum>(sbyte value, Enums.EnumValidation validation, out TEnum result) where TEnum : struct, Enum;
+        public static bool TryToObject<TEnum>(byte value, Enums.EnumValidation validation, out TEnum result) where TEnum : struct, Enum;
+        public static bool TryToObject<TEnum>(short value, Enums.EnumValidation validation, out TEnum result) where TEnum : struct, Enum;
+        public static bool TryToObject<TEnum>(ushort value, Enums.EnumValidation validation, out TEnum result) where TEnum : struct, Enum;
+        public static bool TryToObject<TEnum>(int value, Enums.EnumValidation validation, out TEnum result) where TEnum : struct, Enum;
+        public static bool TryToObject<TEnum>(uint value, Enums.EnumValidation validation, out TEnum result) where TEnum : struct, Enum;
+        public static bool TryToObject<TEnum>(long value, Enums.EnumValidation validation, out TEnum result) where TEnum : struct, Enum;
+        public static bool TryToObject<TEnum>(ulong value, Enums.EnumValidation validation, out TEnum result) where TEnum : struct, Enum;

         // New Non-Generic API
+        public static int GetMemberCount(Type enumType);
+        public static int GetMemberCount(Type enumType, Enums.EnumMemberSelection selection);
+        public static IEnumerable<EnumMember> GetMembers(Type enumType, Enums.EnumMemberSelection selection);
+        public static IEnumerable<string> GetNames(Type enumType, Enums.EnumMemberSelection selection);
+        public static IEnumerable<object> GetValues(Type enumType, Enums.EnumMemberSelection selection);
+        public static bool IsValid(Type enumType, object value, Enums.EnumValidation validation);
+        public static object ToObject(Type enumType, object value, Enums.EnumValidation validation);
+        public static object ToObject(Type enumType, sbyte value, Enums.EnumValidation validation);
+        public static object ToObject(Type enumType, byte value, Enums.EnumValidation validation);
+        public static object ToObject(Type enumType, short value, Enums.EnumValidation validation);
+        public static object ToObject(Type enumType, ushort value, Enums.EnumValidation validation);
+        public static object ToObject(Type enumType, int value, Enums.EnumValidation validation);
+        public static object ToObject(Type enumType, uint value, Enums.EnumValidation validation);
+        public static object ToObject(Type enumType, long value, Enums.EnumValidation validation);
+        public static object ToObject(Type enumType, ulong value, Enums.EnumValidation validation);
+        public static bool TryToObject(Type enumType, object value, out object result);
+        public static bool TryToObject(Type enumType, sbyte value, out object result);
+        public static bool TryToObject(Type enumType, byte value, out object result);
+        public static bool TryToObject(Type enumType, short value, out object result);
+        public static bool TryToObject(Type enumType, ushort value, out object result);
+        public static bool TryToObject(Type enumType, int value, out object result);
+        public static bool TryToObject(Type enumType, uint value, out object result);
+        public static bool TryToObject(Type enumType, long value, out object result);
+        public static bool TryToObject(Type enumType, ulong value, out object result);
+        public static bool TryToObject(Type enumType, object value, Enums.EnumValidation validation, out object result);
+        public static bool TryToObject(Type enumType, sbyte value, Enums.EnumValidation validation, out object result);
+        public static bool TryToObject(Type enumType, byte value, Enums.EnumValidation validation, out object result);
+        public static bool TryToObject(Type enumType, short value, Enums.EnumValidation validation, out object result);
+        public static bool TryToObject(Type enumType, ushort value, Enums.EnumValidation validation, out object result);
+        public static bool TryToObject(Type enumType, int value, Enums.EnumValidation validation, out object result);
+        public static bool TryToObject(Type enumType, uint value, Enums.EnumValidation validation, out object result);
+        public static bool TryToObject(Type enumType, long value, Enums.EnumValidation validation, out object result);
+        public static bool TryToObject(Type enumType, ulong value, Enums.EnumValidation validation, out object result);
     }
 }
 namespace System.Flags {
     public static class FlagEnum {
+        public static IEnumerable<EnumMember<TEnum>> GetFlagMembers<TEnum>(TEnum value) where TEnum : struct, Enum;
+        public static TEnum ToggleFlags<TEnum>(TEnum value) where TEnum : struct, Enum;
+        public static TEnum ToggleFlags<TEnum>(TEnum value, TEnum flags) where TEnum : struct, Enum;
+        public static IEnumerable<EnumMember> GetFlagMembers(Type enumType, object value);
+        public static object ToggleFlags(Type enumType, object value);
+        public static object ToggleFlags(Type enumType, object value, object flags);
     }
 }
 namespace System.Enums {
+    public enum EnumMemberSelection {
+        All = 0,
+        Distinct = 1,
+        Flags = 2
+    }
+    public enum EnumValidation {
+        Default = 1,
+        IsDefined = 3,
+        IsValidFlagCombination = 4,
+        None = 0
+        NoOverflow = 2,
+    }
+    public interface IEnumValidatorAttribute<TEnum> where TEnum : struct, Enum {
+        bool IsValid(TEnum value);
+    }
 }
```