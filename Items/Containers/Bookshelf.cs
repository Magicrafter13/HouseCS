﻿using System;
using System.Collections.Generic;
using HouseCS.ConsoleUtils;

namespace HouseCS.Items.Containers {
	/// <summary>
	/// Bookshelf, has books
	/// </summary>
	public class Bookshelf : Container, IItem {
		private static readonly string typeS = "Bookshelf";

		/// <summary>
		/// string of Item sub-type
		/// </summary>
		public new string SubType => typeS;

		/// <summary>
		/// Adds Item to bookshelf (if it's a book)
		/// </summary>
		/// <param name="item">Item to add</param>
		/// <returns>ColorText object saying the book was added, or that the item isn't a book</returns>
		public new ColorText AddItem(IItem item) {
			if (item is Book) {
				base.AddItem(item);
				return new ColorText(new string[] { "\nBook", " added", " to ", "Bookshelf", ".\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.DarkBlue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
			} else
				return new ColorText(new string[] { "Item", " is not a ", "Book", "." }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
		}

		/// <summary>
		/// Removes a book by index
		/// </summary>
		/// <param name="book">Index of book to remove</param>
		/// <returns>ColorText object saying the book was removed, or telling the user why it didn't work</returns>
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
		/// <param name="book">Book object</param>
		/// <returns>ColorText object saying the book was removed, or the book wasn't found</returns>
		public ColorText RemoveBook(IItem book) {
			for (int i = 0; i < Items.Count; i++)
				if (Items.Remove((Book)book))
					return new ColorText(new string[] { "\nBook", ", ", "removed", ".\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.DarkBlue });
			return new ColorText(new string[] { "No matching ", "Book", " found." }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
		}

		/// <summary>
		/// Minor details for list
		/// </summary>
		/// <param name="beforeNotAfter">True for left side, False for right side</param>
		/// <returns>ColorText object of minor bookshelf details</returns>
		public new ColorText ListInfo(bool beforeNotAfter) => beforeNotAfter
			? new ColorText(new string[] { Size > 0 ? "" : "Empty " }, new ConsoleColor[] { ConsoleColor.White })
			: Size > 0
				? new ColorText(new string[] { " (", Size.ToString(), Size > 1 ? " Books" : " Book", ")" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, Size > 1 ? ConsoleColor.DarkYellow : ConsoleColor.Yellow, ConsoleColor.White })
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

		/// <summary>
		/// Creates a bookshelf with no books on it
		/// </summary>
		public Bookshelf() : base() { }

		/// <summary>
		/// Creates a bookshelf with books on it
		/// </summary>
		/// <param name="books">List of books</param>
		/// <param name="room">Room for bookshelf</param>
		public Bookshelf(List<Book> books, int room) : base() {
			foreach (Book a in books)
				base.AddItem(a);
		}
	}
}