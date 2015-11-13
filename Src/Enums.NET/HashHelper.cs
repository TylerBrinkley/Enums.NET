using System;

namespace EnumsNET
{
	internal static class HashHelper
	{
		// Hashing prime base was chosen so that the hashing prime base cubed would be slightly larger than 2 and that
		// finding the log of a number with base of hashing prime base minimizes the traversal of the hashing prime list while
		// finding the prime greater than or equal to that number
		//private const double _hashingPrimeBase = 1.2717541501;
		private const double _invLogOfHashingPrimeBase = 4.1597827805595945;
		private const int _hashingPrimeOffset = 4;

		private static readonly int[] _hashingPrimeList = {
			3, 5, 5, 7, 11, 11, 17, 17, 23, 29, 37, 47, 59, 79, 97, 127, 157, 199, 251, 331, 409,
			521, 659, 839, 1069, 1361, 1723, 2203, 2789, 3547, 4513, 5737, 7297, 9277, 11801,
			15013, 19079, 24281, 30859, 39241, 49919, 63467, 80713, 102653, 130547, 166021, 211151,
			268517, 341491, 434293, 552301, 702391, 893281, 1136041, 1444747, 1837349, 2336671,
			2971663, 3779201, 4806211, 6112339, 7773361, 9885803, 12572311, 15988897, 20333941,
			25859803, 32887273, 41824537, 53190503, 67645243, 86028121, 109406621, 139138333,
			176949743, 225036619, 286191203, 363964847, 462873787, 588661643, 748632917, 952077017,
			1210807909, 1539849907, 1958310509, int.MaxValue // Note: int.MaxValue is prime
		};

		public static int GetPrimeGTE(int number)
		{
			if (number < 4)
			{
				return 3;
			}
			var index = (int)(Math.Log(number) * _invLogOfHashingPrimeBase) - _hashingPrimeOffset;
			while (_hashingPrimeList[index - 1] >= number)
			{
				--index;
			}
			return _hashingPrimeList[index];
		}

		public static int GetPrimeGTE(int number, bool getMin)
		{
			var highestPrime = GetPrimeGTE(number);
			if (getMin && number > 11)
			{
				for (var i = number | 1; i < highestPrime; i += 2)
				{
					if (OddIsPrime(number))
					{
						return i;
					}
				}
			}
			return highestPrime;
		}

		private static bool OddIsPrime(int number)
		{
			var max = (int)Math.Sqrt(number);
			for (var i = 3; i <= max; i += 2)
			{
				if ((number % i) == 0)
				{
					return false;
				}
			}
			return true;
		}
	}
}