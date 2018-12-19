using System;
using System.Collections.Generic;
using HouseCS.ConsoleUtils;

namespace HouseCS.Items.Containers {
	public class Table : Container, IItem {
		private static readonly string typeS = "Table";

		public Table() : base() { }
		public Table(List<IItem> iS) : base(iS) { }
		public new string SubType => typeS;
		public new ColorText ListInfo(bool beforeNotAfter) => beforeNotAfter
			? new ColorText(new string[] { $"{(Size < 9 ? "Clean" : "Dirty")} " }, new ConsoleColor[] { ConsoleColor.White })
			: Size > 0
				? new ColorText(new string[] { " -", Size.ToString(), " Items" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.Yellow })
				: new ColorText(" - Empty", ConsoleColor.White);
		public new ColorText ToText() {
			List<string> retStr = new List<string>() { "Items", " on this ", "Table", ":\n" };
			List<ConsoleColor> retClr = new List<ConsoleColor>() { ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White };
			for (int i = 0; i < Size; i++) {
				retStr.Add($"\t{i.ToString()}");
				retClr.Add(ConsoleColor.Cyan);
				retStr.Add(": ");
				retClr.Add(ConsoleColor.White);
				ColorText left = GetItem(i).ListInfo(true);
				foreach (string str in left.Lines)
					retStr.Add(str);
				foreach (ConsoleColor clr in left.Colors)
					retClr.Add(clr);
				retStr.Add(GetItem(i).SubType);
				retClr.Add(ConsoleColor.Yellow);
				ColorText right = GetItem(i).ListInfo(false);
				foreach (string str in right.Lines)
					retStr.Add(str);
				foreach (ConsoleColor clr in right.Colors)
					retClr.Add(clr);
				retStr.Add("\n");
				retClr.Add(ConsoleColor.White);
			}
			retStr.Add("End of ");
			retClr.Add(ConsoleColor.White);
			retStr.Add("Table");
			retClr.Add(ConsoleColor.Yellow);
			retStr.Add(" contents.");
			retClr.Add(ConsoleColor.White);
			return new ColorText(retStr.ToArray(), retClr.ToArray());
		}
	}
}