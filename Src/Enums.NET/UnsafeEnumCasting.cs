using System.Runtime.CompilerServices;

namespace EnumsNET
{
	internal static class UnsafeEnumCasting
	{
		[MethodImpl(MethodImplOptions.ForwardRef)]
		public static extern sbyte ToSByte<TEnum>(TEnum value);

		[MethodImpl(MethodImplOptions.ForwardRef)]
		public static extern byte ToByte<TEnum>(TEnum value);

		[MethodImpl(MethodImplOptions.ForwardRef)]
		public static extern short ToInt16<TEnum>(TEnum value);

		[MethodImpl(MethodImplOptions.ForwardRef)]
		public static extern ushort ToUInt16<TEnum>(TEnum value);

		[MethodImpl(MethodImplOptions.ForwardRef)]
		public static extern int ToInt32<TEnum>(TEnum value);

		[MethodImpl(MethodImplOptions.ForwardRef)]
		public static extern uint ToUInt32<TEnum>(TEnum value);

		[MethodImpl(MethodImplOptions.ForwardRef)]
		public static extern long ToInt64<TEnum>(TEnum value);

		[MethodImpl(MethodImplOptions.ForwardRef)]
		public static extern ulong ToUInt64<TEnum>(TEnum value);

		[MethodImpl(MethodImplOptions.ForwardRef)]
		public static extern TEnum ToEnum<TEnum>(sbyte value);

		[MethodImpl(MethodImplOptions.ForwardRef)]
		public static extern TEnum ToEnum<TEnum>(byte value);

		[MethodImpl(MethodImplOptions.ForwardRef)]
		public static extern TEnum ToEnum<TEnum>(short value);

		[MethodImpl(MethodImplOptions.ForwardRef)]
		public static extern TEnum ToEnum<TEnum>(ushort value);

		[MethodImpl(MethodImplOptions.ForwardRef)]
		public static extern TEnum ToEnum<TEnum>(int value);

		[MethodImpl(MethodImplOptions.ForwardRef)]
		public static extern TEnum ToEnum<TEnum>(uint value);

		[MethodImpl(MethodImplOptions.ForwardRef)]
		public static extern TEnum ToEnum<TEnum>(long value);

		[MethodImpl(MethodImplOptions.ForwardRef)]
		public static extern TEnum ToEnum<TEnum>(ulong value);
	}
}
