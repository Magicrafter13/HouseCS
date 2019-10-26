using HouseCS.ConsoleUtils;
using System;
using System.Collections.Generic;

namespace HouseCS.Items.Clothes {
	/// <summary>
	/// Shirt, has color
	/// </summary>
	public class Shirt : Clothing, IItem {
		private const string typeS = "Shirt";

		/// <summary>
		/// string of Item sub-type
		/// </summary>
		public new string SubType => typeS;

		/// <summary>
		/// Matches keyword against Item data
		/// </summary>
		/// <param name="keywords">Keywords to search for</param>
		/// <returns>String output if keywords matched</returns>
		public new List<ColorText> Search(List<string> keywords) {
			List<ColorText> output = new List<ColorText>();
			foreach (string key in keywords) {
				if (key.Equals(Color, StringComparison.OrdinalIgnoreCase)) {
					output.Add(ListInfo(true));
					output.Add(new ColorText(typeS));
					output.Add(ListInfo(false));
				}
			}
			return output;
		}

		/// <summary>
		/// Exports Shirt information
		/// </summary>
		/// <returns>String of shirt constructor</returns>
		public new string Export() {
			return $"new Shirt(\"{Color}\", {RoomID}),";
		}

		/// <summary>
		/// Minor details for list
		/// </summary>
		/// <param name="beforeNotAfter">True for left side, False for right side</param>
		/// <returns>ColorText object of minor shirt details</returns>
		public new ColorText ListInfo(bool beforeNotAfter) => new ColorText(beforeNotAfter ? $"{Color} " : "", ConsoleColor.White);

		/// <summary>
		/// Information about shirt
		/// </summary>
		/// <returns>ColorText object of important info</returns>
		public new ColorText ToText() => new ColorText($"This is a {Color} {SubType}", ConsoleColor.White);

		/// <summary>
		/// Creates black shirt
		/// </summary>
		public Shirt() : base() { }

		/// <summary>
		/// Creates colored shirt
		/// </summary>
		/// <param name="color">Color for shirt</param>
		/// <param name="room">Room for shirt</param>
		public Shirt(string color, int room) : base(color, room) { }
	}
}
