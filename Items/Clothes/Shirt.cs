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
		/// Creates black shirt
		/// </summary>
		public Shirt() : base() { }

		/// <summary>
		/// Creates colored shirt
		/// </summary>
		/// <param name="color">Color for shirt</param>
		/// <param name="name">Name of Shirt</param>
		public Shirt(string color, string name) : base(color, name) { }

		/// <summary> string of Item sub-type </summary>
		public new string SubType => typeS;

		/// <summary>
		/// Matches keywords against item data
		/// </summary>
		/// <param name="keywords">Keywords to search for</param>
		/// <returns>String output if keywords matched</returns>
		public new List<ColorText> Search(List<string> keywords) {
			List<ColorText> output = new List<ColorText>();
			foreach (string key in keywords) {
				if (key.Equals(Color, StringComparison.OrdinalIgnoreCase) ||
					key.Equals(Name, StringComparison.OrdinalIgnoreCase)) {
					output.Add(ListInfo(true));
					output.Add(new ColorText(typeS));
					output.Add(ListInfo(false));
				}
			}
			return output;
		}

		/// <summary>
		/// Exports shirt information
		/// </summary>
		/// <returns>Copyable constructor of shirt</returns>
		public new string Export() => $"new Shirt(\"{Color}\", \"{Name}\"),";

		/// <summary>
		/// Minor details for list
		/// </summary>
		/// <param name="beforeNotAfter">True for left side, False for right side</param>
		/// <returns>ColorText object of minor shirt details</returns>
		public new ColorText ListInfo(bool beforeNotAfter) => new ColorText(beforeNotAfter ? $"{(Name.Equals(string.Empty) ? Color : Name)} " : "", ConsoleColor.White);

		/// <summary>
		/// Information about shirt
		/// </summary>
		/// <returns>ColorText object of important info</returns>
		public new ColorText ToText() => new ColorText($"This is a {Color} {SubType}{(Name.Equals(string.Empty) ? string.Empty : $", labeled {Name}")}", ConsoleColor.White);
	}
}
