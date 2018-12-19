using System;
using System.Collections.Generic;
using HouseCS.ConsoleUtils;

namespace HouseCS.Items.Containers {
	public class Dresser : Container, IItem {
		private static readonly string typeS = "Dresser";

		public Dresser() : base() { }
		public Dresser(List<IItem> iS) : base(iS) { }
		public bool HasClothes(string type) {
			for (int i = 0; i < Size; i++)
				if (GetItem(i).SubType.Equals(type, StringComparison.OrdinalIgnoreCase))
					return true;
			return false;
		}
		public new string SubType => typeS;
		public new ColorText ListInfo(bool beforeNotAfter) => beforeNotAfter
			? ColorText.Empty
			: Size > 0
				? new ColorText(new string[] { " [", "Empty", "]" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White })
				: new ColorText(new string[] { " [", Size.ToString(), " pieces of ", "Clothing", "]" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
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