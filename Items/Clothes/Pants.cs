using System;
using HouseCS.ConsoleUtils;

namespace HouseCS.Items.Clothes {
	public class Pants : Clothing, IItem {
		private static readonly string typeS = "Pants";

		public Pants() : base() { }
		public Pants(string c) : base(c) { }
		public new string SubType => typeS;
		public new ColorText ListInfo(bool beforeNotAfter) => new ColorText(beforeNotAfter ? $"{Color} " : "", ConsoleColor.White);
		public new ColorText ToText() => new ColorText($"These are {Color} {SubType}", ConsoleColor.White);
	}
}