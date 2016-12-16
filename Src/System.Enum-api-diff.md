```diff
namespace System {
    public class Enum {
+       [Obsolete]
+       [EditorBrowsable(EditorBrowsableState.Never)]
        public bool HasFlag(Enum flag);
    }
+   public class EnumMember {
+       public object Value { get; }
+       public string Name { get; }
+       public IEnumerable<Attribute> Attributes { get; }
    }
+   public class EnumMember<TEnum> : EnumMember {
+       public new TEnum Value { get; }
    }
+   public sealed class PrimaryEnumMemberAttribute : Attribute {
    }
+   public sealed class EnumComparer<TEnum> : IEqualityComparer<TEnum>, IComparer<TEnum>, IEqualityComparer, IComparer where TEnum : struct, Enum {
+       public int Compare(TEnum x, TEnum y);
+       public bool Equals(TEnum x, TEnum y);
+       public int GetHashCode(TEnum obj);
    }
}
```