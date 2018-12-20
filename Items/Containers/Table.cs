using System;
using System.Collections.Generic;
using HouseCS.ConsoleUtils;

namespace HouseCS.Items.Containers {
	/// <summary>
	/// Table with Items
	/// </summary>
	public class Table : Container, IItem {
		private const string typeS = "Table";

		/// <summary>
		/// string of Item sub-type
		/// </summary>
		public new string SubType => typeS;

		/// <summary>
		/// Minor details for list
		/// </summary>
		/// <param name="beforeNotAfter">True for left side, False for right side</param>
		/// <returns>ColorText object of minor table details</returns>
		public new ColorText ListInfo(bool beforeNotAfter) => beforeNotAfter
			? new ColorText(new string[] { $"{(Size < 9 ? "Clean" : "Dirty")} " }, new ConsoleColor[] { ConsoleColor.White })
			: Size > 0
				? new ColorText(new string[] { " -", Size.ToString(), " Items" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.Yellow })
				: new ColorText(" - Empty", ConsoleColor.White);

		/// <summary>
		/// Information about table
		/// </summary>
		/// <returns>ColorText object of important info</returns>
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

		/// <summary>
		/// Creates empty table
		/// </summary>
		public Table() : base() { }

		/// <summary>
		/// Creates table with Items
		/// </summary>
		/// <param name="items"></param>
		public Table(List<IItem> items) : base(items) { }
	}
}