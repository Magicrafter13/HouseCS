using System;
using System.Collections.Generic;
using HouseCS.ConsoleUtils;
using HouseCS.Items;
using HouseCS.Items.Containers;

namespace HouseCS {
	/// <summary>
	/// Object for interfacing with a House object.
	/// </summary>
	public class Viewer {
		/// <summary>
		/// Item cache for viewer
		/// </summary>
		public IItem curItem;

		/// <summary>
		/// Room the viewer is stationed in
		/// </summary>
		public int CurRoom { get; private set; }

		/// <summary>
		/// House cache for viewer
		/// </summary>
		public House CurHouse { get; private set; }

		/// <summary>
		/// How many Items are on the current floor
		/// </summary>
		public int FloorSize => CurHouse.GetFloor(CurFloor).Size;

		/// <summary>
		/// Index of current floor for current house
		/// </summary>
		public int CurFloor { get; private set; }

		/// <summary>
		/// Names of rooms on current floor of current house
		/// </summary>
		public List<string> RoomNames => CurHouse.GetFloor(CurFloor).RoomNames;

		/// <summary>
		/// Searches house for items matching certain keywords
		/// </summary>
		/// <param name="floor">Floor to search</param>
		/// <param name="room">Room to search</param>
		/// <param name="item">Item type to search for</param>
		/// <param name="keywords">Keywords to search for</param>
		/// <returns>String output of Items found</returns>
		public string Search(int floor, int room, string item, List<string> keywords) {
			return CurHouse.Search(floor, room, item, keywords);
		}

		/// <summary>
		/// Changes viewers current room
		/// </summary>
		/// <param name="room">Destination room</param>
		/// <returns>Status code of what happened</returns>
		public int GoRoom(int room) {
			if (room < -1)
				return 1;
			List<string> rooms = RoomNames;
			if (room >= rooms.Count)
				return 2;
			CurRoom = room;
			if (room == -1)
				return 3;
			return 0;
		}

		/// <summary>
		/// Tells you if index of Item is valid
		/// </summary>
		/// <param name="item">Index of Item on current floor of current house</param>
		/// <returns>True if item is a valid index, false otherwise</returns>
		public bool IsItem(int item) => item >= 0 && item < CurHouse.GetFloor(CurFloor).Size;

		/// <summary>
		/// Gets Item from current floor of current house
		/// </summary>
		/// <param name="item">Index of Item on current floor of current house</param>
		/// <returns>Item object from current floor of current house</returns>
		public IItem GetItem(int item) => CurHouse.GetItem(CurFloor, item);

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
		/// Shows cached Item
		/// </summary>
		/// <returns>ColorText object of current Item</returns>
		public ColorText ViewCurItem() {
			List<string> retStr = new List<string>() { $"Object type is: {curItem.Type}\n\n" };
			List<ConsoleColor> retClr = new List<ConsoleColor>() { ConsoleColor.White };
			ColorText itm = curItem.ToText();
			foreach (string str in itm.Lines)
				retStr.Add(str);
			foreach (ConsoleColor clr in itm.Colors)
				retClr.Add(clr);
			return new ColorText(retStr.ToArray(), retClr.ToArray());
		}

		/// <summary>
		/// Gets the amount of pages a list will take up
		/// </summary>
		/// <param name="start">Index of start Item</param>
		/// <param name="end">Index of end Item</param>
		/// <param name="type">string of Item type to find</param>
		/// <param name="length">Length of list pages</param>
		/// <returns>int count of how many pages the list will use</returns>
		public int PageCount(int start, int end, string type, int length) => CurHouse.PageCount(CurFloor, start, end, type, length);

		/// <summary>
		/// Lists Items on the current floor of the current house
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="type"></param>
		/// <param name="length"></param>
		/// <param name="page"></param>
		/// <param name="room">Room for listing</param>
		/// <returns>ColorText object of list of Items</returns>
		public ColorText List(int start, int end, string type, int length, int page, int room) => CurHouse.List(CurFloor, start, end, type, length, page, room);

		/*/// <summary>
		/// Lists all Items on current floor of current house
		/// </summary>
		/// <returns>ColorText object of list of all Items</returns>
		public ColorText List() => CurHouse.List(CurFloor);

		/// <summary>
		/// Lists range of Items on current floor of current house
		/// </summary>
		/// <param name="start">Index of start Item</param>
		/// <param name="end">Index of end Item</param>
		/// <returns>ColorText object of range list of Items</returns>
		public ColorText List(int start, int end) => CurHouse.List(CurFloor, start, end, "*", FloorSize, 0);

		/// <summary>
		/// Lists type Items on current floor of current house
		/// </summary>
		/// <param name="type">Item type to find</param>
		/// <returns>ColorText object of list of type Items</returns>
		public ColorText List(string type) => CurHouse.List(CurFloor, type);*/

		/// <summary>
		/// Adds Item to current floor of current house
		/// </summary>
		/// <param name="item">Item object to add</param>
		public void AddItem(IItem item) => CurHouse.AddItem(CurFloor, item);

		/// <summary>
		/// Removes sub Item from Item on current floor of current house
		/// </summary>
		/// <param name="iN">Index of Parent Item</param>
		/// <param name="sIN">Index of sub Item</param>
		public void RemoveItem(int iN, int sIN) {
			IItem temp = CurHouse.GetFloor(CurFloor).GetItem(iN, sIN);
			if (temp == curItem)
				curItem = new Empty();
			if (temp.HasItem(curItem))
				if (!(temp is Display))
					curItem = new Empty();
			//any Item that can have this item will have it removed - currently no sub items have their own sub items
			if (CurHouse.GetFloor(CurFloor).RemoveItem(iN, sIN)) {
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
		/// Removes Item from current floor of current house
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
			CurHouse.GetFloor(CurFloor).RemoveItem(item);
		}

		/// <summary>
		/// Removes Item from current floor of current house
		/// </summary>
		/// <param name="item">Index of Item to remove</param>
		public void RemoveItem(int item) => RemoveItem(CurHouse.GetFloor(CurFloor).GetItem(item));

		/// <summary>
		/// Ascends one floor
		/// </summary>
		/// <returns>Message stating new floor or warning</returns>
		public string GoUp() {
			CurFloor++;
			if (CurFloor < CurHouse.Size)
				return $"\nWelcome to floor {CurFloor}.\n";
			CurFloor--;
			return "\nYou are currently on the top floor, floor unchanged.\n";
		}

		/// <summary>
		/// Descend one floor
		/// </summary>
		/// <returns>Message stating new floor or warning</returns>
		public string GoDown() {
			if (CurFloor <= 0)
				return "\nYou are currently on the bottom floor, floor unchanged.\n";
			CurFloor--;
			return $"\nWelcome to floor {CurFloor}.\n";
		}

		/// <summary>
		/// Changes current Item (cached Item)
		/// </summary>
		/// <param name="item">Index of Item to cache</param>
		/// <returns>True if Item exists, False if not</returns>
		public bool ChangeItemFocus(int item) {
			bool check = item >= 0 && item < CurHouse.GetFloor(CurFloor).Size;
			if (check)
				curItem = CurHouse.GetItem(CurFloor, item);
			return check;
		}

		/// <summary>
		/// Changes current Item (cached Item)
		/// </summary>
		/// <param name="item">Index of parent Item</param>
		/// <param name="subItem">Index of sub Item in parent Item</param>
		/// <returns>True if Item exists, False if not</returns>
		public int ChangeItemFocus(int item, int subItem) {
			if (item >= 0 && item < CurHouse.GetFloor(CurFloor).Size) {
				curItem = CurHouse.GetItem(CurFloor, item, subItem);
				return curItem is Empty ? 1 : 0;
			}
			return 2;
		}

		/// <summary>
		/// Changes current house (cached house)
		/// </summary>
		/// <param name="house">House object</param>
		public void ChangeHouseFocus(House house) {
			CurFloor = 0;
			curItem = new Empty();
			CurHouse = house;
		}

		/// <summary>
		/// ToString override showing status of Viewer
		/// </summary>
		/// <returns>string containing current house index, current floor index, and type of current Item</returns>
		public override string ToString() => $"\tCurrent House: {Program.house}\n\tCurrent Floor: {CurFloor}\n\tCurrent Item Type: {curItem.Type}";

		/// <summary>
		/// Creates a Viewer with a new default House
		/// </summary>
		public Viewer() : this(new House()) { }

		/// <summary>
		/// Creates a Viewer using house as the current house
		/// </summary>
		/// <param name="house">House object for current house</param>
		public Viewer(House house) {
			CurFloor = 0;
			curItem = new Empty();
			CurHouse = house;
		}
	}
}
