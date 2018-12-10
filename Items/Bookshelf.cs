using System.Collections.Generic;

namespace HouseCS.Items
{
  public class Bookshelf : IItem
	{
		private readonly List<Book> books;
		private static readonly string typeS = "Bookshelf";

		public Bookshelf() : this(new List<Book>()) { }
		public Bookshelf(List<Book> b) => books = b;
		public bool HasItem(IItem test) {
			if (test is Book) foreach (Book b in books) if (test == b) return true;
			return false;
		}
		public void AddBook(Book b) => books.Add(b);
		public string RemoveBook(int b)
		{
			if (b < 0 || b >= books.Count)
				return books.Count == 0
					? $"{Program.Bright("yellow", "Bookshelf")} is already empty!"
					: $"{Program.Bright("yellow", "Bookshelf")} only has {Program.Bright("cyan", books.Count.ToString())}{(books.Count > 1 ? Program.Color("yellow", " Books") : Program.Bright("yellow", " Book"))} on it.";
			books.RemoveAt(b);
			return $"\n{Program.Bright("yellow", "Book")} {b} {Program.Color("blue", "removed")}.\n";
		}
		public string RemoveBook(IItem b) {
			for (int i = 0; i < books.Count; i++) if (books.Remove((Book)b)) return $"{Program.Bright("yellow", "\nBook")}, {Program.Color("blue", "removed")}.\n";
			return $"No matching {Program.Bright("yellow", "Book")} found.";
		}
		public int BookCount => books.Count;
		public Book GetBook(int i) => books[i];
		public string Type => typeS;
		public string ListInfo(bool beforeNotAfter) => beforeNotAfter
			? BookCount > 0
				? string.Empty
				: "Empty "
			: BookCount > 0
				? $" ({Program.Bright("cyan", BookCount.ToString())}{(BookCount > 1 ? Program.Color("yellow", " Books") : Program.Bright("yellow", " Book"))})"
				: string.Empty;
		public override string ToString()
		{
			string ret_val = $"{Program.Color("yellow", "Books")} in this {Program.Color("yellow", "shelf")}:";
			for (int i = 0; i < books.Count; i++) ret_val += $"\n\t{Program.Bright("cyan", i.ToString())}: \"{books[i].Title}\" by \"{books[i].Author}\" ID: " + books[i].ID;
			return ret_val + $"\nEnd of {Program.Bright("yellow", "Bookshelf")} contents.";
		}
	}
}
