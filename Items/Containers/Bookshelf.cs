using System;
using System.Collections.Generic;
using HouseCS.ConsoleUtils;

namespace HouseCS.Items.Containers {
	public class Bookshelf : Container, IItem {
		private static readonly string typeS = "Bookshelf";

		public Bookshelf() : base() { }
		public Bookshelf(List<Book> b) : base() {
			foreach (Book a in b)
				base.AddItem(a);
		}

		public new ColorText AddItem(IItem i) {
			if (i is Book) {
				base.AddItem(i);
				return new ColorText(new string[] { "\nBook", " added", " to ", "Bookshelf", ".\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.DarkBlue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
			} else
				return new ColorText(new string[] { "Item", " is not a ", "Book", "." }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
		}

		public ColorText RemoveBook(int b) {
			if (b < 0 || b >= items.Count)
				return items.Count == 0
					? new ColorText(new string[] { "Bookshelf", " is already empty!" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White })
					: new ColorText(new string[] { "Bookshelf", " only has ", items.Count.ToString(), items.Count > 1 ? " Books" : " Book", " on it." }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.White });
			items.RemoveAt(b);
			return new ColorText(new string[] { "\nBook", $" {b} ", "removed", ".\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.DarkBlue });
		}
		public ColorText RemoveBook(IItem b) {
			for (int i = 0; i < items.Count; i++)
				if (items.Remove((Book)b))
					return new ColorText(new string[] { "\nBook", ", ", "removed", ".\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.DarkBlue });
			return new ColorText(new string[] { "No matching ", "Book", " found." }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
		}
		public new string SubType => typeS;
		public new ColorText ListInfo(bool beforeNotAfter) => beforeNotAfter
			? new ColorText(new string[] { Size > 0 ? "" : "Empty " }, new ConsoleColor[] { ConsoleColor.White })
			: Size > 0
				? new ColorText(new string[] { " (", Size.ToString(), Size > 1 ? " Books" : " Book", ")" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, Size > 1 ? ConsoleColor.DarkYellow : ConsoleColor.Yellow, ConsoleColor.White })
				: new ColorText(new string[] { string.Empty }, new ConsoleColor[] { ConsoleColor.White });
		public new ColorText ToText() {
			List<string> retStr = new List<string>() { "Books", " in this ", "shelf", ":" };
			List<ConsoleColor> retClr = new List<ConsoleColor>() { ConsoleColor.DarkYellow, ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White };
			for (int i = 0; i < items.Count; i++) {
				retStr.Add($"\n\t{i.ToString()}");
				retStr.Add($": \"{((Book)items[i]).Title}\" by \"{((Book)items[i]).Author}\" ID: {((Book)items[i]).ID}");
				retClr.Add(ConsoleColor.Cyan);
				retClr.Add(ConsoleColor.White);
			}
			retStr.Add("\nEnd of ");
			retStr.Add("Bookshelf");
			retStr.Add(" contents.");
			retClr.Add(ConsoleColor.White);
			retClr.Add(ConsoleColor.Yellow);
			retClr.Add(ConsoleColor.White);
			return new ColorText(retStr.ToArray(), retClr.ToArray());
		}
	}
}
