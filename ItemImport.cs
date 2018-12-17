using System.Collections.Generic;
using HouseCS.Items;
using HouseCS.Items.Clothes;
using HouseCS.Items.Containers;

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
		public static readonly int[] bookshelfsF = { 1 };
		public static readonly Computer[] computers =
		{
			new Computer("HP", "Pavilion", "p7-1500z", true, "Desktop"),
			new Computer("Compaq", "Presario", "1625", false, "Laptop"),
			new Computer("Gateway", "510xl", "", false, "Desktop"),
			new Computer("HP", "Pavilion", "dv6t-1300", true, "Laptop"),
			new Computer("Tandy", "1000", "TL/2", false, "Desktop")
		};
		public static readonly int[] computersF = { 1, 1, 1, 0, 1 };
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
		public static readonly int[] consolesF = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
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
		public static readonly int[] displaysF = { 1, 1 };
		public static readonly Bed[] beds = {
			new Bed(false, 2),
			new Bed(false, 0)
		};
		public static readonly int[] bedsF = { 1, 1 };
		public static readonly Container[] containers = {
			new Container(new List<IItem>()),
    	new Container(new List<IItem>())
		};
		public static readonly int[] containersF = { 0, 0 };
		public static readonly Fridge[] fridges = {
			new Fridge(new List<IItem>() {
				new Book("One", "Two", 3),
				new Book("Four", "Five", 6)
			}, false),
			new Fridge(new List<IItem>() {
				new Book("Seven", "Eight", 9)
			}, false)
		};
		public static readonly int[] fridgesF = { 0, 0 };
		public static readonly Dresser[] dressers = {
			new Dresser(new List<IItem>() {
				new Shirt("Red"),
				new Pants("Blue"),
				new Pants("Black")
			}),
			new Dresser(new List<IItem>() {
				new Pants("Black")
			})
		};
		public static readonly int[] dressersF = { 1, 1 };
		public static readonly Table[] tables = {
			new Table(new List<IItem>() {
				new GameConsole(0, "Atari", "VCS 2600"),
				new GameConsole(0, "Nintendo", "NES"),
				new GameConsole(0, "Sega", "Genesis")
			})
		};
		public static readonly int[] tablesF = { 1 };
	}
}
