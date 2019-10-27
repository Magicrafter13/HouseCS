using HouseCS.ConsoleUtils;
using System;
using System.Collections.Generic;

namespace HouseCS.Items {
	/// <summary>
	/// Empty Item, used in place of null, so the program can actually handle it on run-time
	/// </summary>
	public class Empty : IItem {
		private const string message = "You have no items/objects selected";

		private const string typeS = "No Item";

		/// <summary>
		/// Room the 'empty' is in
		/// </summary>
		public int RoomID { get; set; }

		/// <summary>
		/// string of Item type
		/// </summary>
		public string Type => typeS;

		/// <summary>
		/// string of Item sub-type
		/// </summary>
		public string SubType => typeS;

		/// <summary>
		/// Matches keyword against Item data
		/// </summary>
		/// <param name="keywords">Keywords to search for</param>
		/// <returns>String output if keywords matched</returns>
		public List<ColorText> Search(List<string> keywords) {
			List<ColorText> output = new List<ColorText> {
				ColorText.Empty
			};
			return output;
		}

		/// <summary>
		/// Exports Empty information
		/// </summary>
		/// <returns>String of empty constructor</returns>
		public string Export() {
			return "new Empty(),";
		}

		/// <summary>
		/// Don't use
		/// </summary>
		/// <param name="item">Index of sub item</param>
		/// <returns>Book, telling you that this should be seen</returns>
		public IItem GetSub(int item) => new Book("This item doesn't contain other items.", "(I don't think it should be possible to see this...)" + item, 2018, -1);

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
