using System.Collections.Generic;
using HouseCS.Items;

namespace HouseCS
{
	public class ItemImport
	{
		public static readonly Bookshelf[] bookshelfs =
		{
			new Bookshelf(new List<Book>()
			{
				new Book("The Hobbit", "J.R.R. Tolkien", 1937),
				new Book("Harry Potter and the Sorcerer's Stone", "J.K. Rowling", 1997)
			})
		};
		public static readonly int[] bookshelfs_f = { 1 };
		public static readonly Computer[] computers =
		{
			new Computer("HP", "Pavilion", "p7-1500z", true, "Desktop"),
			new Computer("Compaq", "Presario", "1625", false, "Laptop"),
			new Computer("Gateway", "510xl", "", false, "Desktop"),
			new Computer("HP", "Pavilion", "dv6t-1300", true, "Laptop"),
			new Computer("Tandy", "1000", "TL/2", false, "Desktop")
		};
		public static readonly int[] computers_f = { 1, 1, 1, 0, 1 };
		public static readonly GameConsole[] consoles =
		{
			new GameConsole(0, "Nintendo", "SNES"),
			new GameConsole(0, "Nintendo", "N64"),
			new GameConsole(0, "Nintendo", "GameCube"),
			new GameConsole(0, "Nintendo", "Wii"),
			new GameConsole(0, "Nintendo", "Wii U"),
			new GameConsole(0, "Microsoft", "Xbox 360"),
			new GameConsole(1, "Nintendo", "Gameboy Color"),
			new GameConsole(1, "Sega", "Game Gear"),
			new GameConsole(1, "Sega", "Game Gear"),
			new GameConsole(1, "Nintendo", "DSi"),
			new GameConsole(1, "Nintendo", "DSi"),
			new GameConsole(1, "Nintendo", "New 3DS XL"),
			new GameConsole(0, "Atari", "VCS 2600"),
			new GameConsole(0, "Nintendo", "NES"),
			new GameConsole(0, "Sega", "Genesis")
		};
		public static readonly int[] consoles_f = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
		public static readonly Display[] displays =
		{
			new Display(true, new List<IItem>()
			{
				computers[0],
				computers[2]
			}, 24),
			new Display(false, new List<IItem>()
			{
				consoles[5],
				consoles[6],
				consoles[7]
			}, 40)
		};
		public static readonly int[] displays_f = { 1, 1 };
		public static readonly Bed[] beds = {
			new Bed(false, 2),
			new Bed(false, 0)
		};
		public static readonly int[] beds_f = { 1, 1 };
	}
}
