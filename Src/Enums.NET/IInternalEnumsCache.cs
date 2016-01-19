namespace EnumsNET
{
	internal interface IInternalEnumsCache<TEnum> : IEnumsCache<TEnum>
	{
		string InternalFormat(IEnumMemberInfo<TEnum> info, string format);

		string InternalFormat(TEnum value, IEnumMemberInfo<TEnum> info, EnumFormat format);

		string InternalFormat(TEnum value, IEnumMemberInfo<TEnum> info, EnumFormat format0, EnumFormat format1);

		string InternalFormat(TEnum value, IEnumMemberInfo<TEnum> info, EnumFormat format0, EnumFormat format1, EnumFormat format2);

		string InternalFormat(TEnum value, IEnumMemberInfo<TEnum> info, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3);

		string InternalFormat(TEnum value, IEnumMemberInfo<TEnum> info, EnumFormat format0, EnumFormat format1, EnumFormat format2, EnumFormat format3, EnumFormat format4);

		string InternalFormat(TEnum value, IEnumMemberInfo<TEnum> info, params EnumFormat[] formats);
	}
}