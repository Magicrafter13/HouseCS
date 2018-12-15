using System.Collections.Generic;
using System;
using HouseCS.ConsoleUtils;

namespace HouseCS.Items
{
	public class Bookshelf : IItem {
		private readonly List<Book> books;
		private static readonly string typeS = "Bookshelf";

		public Bookshelf() : this(new List<Book>()) { }
		public Bookshelf(List<Book> b) => books = b;
		public bool HasItem(IItem test) {
			if (test is Book) foreach (Book b in books) if (test == b) return true;
			return false;
		}

		public void AddBook(Book b) => books.Add(b);
		public ColorText RemoveBook(int b)
		{
			if (b < 0 || b >= books.Count)
				return books.Count == 0
					? new ColorText(new string[] { "Bookshelf", " is already empty!" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White })
					: new ColorText(new string[] { "Bookshelf", " only has ", books.Count.ToString(), books.Count > 1 ? " Books" : " Book", " on it." }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.White });
			books.RemoveAt(b);
			return new ColorText(new string[] { "\nBook", $" {b} ", "removed", ".\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.DarkBlue });
		}
		public ColorText RemoveBook(IItem b) {
			for (int i = 0; i < books.Count; i++)
				if (books.Remove((Book)b))
					return new ColorText(new string[] { "\nBook", ", ", "removed", ".\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.DarkBlue });
			return new ColorText(new string[] { "No matching ", "Book", " found." }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
		}
		public int BookCount => books.Count;
		public Book GetBook(int i) => books[i];
		public string Type => typeS;
		public ColorText ListInfo(bool beforeNotAfter) => beforeNotAfter
			? new ColorText(new string[] { BookCount > 0 ? "" : "Empty " }, new ConsoleColor[] { ConsoleColor.White })
			: BookCount > 0
				? new ColorText(new string[] { $" {BookCount.ToString()}", BookCount > 1 ? " Books" : " Book" }, new ConsoleColor[] { ConsoleColor.Cyan, BookCount > 1 ? ConsoleColor.DarkYellow : ConsoleColor.Yellow })
				: new ColorText(new string[] { string.Empty }, new ConsoleColor[] { ConsoleColor.White });
		public ColorText ToText()
		{
			List<string> retStr = new List<string>() { "Books", " in this ", "shelf", ":" };
			List<ConsoleColor> retClr = new List<ConsoleColor>() { ConsoleColor.DarkYellow, ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White };
			for (int i = 0; i < books.Count; i++) {
				retStr.Add($"\n\t{i.ToString()}");
				retStr.Add($": \"{books[i].Title}\" by \"{books[i].Author}\" ID: {books[i].ID}");
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
