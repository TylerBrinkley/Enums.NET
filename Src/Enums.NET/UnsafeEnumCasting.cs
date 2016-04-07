using System.Runtime.CompilerServices;

namespace EnumsNET
{
	internal static class UnsafeEnumCasting
	{
		[MethodImpl(MethodImplOptions.ForwardRef)]
		public static extern T2 Cast<T1, T2>(T1 value);
	}
}
