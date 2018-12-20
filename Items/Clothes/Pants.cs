using System;
using HouseCS.ConsoleUtils;

namespace HouseCS.Items.Clothes {
	/// <summary>
	/// Pants, has a color
	/// </summary>
	public class Pants : Clothing, IItem {
		private const string typeS = "Pants";

		/// <summary>
		/// string of Item sub-type
		/// </summary>
		public new string SubType => typeS;

		/// <summary>
		/// Minor details for list
		/// </summary>
		/// <param name="beforeNotAfter">True for left side, False for right side</param>
		/// <returns>ColorText object of minor pants details</returns>
		public new ColorText ListInfo(bool beforeNotAfter) => new ColorText(beforeNotAfter ? $"{Color} " : "", ConsoleColor.White);

		/// <summary>
		/// Information about pants
		/// </summary>
		/// <returns>ColorText object of important info</returns>
		public new ColorText ToText() => new ColorText($"These are {Color} {SubType}", ConsoleColor.White);

		/// <summary>
		/// Creates black pants
		/// </summary>
		public Pants() : base() { }

		/// <summary>
		/// Creates colored pants
		/// </summary>
		/// <param name="color">Color for clothes</param>
		public Pants(string color) : base(color) { }
	}
}