using System;

namespace HouseCS.Exceptions {
	internal class ArrayTooSmall : Exception {
		public ArrayTooSmall(int required, int actual) : base($"Size of given Array needs to be at least {required}, but is only {actual}.") { }
	}
}
