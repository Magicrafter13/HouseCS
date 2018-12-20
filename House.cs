using System;
using System.Collections.Generic;
using HouseCS.ConsoleUtils;
using HouseCS.Items;

namespace HouseCS {
	/// <summary>
	/// Object containing Floor objects, and a paint color.
	/// </summary>
	public class House {
		/// <summary>
		/// Possible colors for the House
		/// </summary>
		public static readonly string[] colors = { "White", "Red", "Brown", "Orange", "Yellow", "Green", "Blue", "Purple", "Pink", "Black" };

		/// <summary>
		/// Possible Item types
		/// </summary>
		public static readonly string[] types = { "*", "Bed", "Book", "Computer", "Console", "Display",
			"Bookshelf", "Container", "Dresser", "Fridge", "Table",
			"Clothing", "Pants", "Shirt" };

		/// <summary>
		/// int of House Color (index for colors)
		/// </summary>
		private int Color { get; }

		/// <summary>
		/// How many floors the house has
		/// </summary>
		public int Size => Floors.Length;

		/// <summary>
		/// The floors of this house
		/// </summary>
		public Floor[] Floors { get; }

		/// <summary>
		/// Makes sure none of the floors are null, by using the default constructor
		/// </summary>
		private void InitializeFloors() {
			for (int i = 0; i < Floors.Length; i++)
				Floors[i] = new Floor();
		}

		/// <summary>
		/// Returns page count for listing Items
		/// </summary>
		/// <param name="floor">Index of floor in house</param>
		/// <param name="rangeStart">Index of first Item on floor</param>
		/// <param name="rangeEnd">Index of last Item on floor</param>
		/// <param name="searchType">string of Item type being searched for</param>
		/// <param name="pageLength">int of how many Items are to be shown per page</param>
		/// <returns>How many pages a listing will take, based on Item type, range, and page length</returns>
		public int PageCount(int floor, int rangeStart, int rangeEnd, string searchType, int pageLength) {
			bool validType = false;
			foreach (string t in types)
				if (searchType.Equals(t, StringComparison.OrdinalIgnoreCase))
					validType = true;
			if (!validType)
				return -1;
			if (Floors[floor].Size == 0)
				return -2;
			if (rangeStart >= rangeEnd)
				return -3;
			if (rangeStart < 0)
				return -4;
			int items = 0;
			for (int i = rangeStart; i < rangeEnd; i++) {
				if (i > Floors[floor].Size)
					continue;
				if (searchType.Equals("*") ||
					searchType.Equals(Floors[floor].GetItem(i).SubType, StringComparison.OrdinalIgnoreCase) ||
					searchType.Equals(Floors[floor].GetItem(i).Type)) {
					items++;
				}
			}
			return items / pageLength + (items % pageLength == 0 ? 0 : 1);
		}

		/// <summary>
		/// Lists Items on the specified floor
		/// </summary>
		/// <param name="floor">Floor on this House to search</param>
		/// <param name="start">Index of first Item</param>
		/// <param name="end">Index of last Item</param>
		/// <param name="type">Item type to find</param>
		/// <param name="pageLength">How many Items per page</param>
		/// <param name="page">Page</param>
		/// <returns>ColorText object either containing a List of Items based on the criteria, or a message explaining what was wrong with the arguments</returns>
		public ColorText List(int floor, int start, int end, string type, int pageLength, int page) {
			bool validType = false;
			foreach (string t in types)
				if (type.Equals(t, StringComparison.OrdinalIgnoreCase))
					validType = true;
			if (!validType)
				return new ColorText(new string[] { type, " is not a valid ", "Item", " type." }, new ConsoleColor[] { ConsoleColor.DarkYellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
			if (Floors[floor].Size == 0)
				return new ColorText(new string[] { "Floor is empty!" }, new ConsoleColor[] { ConsoleColor.White });
			if (start >= end)
				return new ColorText(new string[] { "Start", " must be less than ", "End" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Red });
			if (start < 0)
				return new ColorText(new string[] { "Start", " must be greater than or equal to ", "0" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan });
			List<string> retStr = new List<string>() { "\n" };
			List<ConsoleColor> retClr = new List<ConsoleColor>() { ConsoleColor.White };
			List<IItem> items = new List<IItem>();
			List<int> itemIds = new List<int>();
			for (int i = start; i < end; i++) {
				if (i > Floors[floor].Size)
					continue;
				if (type.Equals("*") ||
					type.Equals(Floors[floor].GetItem(i).Type, StringComparison.OrdinalIgnoreCase) ||
					type.Equals(Floors[floor].GetItem(i).SubType, StringComparison.OrdinalIgnoreCase)) {
					items.Add(Floors[floor].GetItem(i));
					itemIds.Add(i);
				}
			}
			if (items.Count == 0)
				return new ColorText(new string[] { "Floor has no ", $"{type} Items", "." }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
			for (int i = pageLength * page; i < pageLength * (page + 1); i++) {
				if (i >= items.Count)
					continue;
				retStr.Add($"{itemIds[i]}");
				retClr.Add(ConsoleColor.Cyan);
				retStr.Add(": ");
				retClr.Add(ConsoleColor.White);
				ColorText left = items[i].ListInfo(true);
				foreach (string str in left.Lines)
					retStr.Add(str);
				foreach (ConsoleColor clr in left.Colors)
					retClr.Add(clr);
				retStr.Add(items[i].SubType);
				retClr.Add(ConsoleColor.Yellow);
				ColorText right = items[i].ListInfo(false);
				foreach (string str in right.Lines)
					retStr.Add(str);
				foreach (ConsoleColor clr in right.Colors)
					retClr.Add(clr);
				retStr.Add("\n");
				retClr.Add(ConsoleColor.White);
			}
			return new ColorText(retStr.ToArray(), retClr.ToArray());
		}

		/// <summary>
		/// Lists all Items on floor
		/// </summary>
		/// <param name="floor">Index of floor in house</param>
		/// <returns>ColorText object containing list of all Items on floor</returns>
		public ColorText List(int floor) => List(floor, 0, Floors[floor].Size, "*", Floors[floor].Size, 0);

		/// <summary>
		/// Lists all Items of type on floor
		/// </summary>
		/// <param name="floor">Index of floor in house</param>
		/// <param name="type">Item type to find</param>
		/// <returns>ColorText object containing list of all type Items on floor</returns>
		public ColorText List(int floor, string type) => List(floor, 0, Floors[floor].Size, type, Floors[floor].Size, 0);

		/// <summary>
		/// Adds an Item to floor
		/// </summary>
		/// <param name="floor">Index of floor in house</param>
		/// <param name="item">Item object</param>
		/// <returns>True if Item was added to the floor, False if Item was already on the floor</returns>
		public bool AddItem(int floor, IItem item) {
			bool check = floor >= 0 && floor < Size;
			if (check)
				Floors[floor].AddItem(item);
			return check;
		}

		/// <summary>
		/// Gets Item from floor
		/// </summary>
		/// <param name="floor">Index of floor in house</param>
		/// <param name="item">Index of Item on floor in house</param>
		/// <returns>Item object from floor in house</returns>
		public IItem GetItem(int floor, int item) => Floors[floor].GetItem(item);

		/// <summary>
		/// Gets sub Item from floor
		/// </summary>
		/// <param name="floor">Index of floor in house</param>
		/// <param name="item">Index of Item on floor in house</param>
		/// <param name="subItem">Index of sub Item in Item on floor in house</param>
		/// <returns>Item object from Item from floor in house</returns>
		public IItem GetItem(int floor, int item, int subItem) => Floors[floor].GetItem(item, subItem);

		/// <summary>
		/// Gets a floor from the house
		/// </summary>
		/// <param name="floor">Index of floor in house</param>
		/// <returns>Floor object from house</returns>
		public Floor GetFloor(int floor) => Floors[floor];

		/// <summary>
		/// ToString override showing info about the house
		/// </summary>
		/// <returns>string containing the color of the house, and how many floors it has</returns>
		public override string ToString() => $"Color: {colors[Color]}\nFloors: {Size}";

		/// <summary>
		/// Creates a white house, with 1 floor
		/// </summary>
		public House() : this(0, 1) { }

		/// <summary>
		/// Creates a house with a set color, and floor count
		/// </summary>
		/// <param name="color">Index of color for house</param>
		/// <param name="floor">floor count</param>
		public House(int color, int floor) {
			Color = color >= 0 && color <= 9 ? color : 0;
			Floors = new Floor[floor];
			InitializeFloors();
		}

		/// <summary>
		/// Creates a house with a set color, and an array of floors
		/// </summary>
		/// <param name="color">Index of color for house</param>
		/// <param name="floors">Array of floor objects</param>
		public House(int color, Floor[] floors) {
			Color = color >= 0 && color <= 9 ? color : 0;
			Floors = floors;
		}
	}
}
