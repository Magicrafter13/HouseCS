using HouseCS.ConsoleUtils;
using System;
using System.Collections.Generic;

namespace HouseCS.Items {
	/// <summary>
	/// Book, has a title, author, and publishing year
	/// </summary>
	public class Book : IItem {
		/// <summary>
		/// Total book count for program
		/// </summary>
		private static int totalBooks = 0;

		private const string typeS = "Book";

		/// <summary>
		/// Book title
		/// </summary>
		public string Title { get; private set; }

		/// <summary>
		/// Book author
		/// </summary>
		public string Author { get; private set; }

		/// <summary>
		/// Book publishing year
		/// </summary>
		public int Year { get; private set; }

		/// <summary>
		/// Unique object id
		/// </summary>
		public int ID { get; }

		/// <summary>
		/// Name of book
		/// </summary>
		public string Name => Title;

		/// <summary>
		/// string of Item type
		/// </summary>
		public string Type => typeS;

		/// <summary>
		/// string of Item sub-type
		/// </summary>
		public string SubType => typeS;

		/// <summary>
		/// Matches keyword against Item data
		/// </summary>
		/// <param name="keywords">Keywords to search for</param>
		/// <returns>String output if keywords matched</returns>
		public List<ColorText> Search(List<string> keywords) {
			List<ColorText> output = new List<ColorText>();
			foreach (string key in keywords) {
				if (Title.ToLower().Contains(key.ToLower()) ||
				key.Equals(Author, StringComparison.OrdinalIgnoreCase) ||
				key.Equals(Year.ToString())) {
					output.Add(ListInfo(true));
					output.Add(new ColorText(typeS));
					output.Add(ListInfo(false));
				}
			}
			return output;
		}

		/// <summary>
		/// Exports Book information
		/// </summary>
		/// <returns>String of Book constructor</returns>
		public string Export() {
			return $"new Book(\"{Title}\", \"{Author}\", {Year}),";
		}

		/// <summary>
		/// Does the same as the constructor, sets title, author, and publishing year
		/// </summary>
		/// <param name="title">Book title</param>
		/// <param name="author">Book author</param>
		/// <param name="year">Book publishing year</param>
		public void Reset(string title, string author, int year) {
			Title = title;
			Author = author;
			Year = year >= 1600 ? year : 1600;
		}

		/// <summary>
		/// Don't use
		/// </summary>
		/// <param name="item">test Item</param>
		/// <returns>false</returns>
		public bool HasItem(IItem item) => false;

		/// <summary>
		/// Minor details for list
		/// </summary>
		/// <param name="beforeNotAfter">True for left side, False for right side</param>
		/// <returns>ColorText object of minor book details</returns>
		public ColorText ListInfo(bool beforeNotAfter) => new ColorText(new string[] { beforeNotAfter ? string.Empty : $": {Title}" }, new ConsoleColor[] { ConsoleColor.White });

		/// <summary>
		/// Information about book
		/// </summary>
		/// <returns>ColorText object of important info</returns>
		public ColorText ToText() => new ColorText(new string[] { $"Title: {Title}\nAuthor: {Author}\nYear: {Year}" }, new ConsoleColor[] { ConsoleColor.White });

		/// <summary>
		/// Creates a book titled "none", written by "none", in 1600 AD
		/// </summary>
		public Book() : this("none", "none", 1600) { }

		/// <summary>
		/// Creates a book titled tile, written by author, in year AD
		/// </summary>
		/// <param name="title">Book title</param>
		/// <param name="author">Book author</param>
		/// <param name="year">Book publishing year</param>
		public Book(string title, string author, int year) {
			Reset(title, author, year);
			totalBooks++;
			ID = totalBooks;
		}
	}
}
