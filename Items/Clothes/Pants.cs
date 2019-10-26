using HouseCS.ConsoleUtils;
using System;
using System.Collections.Generic;

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
		/// Exports Pants information
		/// </summary>
		/// <returns>String of pants constructor</returns>
		public new string Export() {
			return $"new Pants(\"{Color}\", {RoomID}),";
		}

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
		/// <param name="room">Room for pants</param>
		public Pants(string color, int room) : base(color, room) { }
	}
}
