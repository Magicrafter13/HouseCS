using System;
using System.Collections.Generic;
using HouseCS.ConsoleUtils;

namespace HouseCS.Items.Clothes {
	/// <summary>
	/// Clothing, can have a color
	/// </summary>
	public class Clothing : IItem {
		private const string typeS = "Clothing";

		/// <summary>
		/// Room the clothes are in
		/// </summary>
		public int RoomID { get; private set; }

		/// <summary>
		/// Clothes color
		/// </summary>
		public string Color { get; set; }

		/// <summary>
		/// string of Item type
		/// </summary>
		public string Type => typeS;

		/// <summary>
		/// string of Item sub-type
		/// </summary>
		public string SubType => typeS;

		/// <summary>
		/// Sets RoomID of Clothing
		/// </summary>
		public void Move(int room) {
			RoomID = room;
		}

		/// <summary>
		/// Matches keyword against Item data
		/// </summary>
		/// <param name="keywords">Keywords to search for</param>
		/// <returns>String output if keywords matched</returns>
		public List<ColorText> Search(List<string> keywords) {
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
		/// Exports Clothing information
		/// </summary>
		/// <returns>String of clothing constructor</returns>
		public string Export() {
			return $"new Clothing(\"{Color}\", {RoomID}),";
		}

		/// <summary>
		/// Don't use
		/// </summary>
		/// <param name="item">test Item</param>
		/// <returns>false</returns>
		public bool HasItem(IItem item) => false;

		/// <summary>
		/// Minor details for list
		/// </summary>
		/// <param name="beforeNotAfter">True for left side, False for right side</param>
		/// <returns>ColorText object of minor clothing details</returns>
		public ColorText ListInfo(bool beforeNotAfter) => new ColorText(beforeNotAfter ? $"{Color} " : " - Generic", ConsoleColor.White);

		/// <summary>
		/// Information about clothing
		/// </summary>
		/// <returns>ColorText object of important info</returns>
		public ColorText ToText() => new ColorText($"This is a Generic piece of clothing, it is {Color}", ConsoleColor.White);

		/// <summary>
		/// Creates a black piece of clothing
		/// </summary>
		public Clothing() : this("Black", -1) { }

		/// <summary>
		/// Creates a colored piece of clothing
		/// </summary>
		/// <param name="color">Color for clothes</param>
		/// <param name="room">Room for clothing</param>
		public Clothing(string color, int room) {
			Color = color;
			RoomID = room;
		}
	}
}
