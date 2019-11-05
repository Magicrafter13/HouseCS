using HouseCS.ConsoleUtils;
using HouseCS.Items;
using System;
using System.Collections.Generic;

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
		public static readonly string[] types = { "*", "Bed", "Book", "Computer", "Console", "Display", "Printer",
			"Bookshelf", "Container", "Dresser", "Fridge", "Table",
			"Clothing", "Pants", "Shirt" };

		/// <summary>
		/// int of House Color (index for colors)
		/// </summary>
		private int Color { get; }

		/// <summary>
		/// How many floors the house has
		/// </summary>
		public int Size => Floors.Count;

		/// <summary>
		/// The floors of this house
		/// </summary>
		public List<Floor> Floors { get; private set; }

		/// <summary>
		/// Whether the house is on a Street, or an Avenue
		/// </summary>
		public bool Street { get; private set; }

		/// <summary>
		/// House number, 0-20
		/// </summary>
		public int HouseNumber { get; private set; }

		/// <summary>
		/// The connected road (street parallel to the house)
		/// </summary>
		public int ConRoad { get; private set; }

		/// <summary>
		/// The adjacent road (street geographically perpendicular before the house, relative to the main street)
		/// </summary>
		public int AdjRoad { get; private set; }

		/// <summary>
		/// Quadrant the house is located in, relative to the main street
		/// </summary>
		public int Quadrant { get; private set; }

		/// <summary>
		/// Searches house for items matching certain keywords
		/// </summary>
		/// <param name="floor">Floor to search</param>
		/// <param name="room">Room to search</param>
		/// <param name="item">Item type to search for</param>
		/// <param name="keywords">Keywords to search for</param>
		/// <returns>String output of Items found</returns>
		public List<ColorText> Search(int floor, int room, string item, List<string> keywords) {
			List<ColorText> output = new List<ColorText>();
			if (floor > -1) {
				List<ColorText> tmp = Floors[floor].Search(room, item, keywords);
				if (tmp.Count != 0) {
					output.Add(new ColorText($"Floor {floor}\n"));
					output.AddRange(Floors[floor].Search(room, item, keywords));
					output.Add(new ColorText("\n"));
				}
			}
			else {
				for (int flr = 0; flr < Size; flr++) {
					if (floor != -1 && floor != flr) continue;
					List<ColorText> tmp = Floors[flr].Search(room, item, keywords);
					if (tmp.Count != 0) {
						output.Add(new ColorText($"Floor {flr}\n"));
						output.AddRange(tmp);
						output.Add(new ColorText("\n"));
					}
				}
			}
			return output;
		}

		/// <summary>
		/// Exports Item information
		/// </summary>
		/// <param name="house">House number</param>
		/// <returns>String with all Items in the house</returns>
		public string Export(int house) {
			string retStr = $"House {house}\nHouse house{house} = new House({Color}, {Floors.Count}, {Street.ToString().ToLower()}, {HouseNumber}, {ConRoad}, {AdjRoad}, {Quadrant});\n";
			for (int i = 0; i < Floors.Count; i++)
				retStr += Floors[i].Export(i);
			return $"{retStr}End House {house}\n";
		}

		/// <summary>
		/// Returns page count for listing Items
		/// </summary>
		/// <param name="floor">Index of floor in house</param>
		/// <param name="rangeStart">Index of first Item on floor</param>
		/// <param name="rangeEnd">Index of last Item on floor</param>
		/// <param name="searchType">string of Item type being searched for</param>
		/// <param name="pageLength">int of how many Items are to be shown per page</param>
		/// <param name="room">Room of Items</param>
		/// <returns>How many pages a listing will take, based on Item type, range, and page length</returns>
		public int PageCount(int floor, int rangeStart, int rangeEnd, string searchType, int pageLength, int room) {
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
			if (room < -2 || room > Floors[floor].RoomNames.Count)
				return -5;
			int items = 0;
			for (int i = rangeStart; i < rangeEnd; i++) {
				if (i > Floors[floor].Size)
					continue;
				if ((searchType.Equals("*") ||
					searchType.Equals(Floors[floor].GetItem(i).SubType, StringComparison.OrdinalIgnoreCase) ||
					searchType.Equals(Floors[floor].GetItem(i).Type)) &&
					(room == -2 ||
					room == Floors[floor].Room[i])) {
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
		/// <param name="room">Room to search</param>
		/// <returns>ColorText object either containing a List of Items based on the criteria, or a message explaining what was wrong with the arguments</returns>
		public ColorText List(int floor, int start, int end, string type, int pageLength, int page, int room) {
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
			if (room < -2)
				return new ColorText(new string[] { "Room", " must be greater than or equal to ", "-2" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan });
			List<string> retStr = new List<string>() { "\n" };
			List<ConsoleColor> retClr = new List<ConsoleColor>() { ConsoleColor.White };
			List<IItem> items = new List<IItem>();
			List<int> itemIds = new List<int>();
			for (int i = start; i < end; i++) {
				if (i > Floors[floor].Size)
					continue;
				if (
					(
						type.Equals("*") ||
						type.Equals(Floors[floor].GetItem(i).Type, StringComparison.OrdinalIgnoreCase) ||
						type.Equals(Floors[floor].GetItem(i).SubType, StringComparison.OrdinalIgnoreCase)
					) && (room == -2 || Floors[floor].Room[i] == room)) {
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
		/// Adds an Item to floor
		/// </summary>
		/// <param name="floor">Index of floor in house</param>
		/// <param name="item">Item object</param>
		/// <param name="roomID">Room number that the item will be in</param>
		/// <returns>True if Item was added to the floor, False if Item was already on the floor</returns>
		public bool AddItem(int floor, IItem item, int roomID) {
			bool check = floor >= 0 && floor < Size && roomID >= -1 && roomID < Floors[floor].RoomNames.Count;
			if (check)
				Floors[floor].AddItem(item, roomID);
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
		/// Creates a new Floor in the House
		/// </summary>
		public void AddFloor() => AddFloor(new Floor());

		/// <summary>
		/// Adds a supplied Floor to House
		/// </summary>
		/// <param name="f"></param>
		public void AddFloor(Floor f) => Floors.Add(f);

		/// <summary>
		/// Removes a Floor from the House
		/// </summary>
		/// <param name="floor">Floor to remove</param>
		public void RemoveFloor(int floor) => Floors.RemoveAt(floor);

		/// <summary>
		/// Gets a floor from the house
		/// </summary>
		/// <param name="floor">Index of floor in house</param>
		/// <returns>Floor object from house</returns>
		public Floor GetFloor(int floor) => Floors[floor];

		/// <summary>
		/// Reports the House's color
		/// </summary>
		public string GetColor => colors[Color];

		/// <summary>
		/// Deciphers the address of the House
		/// </summary>
		public string Address => $"{AdjRoad}{(HouseNumber < 10 ? $"0{HouseNumber}" : $"{HouseNumber}")} {(Quadrant < 2 ? Quadrant == 0 ? "NE" : "NW" : Quadrant == 2 ? "SW" : "SE")} {Program.OrdSuf(ConRoad)} {(Street ? "St" : "Ave")}";

		/// <summary>
		/// ToString override showing info about the house
		/// </summary>
		/// <returns>string containing the color of the house, and how many floors it has</returns>
		public override string ToString() => $"Color: {GetColor}\nFloors: {Size}\n{(Quadrant == -1 ? "House has no assigned address." : $"{Address}")}";

		/// <summary>
		/// Creates a white house, with 1 floor
		/// </summary>
		public House() : this(0, 1, true, -1, -1, -1, -1) { }

		/// <summary>
		/// Creates a house with a set color, and floor count
		/// </summary>
		/// <param name="color">Index of color for house</param>
		/// <param name="floor">floor count</param>
		/// <param name="street">True = Street, False = Avenue</param>
		/// <param name="houseNumber">This House's number on the street</param>
		/// <param name="conRoad">Road parallel to House</param>
		/// <param name="adjRoad">Road adjacent to House</param>
		/// <param name="quad">Relative quadrant of House</param>
		public House(int color, int floor, bool street, int houseNumber, int conRoad, int adjRoad, int quad) : this(color, new List<Floor>(new Floor[floor]), street, houseNumber, conRoad, adjRoad, quad) { }

		/// <summary>
		/// Creates a house with a set color, and an array of floors
		/// </summary>
		/// <param name="color">Index of color for house</param>
		/// <param name="floors">Array of floor objects</param>
		/// <param name="street">True = Street, False = Avenue</param>
		/// <param name="houseNumber">This House's number of the street</param>
		/// <param name="conRoad">Road parallel to House</param>
		/// <param name="adjRoad">Road adjacent to House</param>
		/// <param name="quad">Relative quadrant of House</param>
		public House(int color, List<Floor> floors, bool street, int houseNumber, int conRoad, int adjRoad, int quad) {
			Color = color >= 0 && color < colors.Length ? color : 0;
			Floors = floors;
			Street = street;
			HouseNumber = houseNumber;
			ConRoad = conRoad;
			AdjRoad = adjRoad;
			Quadrant = quad;
		}
	}
}
