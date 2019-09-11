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

        internal static ref byte GetRawData(this object obj) =>
            ref As<RawData>(obj).Data;
    }

    // Helper class to assist with unsafe pinning of arbitrary objects.
    // It's used by VM code.
    internal class RawData
    {
        public byte Data;
    }
}