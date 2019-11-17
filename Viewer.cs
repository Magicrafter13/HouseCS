using HouseCS.ConsoleUtils;
using HouseCS.Items;
using HouseCS.Items.Containers;
using System;
using System.Collections.Generic;

namespace HouseCS {
	/// <summary>
	/// A viewer contains a reference to one house, and has methods to act upon the house, or otherwise interact with it. At least one viewer is REQUIRED for the program to function.
	/// </summary>
	public class Viewer {
		/// <summary> Item cache for viewer </summary>
		public IItem curItem;

		/// <summary>
		/// Creates a viewer containing a new empty house
		/// </summary>
		public Viewer() : this(new House()) { }

		/// <summary>
		/// Creates a viewer containing the provided house
		/// </summary>
		/// <param name="house">House object for current house</param>
		public Viewer(House house) {
			CurFloor = 0;
			curItem = new Empty();
			CurHouse = house;
		}

		/// <summary> This viewer's house </summary>
		public House CurHouse { get; private set; }

		/// <summary> Viewer's current floor </summary>
		public int CurFloor { get; private set; }

		/// <summary>
		/// Viewer's current room
		/// <para>
		/// -1 means the viewer is not in a room (or "in the hall")
		/// </para>
		/// <para>
		/// 0 or higher, represents the actual room number
		/// </para>
		/// </summary>
		public int CurRoom { get; private set; }

		/// <summary> Current floor's item count </summary>
		public int FloorSize => GetFloor().Size;

		/// <summary> Names of each room on current floor </summary>
		public List<string> RoomNames => GetFloor().RoomNames;

		/// <summary>
		/// Changes current floor
		/// </summary>
		/// <param name="floor">Destination floor</param>
		/// <returns>True if current floor was changed, False if it wasn't, due to invalid input</returns>
		public bool GoFloor(int floor) {
			bool check = floor >= 0 && floor < CurHouse.Size;
			if (check)
				CurFloor = floor;
			return check;
		}

		/// <summary>
		/// Ascends one floor
		/// </summary>
		/// <returns>Message stating new floor or warning</returns>
		public string GoUp() => GoFloor(CurFloor + 1) ? $"\nWelcome to floor {CurFloor}.\n" : "\nYou are currently on the top floor, floor unchanged.\n";

		/// <summary>
		/// Descend one floor
		/// </summary>
		/// <returns>Message stating new floor or warning</returns>
		public string GoDown() => GoFloor(CurFloor - 1) ? $"\nWelcome to floor {CurFloor}.\n" : "\nYou are currently on the bottom floor, floor unchanged.\n";

		/// <summary>
		/// Changes viewers current room
		/// </summary>
		/// <param name="room">Destination room</param>
		/// <returns>Status code of what happened</returns>
		public int GoRoom(int room) {
			if (room < -1)
				return 1;
			if (room >= RoomNames.Count)
				return 2;
			CurRoom = room;
			if (room == -1)
				return 3;
			return 0;
		}

		/// <summary>
		/// Gets the current floor of the current house
		/// </summary>
		/// <returns>Current Floor</returns>
		public Floor GetFloor() => GetFloor(CurFloor);

		/// <summary>
		/// Gets floor 'floor' of the current house
		/// </summary>
		/// <param name="floor">Number of floor you want</param>
		/// <returns>Requested floor (if it exists)</returns>
		public Floor GetFloor(int floor) => CurHouse.GetFloor(floor);

		/// <summary>
		/// Gets Item 'item' from current floor of current house
		/// </summary>
		/// <param name="item">Number of item you want</param>
		/// <returns>Item requested</returns>
		public IItem GetItem(int item) => CurHouse.GetItem(CurFloor, item);

		/// <summary>
		/// Gets Item's SubItem from current floor of current house
		/// </summary>
		/// <param name="item">Number of containing item</param>
		/// <param name="subItem">Number of subitem</param>
		/// <returns></returns>
		public IItem GetItem(int item, int subItem) => CurHouse.GetItem(CurFloor, item, subItem);

		/// <summary>
		/// Adds item to current floor of current house
		/// </summary>
		/// <param name="item">Item object to add</param>
		public void AddItem(IItem item) => CurHouse.AddItem(CurFloor, item, CurRoom);

		/// <summary>
		/// Removes item from current floor of current house
		/// </summary>
		/// <param name="item">Item object to remove</param>
		public void RemoveItem(IItem item) {
			IItem temp = item;
			if (temp == curItem)
				curItem = new Empty();
			if (temp.HasItem(curItem))
				if (!(temp is Display))
					curItem = new Empty();
			//any Item that can have this item will have it removed
			foreach (Floor f in CurHouse.Floors) {
				foreach (IItem i in f.Items) {
					if (i.HasItem(temp)) {
						switch (i.Type) {
							case "Container":
								((Container)i).RemoveItem(temp);
								break;
							case "Display":
								((Display)i).Disconnect(temp);
								break;
						}
					}
				}
			}
			GetFloor().RemoveItem(item);
		}

		/// <summary>
		/// Removes item 'sIN' from item 'iN'from the current floor of the current house
		/// </summary>
		/// <param name="iN">Parent item (containing)</param>
		/// <param name="sIN">Sub item</param>
		public void RemoveItem(int iN, int sIN) {
			IItem temp = GetFloor().GetItem(iN, sIN);
			if (temp == curItem)
				curItem = new Empty();
			if (temp.HasItem(curItem))
				if (!(temp is Display))
					curItem = new Empty();
			//any Item that can have this item will have it removed - currently no sub items have their own sub items
			if (GetFloor().RemoveItem(iN, sIN)) {
				foreach (Floor f in CurHouse.Floors) {
					foreach (IItem i in f.Items) {
						if (i.HasItem(temp)) {
							switch (i.Type) {
								case "Container":
									((Container)i).RemoveItem(temp);
									break;
								case "Display":
									((Display)i).Disconnect(temp);
									break;
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Removes item from current floor of current house
		/// </summary>
		/// <param name="item">Index of Item to remove</param>
		public void RemoveItem(int item) => RemoveItem(GetFloor().GetItem(item));

		/// <summary>
		/// Checks if item is on the current floor of the current house
		/// </summary>
		/// <param name="item">Item to test for</param>
		/// <returns>True if item exists, false if not</returns>
		public bool IsItem(int item) => item >= 0 && item < FloorSize;

		/// <summary>
		/// Shows cached item
		/// </summary>
		/// <returns>ColorText object of current Item</returns>
		public ColorText ViewCurItem() {
			List<string> retStr = new List<string>() { $"Object type is: {curItem.SubType}\nNamed: {curItem.Name}\n\n" };
			List<ConsoleColor> retClr = new List<ConsoleColor>() { ConsoleColor.White };
			ColorText itm = curItem.ToText();
			foreach (string str in itm.GetLines())
				retStr.Add(str);
			foreach (ConsoleColor clr in itm.Colors())
				retClr.Add(clr);
			return new ColorText(retStr.ToArray(), retClr.ToArray());
		}

		/// <summary>
		/// Sets the house of the viewer
		/// </summary>
		/// <param name="house">New house for viewer</param>
		[Obsolete("Method is outdated. Please consider using different viewers for different houses!")]
		public void ChangeHouseFocus(House house) {
			CurFloor = 0;
			curItem = new Empty();
			CurHouse = house;
		}

		/// <summary>
		/// Sets viewer's cached item
		/// </summary>
		/// <param name="item">Item to cache</param>
		/// <returns>True if item exists, False if not</returns>
		public bool ChangeItemFocus(int item) {
			bool check = item >= 0 && item < CurHouse.GetFloor(CurFloor).Size;
			if (check)
				curItem = GetItem(item);
			return check;
		}

		/// <summary>
		/// Sets viewer's cached item
		/// </summary>
		/// <param name="item">Item containing item to cache</param>
		/// <param name="subItem">Item to cache</param>
		/// <returns>0 if sub-item exists, 1 if not, and 2 if item doesn't exist</returns>
		public int ChangeItemFocus(int item, int subItem) {
			if (item >= 0 && item < CurHouse.GetFloor(CurFloor).Size) {
				curItem = GetItem(item, subItem);
				return curItem is Empty ? 1 : 0;
			}
			return 2;
		}

		/// <summary>
		/// Lists items on the current floor of the current house
		/// </summary>
		/// <param name="start">Index of beginning of range (inclusive)</param>
		/// <param name="end">Index of end of range (exclusive)</param>
		/// <param name="type">Type of item to search for ("*" for all)</param>
		/// <param name="length">Maximum amount of items to return</param>
		/// <param name="page">Which page<para>For example: if you only want 20 items on screen at once, for the first 20 items the page would be 0, but if you need items 20-39, the page would be 1</para></param>
		/// <param name="room">Room containing the items wanted<para>use -2 for all rooms, and -1 for items not in a room</para></param>
		/// <returns>ColorText object of list of designated items</returns>
		public ColorText List(int start, int end, string type, int length, int page, int room) => CurHouse.List(CurFloor, start, end, type, length, page, room);

		/// <summary>
		/// How many pages a list of items will require
		/// </summary>
		/// <param name="start">Index of beginning of range</param>
		/// <param name="end">Index of end of range</param>
		/// <param name="type">Type of item to search for ("*" for all)</param>
		/// <param name="length">Maximum amount of items to return</param>
		/// <param name="room">Room containing the items wanted<para>use -2 for all rooms, and -1 for items not in a room</para></param>
		/// <returns>Total amount of pages that will be required to list the specified items</returns>
		public int PageCount(int start, int end, string type, int length, int room) => CurHouse.PageCount(CurFloor, start, end, type, length, room);

		/// <summary>
		/// Searches house for items matching specified keywords
		/// </summary>
		/// <param name="floor">Floor to search</param>
		/// <param name="room">Room to search</param>
		/// <param name="item">Item type to search for</param>
		/// <param name="keywords">Keywords to search for</param>
		/// <returns>ColorText object of items found to be matching the keywords</returns>
		public List<ColorText> Search(int floor, int room, string item, List<string> keywords) => CurHouse.Search(room != -2 && floor == -1 ? CurFloor : floor, room, item, keywords);

		/// <summary>
		/// ToString override showing information about viewer
		/// </summary>
		/// <returns>Current house id, current floor, current room + name, current item type</returns>
		public override string ToString() => $"\tCurrent House: {Program.enviVar[2, 1]}\n\tCurrent Floor: {CurFloor}\n\tCurrent Room: {CurRoom}, {(CurRoom > -1 ? GetFloor().RoomNames[CurRoom] : "hall")}\n\tCurrent Item Type: {curItem.Type}";
	}
}
