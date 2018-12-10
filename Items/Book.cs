namespace HouseCS.Items
{
	public class Book : IItem
	{
		private static int totalBooks = 0;
		private static readonly string typeS = "Book";

		public void Reset(string t, string a, int y)
		{
			Title = t;
			Author = a;
			Year = y >= 1600 ? y : 1600;
		}
		public Book() : this("none", "none", 1600) { }
		public Book(string t, string a, int y)
		{
			Reset(t, a, y);
			totalBooks++;
			ID = totalBooks;
		}
		public bool HasItem(IItem test) => false;
		public string Title { get; private set; }
		public string Author { get; private set; }
		public int Year { get; private set; }
		public int ID { get; }
		public string Type => typeS;
		public string ListInfo(bool beforeNotAfter) => beforeNotAfter ? string.Empty : $": {Title}";
		public override string ToString() => $"Title: {Title}\nAuthor: {Author}\nYear: {Year}";
	}
}
