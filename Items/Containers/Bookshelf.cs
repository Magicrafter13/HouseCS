using HouseCS.ConsoleUtils;
using System;
using System.Collections.Generic;

namespace HouseCS.Items.Containers {
	/// <summary>
	/// Bookshelfs are one of the least flexible containers. They can only hold books. But hey, they can hold books.
	/// </summary>
	public class Bookshelf : Container, IItem {
		private static readonly string typeS = "Bookshelf";

		/// <summary>
		/// Creates an empty bookshelf, with no name
		/// </summary>
		public Bookshelf() : base() { }

		/// <summary>
		/// Creates a bookshelf
		/// </summary>
		/// <param name="books">Books</param>
		/// <param name="name">Name of bookshelf</param>
		public Bookshelf(List<Book> books, string name) : base(new List<IItem>(books), name) { }

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
					if (output.Count == 0 && temp.Count > 0) output.Add(new ColorText("Bookshelf:\n"));
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
		/// Exports bookshelf information
		/// </summary>
		/// <param name="space">How many spaces to start the string with</param>
		/// <returns>Copyable constructor of bookshelf</returns>
		public new string Export(int space) {
			string retStr = string.Empty;
			for (int i = 0; i < space; i++)
				retStr += " ";
			retStr += $"new Bookshelf(new List<Book>() {{{(Items.Count > 0 ? "\n" : " ")}";
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
		/// Adds item to bookshelf (if it's a book)
		/// </summary>
		/// <param name="item">Item to add</param>
		/// <returns>ColorText object saying the book was added, or that the item isn't a book</returns>
		public new ColorText AddItem(IItem item) {
			if (item is null)
				throw new ArgumentNullException(nameof(item));
			if (item is Book) {
				base.AddItem(item);
				return new ColorText(new string[] { "\nBook", " added", " to ", "Bookshelf", ".\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.DarkBlue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
			}
			else return new ColorText(new string[] { "Item", " is not a ", "Book", "." }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
		}

		/// <summary>
		/// Removes a book
		/// </summary>
		/// <param name="book">Book to remove</param>
		/// <returns>ColorText object saying the book was removed, or warning that the bookshelf didn't have the book</returns>
		public ColorText RemoveBook(int book) {
			if (book < 0 || book >= Items.Count)
				return Items.Count == 0
					? new ColorText(new string[] { "Bookshelf", " is already empty!" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White })
					: new ColorText(new string[] { "Bookshelf", " only has ", Items.Count.ToString(), Items.Count > 1 ? " Books" : " Book", " on it." }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.White });
			Items.RemoveAt(book);
			return new ColorText(new string[] { "\nBook", $" {book} ", "removed", ".\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.DarkBlue });
		}

		/// <summary>
		/// Removes a book
		/// </summary>
		/// <param name="book">Book to remove</param>
		/// <returns>ColorText object saying the book was removed, or that the book wasn't on the bookshelf</returns>
		public ColorText RemoveBook(Book book) => book is null ? throw new ArgumentNullException(nameof(book)) : Items.Remove(book) ? new ColorText(new string[] { "\nBook", ", ", "removed", ".\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.DarkBlue }) : new ColorText(new string[] { "No matching ", "Book", " found." }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });

		/// <summary>
		/// Removes a book
		/// </summary>
		/// <param name="book">Book to remove</param>
		/// <returns>Status of removed book, or warning that book was not a book</returns>
		[Obsolete("Method deprecated, please use RemoveBook(Book);")]
		public ColorText RemoveBook(IItem book)
		{
			if (book is null)
				throw new ArgumentNullException(nameof(book));
			if (book is Book)
				return RemoveBook((Book)book);
			else
				return new ColorText(new string[] { "Item", " is not a ", "Book", "." }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
		}

		/// <summary>
		/// Minor details for list
		/// </summary>
		/// <param name="beforeNotAfter">True for left side, False for right side</param>
		/// <returns>ColorText object of minor bookshelf details</returns>
		public new ColorText ListInfo(bool beforeNotAfter) => beforeNotAfter
			? new ColorText(new string[] { Size > 0 ? string.Empty : $"Empty{(string.IsNullOrEmpty(Name) ? " " : $", {Name} ")}" }, new ConsoleColor[] { ConsoleColor.White })
			: Size > 0
				? new ColorText(new string[] { " (", Size.ToString(), Size > 1 ? " Books" : " Book", $"){(string.IsNullOrEmpty(Name) ? string.Empty : $", {Name}")}" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, Size > 1 ? ConsoleColor.DarkYellow : ConsoleColor.Yellow, ConsoleColor.White })
				: new ColorText(new string[] { string.Empty }, new ConsoleColor[] { ConsoleColor.White });

		/// <summary>
		/// Information about bookshelf
		/// </summary>
		/// <returns>ColorText object of important info</returns>
		public new ColorText ToText() {
			List<string> retStr = new List<string>() { "Books", " in this ", "shelf", ":" };
			List<ConsoleColor> retClr = new List<ConsoleColor>() { ConsoleColor.DarkYellow, ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White };
			for (int i = 0; i < Items.Count; i++) {
				retStr.Add($"\n\t{i.ToString()}");
				retStr.Add($": \"{((Book)Items[i]).Title}\" by \"{((Book)Items[i]).Author}\" ID: {((Book)Items[i]).ID}");
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
