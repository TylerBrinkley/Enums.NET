using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnumsNET.Test
{
	public static class TestHelper
	{
		public static void ExpectException<TException>(Action action)
			where TException : Exception
		{
			try
			{
				action();
				Assert.Fail($"Expected exception of type {typeof(TException).Name}");
			}
			catch (TException)
			{
			}
		}

		public static void ArraysAreEqual<T>(T[] array1, T[] array2)
		{
			Assert.AreEqual(array1.Length, array2.Length);

			for (var i = 0; i < array1.Length; ++i)
			{
				Assert.AreEqual(array1[i], array2[i]);
			}
		}

		public static void ArrayOfArraysAreEqual<T>(T[][] array1, T[][] array2)
		{
			Assert.AreEqual(array1.Length, array2.Length);

			for (var i = 0; i < array1.Length; ++i)
			{
				ArraysAreEqual(array1[i], array2[i]);
			}
		}
	}
}
