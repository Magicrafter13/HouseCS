using System.Collections.Generic;

namespace HouseCS.Items
{
	public class Bookshelf : IItem
	{
		private readonly List<Book> books;
		private static readonly string typeS = "Bookshelf";

		public Bookshelf() : this(new List<Book>()) { }
		public Bookshelf(List<Book> b) => books = b;
		public void AddBook(Book b) => books.Add(b);
		public string RemoveBook(int b)
		{
			if (b < 0 || b >= books.Count)
				return books.Count == 0
					? "Bookshelf is already empty!"
					: $"Bookshelf only has {books.Count} book{(books.Count > 1 ? "s" : "")} on it.";
			books.RemoveAt(b);
			return $"\nBook {b} removed.\n";
		}
		public int BookCount => books.Count;
		public Book GetBook(int i) => books[i];
		public string Type => typeS;
		public string ListInfo(bool beforeNotAfter) => beforeNotAfter ? BookCount > 0 ? "" : "Empty " : $" ({BookCount} books)";
		public override string ToString()
		{
			string retVal = string.Empty;
			foreach (Book b in books)
				retVal += $"\n\t\"{b.Title}\" by \"{b.Author}\" ID: {b.ID}";
			return $"Books in this shelf:{retVal}\nEnd of Bookshelf contents.";
		}
	}
}
