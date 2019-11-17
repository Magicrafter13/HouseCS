using HouseCS.ConsoleUtils;
using HouseCS.Items.Clothes;
using System;
using System.Collections.Generic;

namespace HouseCS.Items.Containers {
	/// <summary>
	/// Dressers are technically more flexible than bookshelves, however they haven't quite reached my full vision yet. They only hold Clothing.
	/// </summary>
	public class Dresser : Container, IItem {
		private const string typeS = "Dresser";

		/// <summary>
		/// Creates an empty dresser with no name
		/// </summary>
		public Dresser() : base() { }

		/// <summary>
		/// Creates a dresser
		/// </summary>
		/// <param name="items">Clothes in dresser</param>
		/// <param name="name">Name of dresser</param>
		public Dresser(List<Clothing> items, string name) : base(new List<IItem>(items), name) { }

		/// <summary>
		/// string of item sub-type
		/// </summary>
		public new string SubType => typeS;

		/// <summary>
		/// Matches keywords against item data
		/// </summary>
		/// <param name="keywords">Keywords to search for</param>
		/// <returns>String output if keywords matched</returns>
		public new List<ColorText> Search(List<string> keywords) {
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
					if (output.Count == 0 && temp.Count > 0) output.Add(new ColorText("Dresser:\n"));
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
		/// Exports dresser information
		/// </summary>
		/// <param name="space">How many spaces to start the string with</param>
		/// <returns>Copyable constructor of dresser</returns>
		public new string Export(int space) {
			string retStr = string.Empty;
			for (int i = 0; i < space; i++)
				retStr += " ";
			retStr += $"new Dresser(new List<Clothing>() {{{(Items.Count > 0 ? "\n" : " ")}";
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
		/// Checks if dresser has clothes of type
		/// </summary>
		/// <param name="type">Clothing type to test for</param>
		/// <returns>True if clothes are found, False if not</returns>
		public bool HasClothes(string type) {
			if (type is null)
				throw new ArgumentNullException(nameof(type));
			for (int i = 0; i < Size; i++)
				if (GetItem(i).SubType.Equals(type, StringComparison.OrdinalIgnoreCase))
					return true;
			return false;
		}

		/// <summary>
		/// Minor details for list
		/// </summary>
		/// <param name="beforeNotAfter">True for left side, False for right side</param>
		/// <returns>ColorText object of minor dresser details</returns>
		public new ColorText ListInfo(bool beforeNotAfter) => beforeNotAfter
			? string.IsNullOrEmpty(Name)
				? ColorText.Empty
				: new ColorText($"{Name} ")
			: Size > 0
				? new ColorText(new string[] { " [", Size.ToString(), " pieces of ", "Clothing", "]" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White })
				: new ColorText(new string[] { " [", "Empty", "]" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });

		/// <summary>
		/// Information about dresser
		/// </summary>
		/// <returns>ColorText object of important info</returns>
		public new ColorText ToText() {
			List<string> retStr = new List<string>() { "Items", " in this ", "Dresser", ":" };
			List<ConsoleColor> retClr = new List<ConsoleColor>() { ConsoleColor.DarkYellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White };
			List<string> type = new List<string>();
			List<int> count = new List<int>();
			for (int i = 0; i < Size; i++) {
				string tempType = GetItem(i).SubType;
				bool isNewType = true;
				for (int s = 0; s < type.Count; s++) {
					if (type[s].Equals(tempType, StringComparison.OrdinalIgnoreCase)) {
						isNewType = false;
						int tempInt = count[s];
						count.RemoveAt(s);
						count.Insert(s, tempInt + 1);
					}
				}
				if (isNewType) {
					type.Add(tempType);
					count.Add(1);
				}
			}
			for (int i = 0; i < type.Count; i++) {
				retStr.Add($"\n\t{type[i]}: {count[i]}");
				retClr.Add(ConsoleColor.White);
			}
			retStr.Add("\nEnd of ");
			retClr.Add(ConsoleColor.White);
			retStr.Add("Dresser");
			retClr.Add(ConsoleColor.Yellow);
			retStr.Add(" contents.");
			retClr.Add(ConsoleColor.White);
			return new ColorText(retStr.ToArray(), retClr.ToArray());
		}
	}
}
