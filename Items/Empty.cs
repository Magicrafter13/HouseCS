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
		/// Creates Emptiness
		/// </summary>
		public Empty() { }

		/// <summary> Name of empty </summary>
		public string Name { get; private set; } = "If you see this in the program, something went wrong.";

		/// <summary> string of Item type </summary>
		public string Type => typeS;

		/// <summary> string of Item sub-type </summary>
		public string SubType => typeS;

		/// <summary>
		/// Matches keywords against item data
		/// </summary>
		/// <param name="keywords">Keywords to search for</param>
		/// <returns>String output if keywords matched</returns>
		public List<ColorText> Search(List<string> keywords) => new List<ColorText> { ColorText.Empty };

		/// <summary>
		/// Exports empty information
		/// </summary>
		/// <returns>Copyable constructor of empty</returns>
		public string Export() => $"new Empty({Name}),";

		/// <summary>
		/// Don't use
		/// </summary>
		/// <param name="item">Index of sub item</param>
		/// <returns>Book, telling you that this shouldn't be seen</returns>
		public IItem GetSub(int item) => new Book("This item doesn't contain other items.", "(I don't think it should be possible to see this...)" + item, 2018);

		/// <summary>
		/// Don't use
		/// </summary>
		/// <param name="item">Test item</param>
		/// <returns>false</returns>
		public bool HasItem(IItem item) => false;

		/// <summary>
		/// Adds to the name of the 'Empty', could be used to debug
		/// </summary>
		/// <param name="name">String added to Name</param>
		public void Rename(string name) => Name += name ?? throw new ArgumentNullException(nameof(name));

		/// <summary>
		/// Empty
		/// </summary>
		/// <param name="beforeNotAfter">Unused</param>
		/// <returns>Emptiness</returns>
		public ColorText ListInfo(bool beforeNotAfter) => ColorText.Empty;

		/// <summary>
		/// Empty
		/// </summary>
		/// <returns>message</returns>
		public ColorText ToText() => new ColorText(new string[] { message }, new ConsoleColor[] { ConsoleColor.White });
	}
}
