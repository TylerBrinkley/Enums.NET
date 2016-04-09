.class private auto ansi beforefieldinit EnumsNET.EnumInfo`3<TEnum,
	valuetype .ctor ([mscorlib]System.IFormattable, [mscorlib]System.IConvertible, class [mscorlib]System.IComparable`1<!TInt>, class [mscorlib]System.IEquatable`1<!TInt>, [mscorlib]System.ValueType) TInt,
	valuetype .ctor (class EnumsNET.Numerics.INumericProvider`1<!TInt>, [mscorlib]System.ValueType) TIntProvider>
	   extends [mscorlib]System.Object
	   implements class EnumsNET.IEnumInfo`1<!TEnum>,
				  EnumsNET.ICommonEnumInfo
{
	.method assembly hidebysig static !TEnum ToEnum(!TInt 'value') cil managed
	{
	  .maxstack  1
	  IL_0000:  ldarg.0
	  IL_0001:  ret
	}

	.method assembly hidebysig static !TInt ToInt(!TEnum 'value') cil managed
	{
	  .maxstack  1
	  IL_0000:  ldarg.0
	  IL_0001:  ret
	}
}
