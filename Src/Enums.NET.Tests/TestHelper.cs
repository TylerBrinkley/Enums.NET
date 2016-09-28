using System;
using NUnit.Framework;

namespace EnumsNET.Tests
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
    }
}
