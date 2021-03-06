using HouseCS.ConsoleUtils;
using System;
using System.Collections.Generic;

namespace HouseCS.Items.Containers
{
	/// <summary>
	/// Tables are second only to standard Containers. They have very few limitations on what they can hold.
	/// </summary>
	public class Table : Container, IItem
	{
		private const string typeS = "Table";

		/// <summary>
		/// Creates an empty table with no name
		/// </summary>
		public Table() : base() { }

		/// <summary>
		/// Creates table
		/// </summary>
		/// <param name="items">Items on table</param>
		/// <param name="name">Name of table</param>
		public Table(List<IItem> items, string name) : base(items, name) { }

		/// <summary>
		/// string of item sub-type
		/// </summary>
		public new string SubType => typeS;

		/// <summary>
		/// Matches keywords against item data
		/// </summary>
		/// <param name="keywords">Keywords to search for</param>
		/// <returns>String output if keywords matched</returns>
		public new List<ColorText> Search(List<string> keywords)
		{
			if (keywords is null)
				throw new ArgumentNullException(nameof(keywords));
			List<ColorText> output = new List<ColorText>();
			foreach (string key in keywords) {
				if ((Size == 0 &&
					key.Equals("Empty", StringComparison.OrdinalIgnoreCase)) ||
					key.Equals(Name, StringComparison.OrdinalIgnoreCase)) {
					output.Add(ListInfo(true));
					output.Add(new ColorText(typeS));
					output.Add(ListInfo(false));
				}
			}
			if (Size > 0) {
				for (int i = 0; i < Items.Count; i++) {
					List<ColorText> temp = Items[i].Search(keywords);
					if (output.Count == 0 && temp.Count > 0) output.Add(new ColorText("Table:\n"));
					if (temp.Count > 0) {
						output.Add(new ColorText($"\t{i}: "));
						output.AddRange(temp);
						output.Add(new ColorText("\n"));
					}
				}
			}
			return output;
		}

		/// <summary>
		/// Exports table information
		/// </summary>
		/// <param name="space">How many spaces to start the string with</param>
		/// <returns>Copyable constructor of table</returns>
		public new string Export(int space)
		{
			string retStr = string.Empty;
			for (int i = 0; i < space; i++)
				retStr += " ";
			retStr += $"new Table(new List<IItem>() {{{(Items.Count > 0 ? "\n" : " ")}";
			for (int i = 0; i < Items.Count; i++) {
				if (Items[i] is Container) {
					retStr += Items[i].SubType switch
					{
						"Bookshelf" => ((Bookshelf)Items[i]).Export(space + 2),
						"Dresser" => ((Dresser)Items[i]).Export(space + 2),
						"Fridge" => ((Fridge)Items[i]).Export(space + 2),
						"Table" => ((Table)Items[i]).Export(space + 2),
						_ => ((Container)Items[i]).Export(space + 2),
					};
					continue;
				}
				if (Items[i] is Display) {
					retStr += $"{((Display)Items[i]).Export(space + 2)}\n";
					continue;
				}
				for (int s = 0; s < space + 2; s++)
					retStr += " ";
				retStr += $"{Items[i].Export()}\n";
			}
			if (Items.Count > 0)
				for (int i = 0; i < space; i++)
					retStr += " ";
			return $"{retStr}}}, \"{Name}\"),\n";
		}

		/// <summary>
		/// Minor details for list
		/// </summary>
		/// <param name="beforeNotAfter">True for left side, False for right side</param>
		/// <returns>ColorText object of minor table details</returns>
		public new ColorText ListInfo(bool beforeNotAfter) => beforeNotAfter
			? new ColorText(new string[] { $"{(Size < 9 ? "Clean" : "Dirty")} " }, new ConsoleColor[] { ConsoleColor.White })
			: Size > 0
				? new ColorText(new string[] { " - ", Size.ToString(), " Items", $"{(string.IsNullOrEmpty(Name) ? string.Empty : $", {Name}")}" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.White })
				: new ColorText($" - Empty{(string.IsNullOrEmpty(Name) ? string.Empty : $", {Name}")}", ConsoleColor.White);

		/// <summary>
		/// Information about table
		/// </summary>
		/// <returns>ColorText object of important info</returns>
		public new ColorText ToText()
		{
			List<string> retStr = new List<string>() { "Items", " on this ", "Table", ":\n" };
			List<ConsoleColor> retClr = new List<ConsoleColor>() { ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White };
			for (int i = 0; i < Size; i++) {
				retStr.Add($"\t{i.ToString()}");
				retClr.Add(ConsoleColor.Cyan);
				retStr.Add(": ");
				retClr.Add(ConsoleColor.White);
				ColorText left = GetItem(i).ListInfo(true);
				foreach (string str in left.GetLines())
					retStr.Add(str);
				foreach (ConsoleColor clr in left.Colors())
					retClr.Add(clr);
				retStr.Add(GetItem(i).SubType);
				retClr.Add(ConsoleColor.Yellow);
				ColorText right = GetItem(i).ListInfo(false);
				foreach (string str in right.GetLines())
					retStr.Add(str);
				foreach (ConsoleColor clr in right.Colors())
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
