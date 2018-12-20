using System;
using HouseCS.ConsoleUtils;

namespace HouseCS.Items {
	/// <summary>
	/// Empty Item, used in place of null, so the program can actually handle it on run-time
	/// </summary>
	public class Empty : IItem {
		private const string message = "You have no items/objects selected";

		private static readonly string typeS = "No Item";

		/// <summary>
		/// string of Item type
		/// </summary>
		public string Type => typeS;

		/// <summary>
		/// string of Item sub-type
		/// </summary>
		public string SubType => typeS;

		/// <summary>
		/// Don't use
		/// </summary>
		/// <param name="item">Index of sub item</param>
		/// <returns>Book, telling you that this should be seen</returns>
		public IItem GetSub(int item) => new Book("This item doesn't contain other items.", "(I don't think it should be possible to see this...)", 2018);

		/// <summary>
		/// Don't use
		/// </summary>
		/// <param name="item">test Item</param>
		/// <returns>false</returns>
		public bool HasItem(IItem item) => false;

		/// <summary>
		/// Empty
		/// </summary>
		/// <param name="beforeNotAfter">True is left, False is right</param>
		/// <returns>Emptiness</returns>
		public ColorText ListInfo(bool beforeNotAfter) => ColorText.Empty;

		/// <summary>
		/// Empty
		/// </summary>
		/// <returns>message</returns>
		public ColorText ToText() => new ColorText(new string[] { message }, new ConsoleColor[] { ConsoleColor.White });

		/// <summary>
		/// Creates Emptiness
		/// </summary>
		public Empty() { }
	}
}
