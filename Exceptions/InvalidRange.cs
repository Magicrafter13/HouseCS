using System;

namespace HouseCS.Exceptions
{
	internal class InvalidRange : Exception
	{
		public InvalidRange(int min, int max) : base($"int range invalid (min = {min}, max = {max}) min must be less than max.") { }
		public InvalidRange(double min, double max) : base($"double range invalid (min = {min}, max = {max}) min must be less than max.") { }
	}
}
