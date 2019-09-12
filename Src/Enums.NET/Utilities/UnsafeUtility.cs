using System.Runtime.CompilerServices;
using System.Security;

namespace EnumsNET.Utilities
{
    internal static class UnsafeUtility
    {
#if FORWARD_REF
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [SecuritySafeCritical]
        public static extern ref TTo As<TFrom, TTo>(ref TFrom source);

#if FORWARD_REF
        [MethodImpl(MethodImplOptions.ForwardRef | MethodImplOptions.AggressiveInlining)]
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [SecuritySafeCritical]
        public static extern T As<T>(object? value) where T : class;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref byte GetRawData(this object obj) =>
            ref As<RawData>(obj).Data;

        private sealed class RawData
        {
            public byte Data;
        }
    }
}