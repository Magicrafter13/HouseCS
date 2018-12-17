using System;
using HouseCS.ConsoleUtils;

namespace HouseCS.Items
{
	public class Empty : IItem
	{
		private static readonly string message = "You have no items/objects selected";
		private static readonly string typeS = "No Item";

		public Empty() { }
		public IItem GetSub(int i) => new Book("This item doesn't contain other items.", "(I don't think it should be possible to see this...)", 2018);
		public bool HasItem(IItem test) => false;
		public string Type => typeS;
		public string SubType => typeS;
		public ColorText ListInfo(bool beforeNotAfter) => ColorText.Empty;
		public ColorText ToText() => new ColorText(new string[] { message }, new ConsoleColor[] { ConsoleColor.White });
	}
}
