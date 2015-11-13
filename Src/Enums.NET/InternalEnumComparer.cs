using System.Collections.Generic;

namespace EnumsNET
{
    internal class InternalEnumComparer<TEnum> : IEqualityComparer<TEnum>
    {
        public bool Equals(TEnum x, TEnum y) => EnumsCache<TEnum>.Equal(x, y);

        public int GetHashCode(TEnum obj) => EnumsCache<TEnum>.GetHashCode(obj);
    }
}
