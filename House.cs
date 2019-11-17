using HouseCS.ConsoleUtils;
using HouseCS.Items;
using System;
using System.Collections.Generic;

namespace HouseCS
{
	/// <summary>
	/// Houses are a large chunk of the program. A house is where floors, containing items are stored.
	/// </summary>
	public class House
	{
		/// <summary> Possible Colors for the House </summary>
		public static readonly string[] colors = { "White", "Red", "Brown", "Orange", "Yellow", "Green", "Blue", "Purple", "Pink", "Black" };

		/// <summary> Valid Item Types </summary>
		public static readonly string[] types = { "*", "Bed", "Book", "Computer", "Console", "Display", "Printer",
			"Bookshelf", "Container", "Dresser", "Fridge", "Table",
			"Clothing", "Pants", "Shirt" };

		/// <summary>
		/// Creates a white house with 1 floor, with no set address
		/// </summary>
		public House() : this(0, 1, true, -1, -1, -1, -1) { }

		/// <summary>
		/// Creates a house, with empty floors
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
		/// Creates a house with specified floors
		/// </summary>
		/// <param name="color">Index of color for house</param>
		/// <param name="floors">Array of floor objects</param>
		/// <param name="street">True = Street, False = Avenue</param>
		/// <param name="houseNumber">This House's number of the street</param>
		/// <param name="conRoad">Road parallel to House</param>
		/// <param name="adjRoad">Road adjacent to House</param>
		/// <param name="quad">Relative quadrant of House</param>
		public House(int color, List<Floor> floors, bool street, int houseNumber, int conRoad, int adjRoad, int quad)
		{
			Color = color >= 0 && color < colors.Length ? color : 0;
			Floors = floors ?? throw new ArgumentNullException(nameof(floors));
			Street = street;
			HouseNumber = houseNumber;
			ConRoad = conRoad;
			AdjRoad = adjRoad;
			Quadrant = quad;
		}

		/// <summary> House color enum, for ease of programming </summary>
		public enum HouseColor
		{
			/// <summary> White House </summary>
			White,
			/// <summary> Red House </summary>
			Red,
			/// <summary> Brown House </summary>
			Brown,
			/// <summary> Orange House </summary>
			Orange,
			/// <summary> Yellow House </summary>
			Yellow,
			/// <summary> Green House </summary>
			Green,
			/// <summary> Blue House </summary>
			Blue,
			/// <summary> Purple House </summary>
			Purple,
			/// <summary> Pink House </summary>
			Pink,
			/// <summary> Black House </summary>
			Black
		};

		/// <summary> House's current color </summary>
		public string GetColor => colors[Color];

		/// <summary> The house's floors </summary>
		public List<Floor> Floors { get; private set; }

		/// <summary> How many floors the house has </summary>
		public int Size => Floors.Count;

		/// <summary> Whether the house is on a Street, or an Avenue </summary>
		public bool Street { get; private set; }

		/// <summary> House number, 0-Program.maxHouses </summary>
		public int HouseNumber { get; private set; }

		/// <summary> The connected road (street parallel to the house) </summary>
		public int ConRoad { get; private set; }

		/// <summary> The adjacent road (street geographically perpendicular before the house, relative to the main street) </summary>
		public int AdjRoad { get; private set; }

		/// <summary> Quadrant the house is located in, relative to the main street (like a unit circle) </summary>
		public int Quadrant { get; private set; }

		/// <summary> Deciphers the address of the House </summary>
		public string Address => Quadrant >= 0 ? $"{AdjRoad}{(HouseNumber < 10 ? $"0{HouseNumber}" : $"{HouseNumber}")} {(Quadrant < 2 ? Quadrant == 0 ? "NE" : "NW" : Quadrant == 2 ? "SW" : "SE")} {Program.OrdSuf(ConRoad)} {(Street ? "St" : "Ave")}" : "No set address.";

		/// <summary> House's current color </summary>
		private int Color { get; }

		/// <summary>
		/// Adds floor f to the house
		/// </summary>
		/// <param name="f">Floor to add</param>
		public void AddFloor(Floor f) => Floors.Add(f ?? throw new ArgumentNullException(nameof(f)));

		/// <summary>
		/// Creates a new, empty floor to the house
		/// </summary>
		public void AddFloor() => AddFloor(new Floor());

		/// <summary>
		/// Gets floor 'floor' of the house
		/// </summary>
		/// <param name="floor">Floor you want</param>
		/// <returns>Requested floor</returns>
		public Floor GetFloor(int floor) => Floors[floor];

		/// <summary>
		/// Removes a floor from the house
		/// </summary>
		/// <param name="floor">Floor to remove</param>
		public void RemoveFloor(int floor) => Floors.RemoveAt(floor);

		/// <summary>
		/// Adds item to floor, in room 'roomID'
		/// </summary>
		/// <param name="floor">Floor to add to</param>
		/// <param name="item">Item to add</param>
		/// <param name="roomID">Room to add to</param>
		/// <returns>True if item was added to the floor, False if floor doesn't exist, or room doesn't exist</returns>
		public bool AddItem(int floor, IItem item, int roomID)
		{
			if (item is null)
				throw new ArgumentNullException(nameof(item));
			bool check = roomID >= -1 && roomID < Floors[floor].RoomNames.Count;
			GetFloor(floor).AddItem(item, roomID);
			return check;
		}

		/// <summary>
		/// Gets item from floor
		/// </summary>
		/// <param name="floor">Floor to retrieve from</param>
		/// <param name="item">Item to retrieve</param>
		/// <returns>Requested item</returns>
		public IItem GetItem(int floor, int item) => GetFloor(floor).GetItem(item);

		/// <summary>
		/// Gets item from an item on floor
		/// </summary>
		/// <param name="floor">Floor to retrieve from</param>
		/// <param name="item">Item containing requested item</param>
		/// <param name="subItem">Item to retrieve</param>
		/// <returns>Requested item</returns>
		public IItem GetItem(int floor, int item, int subItem) => GetFloor(floor).GetItem(item, subItem);

		/// <summary>
		/// Searches house for items matching the keywords
		/// </summary>
		/// <param name="floor">Floor to search</param>
		/// <param name="room">Room to search</param>
		/// <param name="item">Item type to search for</param>
		/// <param name="keywords">Keywords to search for</param>
		/// <returns>ColorText object of items found</returns>
		public List<ColorText> Search(int floor, int room, string item, List<string> keywords)
		{
			if (keywords is null)
				throw new ArgumentNullException(nameof(keywords));
			List<ColorText> output = new List<ColorText>();
			if (floor > -1) {
				List<ColorText> tmp = GetFloor(floor).Search(room, item, keywords);
				if (tmp.Count != 0) {
					output.Add(new ColorText($"Floor {floor}\n"));
					output.AddRange(GetFloor(floor).Search(room, item, keywords));
					output.Add(new ColorText("\n"));
				}
			}
			else {
				for (int flr = 0; flr < Size; flr++) {
					if (floor != -1 && floor != flr) continue;
					List<ColorText> tmp = GetFloor(flr).Search(room, item, keywords);
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
		/// Exports item information
		/// </summary>
		/// <param name="house">House number</param>
		/// <returns>Copyable constructors for each item</returns>
		public string Export(int house)
		{
			string retStr = $"House {house}\nHouse house{house} = new House({Color}, {Floors.Count}, {Street.ToString().ToLower()}, {HouseNumber}, {ConRoad}, {AdjRoad}, {Quadrant});\n";
			for (int i = 0; i < Floors.Count; i++)
				retStr += Floors[i].Export(i);
			return $"{retStr}End House {house}\n";
		}

		/// <summary>
		/// Lists Items on the specified floor
		/// </summary>
		/// <param name="floor">Floor to list</param>
		/// <param name="start">Index of beginning of range (inclusive)</param>
		/// <param name="end">Index of end of range (exclusive)</param>
		/// <param name="type">Type of item to search for ("*" for all)</param>
		/// <param name="pageLength">Maximum amount of items to return</param>
		/// <param name="page">Which page<para>For example: if you only want 20 items on screen at once, for the first 20 items the page would be 0, but if you need items 20-39, the page would be 1</para></param>
		/// <param name="room">Room containing the items wanted<para>use -2 for all rooms, and -1 for items not in a room</para></param>
		/// <returns>ColorText object of list of designated items</returns>
		public ColorText List(int floor, int start, int end, string type, int pageLength, int page, int room)
		{
			bool validType = false;
			foreach (string t in types)
				if (type.Equals(t, StringComparison.OrdinalIgnoreCase))
					validType = true;
			if (!validType)
				return new ColorText(new string[] { type, " is not a valid ", "Item", " type." }, new ConsoleColor[] { ConsoleColor.DarkYellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
			if (floor < 0 || floor >= Floors.Count)
				return new ColorText(new string[] { "Floor", " must be greater than or equal to ", "0", ", and less than ", Floors.Count.ToString() }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Cyan });
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
				foreach (string str in left.GetLines())
					retStr.Add(str);
				foreach (ConsoleColor clr in left.Colors())
					retClr.Add(clr);
				retStr.Add(items[i].SubType);
				retClr.Add(ConsoleColor.Yellow);
				ColorText right = items[i].ListInfo(false);
				foreach (string str in right.GetLines())
					retStr.Add(str);
				foreach (ConsoleColor clr in right.Colors())
					retClr.Add(clr);
				retStr.Add("\n");
				retClr.Add(ConsoleColor.White);
			}
			return new ColorText(retStr.ToArray(), retClr.ToArray());
		}

		/// <summary>
		/// How many pages a list of items will require
		/// </summary>
		/// <param name="floor">Floor to list</param>
		/// <param name="rangeStart">Index of beginning of range</param>
		/// <param name="rangeEnd">Index of end of range</param>
		/// <param name="searchType">Type of item to search for ("*" for all)</param>
		/// <param name="pageLength">Maximum amount of items to return</param>
		/// <param name="room">Room containing the items wanted<para>use -2 for all rooms, and -1 for items not in a room</para></param>
		/// <returns>Total amount of pages that will be required to list the specified items</returns>
		public int PageCount(int floor, int rangeStart, int rangeEnd, string searchType, int pageLength, int room)
		{
			bool validType = false;
			foreach (string t in types)
				if (searchType.Equals(t, StringComparison.OrdinalIgnoreCase))
					validType = true;
			if (!validType)
				return -1;
			if (floor < 0 || floor >= Floors.Count)
				return -6;
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
		/// ToString override showing info about the house
		/// </summary>
		/// <returns>Color, floor count, and address</returns>
		public override string ToString() => $"Color: {GetColor}\nFloors: {Size}\n{(Quadrant == -1 ? "House has no assigned address." : $"{Address}")}";
	}
}
