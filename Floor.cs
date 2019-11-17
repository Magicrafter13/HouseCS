using HouseCS.ConsoleUtils;
using HouseCS.Items;
using HouseCS.Items.Containers;
using System;
using System.Collections.Generic;

namespace HouseCS {
	/// <summary>
	/// A floor contains items, and is necessary for the program to function, but usually handled by the house
	/// </summary>
	public class Floor {
		/// <summary>
		/// Creates a floor with an empty List of Items, empty list of room id's, the lights off, and a single room named "Room"
		/// </summary>
		public Floor() : this(new List<IItem>(), new List<int>(), false, new List<string>() { "Room" }) { }

		/// <summary>
		/// Creates a floor containing 'i' items, in 'r' rooms, with lights in state 'l', and rooms named 'roomNames'
		/// </summary>
		/// <param name="i">List of Items for the floor</param>
		/// <param name="r">List of Room ID's for the Items</param>
		/// <param name="l">Sets state of lights, true = on, false = off</param>
		/// <param name="roomNames">Names of each room on the floor</param>
		public Floor(List<IItem> i, List<int> r, bool l, List<string> roomNames) {
			Items = i ?? throw new ArgumentNullException(nameof(i));
			Room = r ?? throw new ArgumentNullException(nameof(r));
			if (i.Count != r.Count) {
				Room = new List<int>(new int[Items.Count]);
				for (int i1 = 0; i1 < Room.Count; i1++)
					Room[i1] = -1;
			}
			Lights = l;
			RoomNames = roomNames ?? throw new ArgumentNullException(nameof(roomNames));
		}

		/// <summary>
		/// Creates a floor with a set List of Items, and the lights off
		/// </summary>
		/// <param name="i">List of Items for the floor</param>
		[Obsolete("This Constructor is outdated, please use Floor(List<IItem>, List<int>, bool, List<string>), or Floor()")]
		public Floor(List<IItem> i) : this(i, new List<int>(new int[i.Count + 1]), false, new List<string>()) { }

		/// <summary>
		/// Creates a floor with the lights on or off
		/// </summary>
		/// <param name="l">Lights, true = on, false = off</param>
		[Obsolete("This Constructor is outdated, please use Floor(List<IItem>, List<int>, bool, List<string>), or Floor()")]
		public Floor(bool l) : this(new List<IItem>(), new List<int>(new int[] { -1 }), l, new List<string>()) { }

		/// <summary>
		/// Creates a floor with set rooms, and no Items, with lights off
		/// </summary>
		/// <param name="rooms">Room names</param>
		[Obsolete("This Constructor is outdated, please use Floor(List<IItem>, List<int>, bool, List<string>), or Floor()")]
		public Floor(List<string> rooms) : this(new List<IItem>(), new List<int>(new int[] { -1 }), false, rooms) { }

		/// <summary>
		/// Creates a floor with Items, set lights, and no rooms
		/// </summary>
		/// <param name="items">Items on floor</param>
		/// <param name="lights">Whether lights are on or off</param>
		[Obsolete("This Constructor is outdated, please use Floor(List<IItem>, List<int>, bool, List<string>), or Floor()")]
		public Floor(List<IItem> items, bool lights) : this(items, new List<int>(new int[items.Count + 1]), lights, new List<string>()) { }

		/// <summary>
		/// Creates a floor with Items, rooms, and lights off
		/// </summary>
		/// <param name="items">Items on floor</param>
		/// <param name="rooms">Rooms on floor</param>
		[Obsolete("This Constructor is outdated, please use Floor(List<IItem>, List<int>, bool, List<string>), or Floor()")]
		public Floor(List<IItem> items, List<string> rooms) : this(items, new List<int>(new int[items.Count + 1]), false, rooms) { }

		/// <summary>
		/// Creates a floor with no Items, rooms, and set lights
		/// </summary>
		/// <param name="lights">Lights, true = on, false = off</param>
		/// <param name="rooms">Rooms on floor</param>
		[Obsolete("This Constructor is outdated, please use Floor(List<IItem>, List<int>, bool, List<string>), or Floor()")]
		public Floor(bool lights, List<string> rooms) : this(new List<IItem>(), new List<int>(new int[] { -1 }), lights, rooms) { }

		/// <summary> How many items are on the floor </summary>
		public int Size => Items.Count;

		/// <summary>
		/// Rooms on the floor
		/// </summary>
		public List<string> RoomNames { get; private set; }

		/// <summary> Items on the floor </summary>
		public List<IItem> Items { get; }

		/// <summary> Room id's of the floor's items </summary>
		public List<int> Room { get; }

		/// <summary> Whether or not the lights are turned on </summary>
		public bool Lights { get; private set; }

		/// <summary>
		/// Searches floor for items matching the keywords
		/// </summary>
		/// <param name="room">Room to search</param>
		/// <param name="itemType">Item type to search for</param>
		/// <param name="keywords">Keywords to search for</param>
		/// <returns>ColorText object of items found</returns>
		public List<ColorText> Search(int room, string itemType, List<string> keywords) {
			if (itemType is null)
				throw new ArgumentNullException(nameof(itemType));
			if (keywords is null)
				throw new ArgumentNullException(nameof(keywords));
			List<ColorText> output = new List<ColorText>();
			List<IItem>[] sortedItems = new List<IItem>[RoomNames.Count + 1];
			for (int l = 0; l < sortedItems.Length; l++)
				sortedItems[l] = new List<IItem>();
			for (int i = 0; i < Items.Count; i++)
				if (room == -2 || room == Room[i])
					sortedItems[Room[i] + 1].Add(Items[i]);
			for (int r = 0; r < sortedItems.Length; r++) {
				List<ColorText> tempSearch = new List<ColorText> { new ColorText($"  Room {r - 1}:\n") };
				foreach (IItem item in sortedItems[r]) {
					if (string.IsNullOrEmpty(itemType) || item.Type.Equals(itemType, StringComparison.OrdinalIgnoreCase)) {
						List<ColorText> temp = item.Search(keywords);
						if (temp.Count != 0) {
							tempSearch.Add(new ColorText(new string[] { $"    {Items.IndexOf(item)}", ": " }, new ConsoleColor[] { ConsoleColor.Cyan, ConsoleColor.White }));
							tempSearch.AddRange(temp);
							tempSearch.Add(new ColorText("\n"));
						}
					}
				}
				if (tempSearch.Count > 1) output.AddRange(tempSearch);
			}
			if (output.Count != 0) output.Add(new ColorText("\n"));
			return output;
		}

		/// <summary>
		/// Adds a room to the floor
		/// </summary>
		/// <param name="room">Room name</param>
		public void AddRoom(string room) => RoomNames.Add(room is null ? throw new ArgumentNullException(nameof(room)) : room);

		/// <summary>
		/// Removes a room from the Floor
		/// </summary>
		/// <param name="room">Room to remove</param>
		/// <param name="deleteItems">False moves items from this room to the hall</param>
		/// <returns>Output of command</returns>
		public string RemoveRoom(int room, bool deleteItems) {
			string output = $"Room {room}, {RoomNames[room]} removed.\n";
			if (room < 0 || room >= RoomNames.Count)
				return "Room doesn't exist.\n";
			RoomNames.RemoveAt(room);
			for (int i = 0; i < Items.Count; i++) {
				if (Room[i] == room) {
					if (deleteItems) {
						output += $"{Items[i].ListInfo(true).GetLines()[0]}{Items[i].SubType}{Items[i].ListInfo(false).GetLines()[0]} removed.\n";
						Items.RemoveAt(i);
						Room.RemoveAt(i);
						i--;
					}
					else if (Room[i] == room)
						Room[i] = -1;
				}
				else if (Room[i] > room)
					Room[i]--;
			}
			return $"{output}\n";
		}

		/// <summary>
		/// Exports item information
		/// </summary>
		/// <param name="floor">Floor to export</param>
		/// <returns>Copyable constructors for each item</returns>
		public string Export(int floor) => Export(floor, -2);

		/// <summary>
		/// Exports item information
		/// </summary>
		/// <param name="floor">Floor to export</param>
		/// <param name="roomID">Room to export</param>
		/// <returns>Copyable constructors for each item</returns>
		public string Export(int floor, int roomID) {
			string retStr = $"  Floor {floor}\n    {(roomID == -2 ? $"Room Names = {{ \"{RoomNames[0]}\"" : $"Room = {{ {RoomNames[roomID]}")}";
			if (roomID == -2)
				for (int i = 1; i < RoomNames.Count; i++)
					retStr += $", \"{RoomNames[i]}\"";
			retStr += " }\n";
			for (int i = 0; i < Items.Count; i++) {
				if (roomID == -2 || Room[i] == roomID) {
					if (Items[i] is Container) {
						retStr += Items[i].SubType switch
						{
							"Bookshelf" => ((Bookshelf)Items[i]).Export(4),
							"Dresser" => ((Dresser)Items[i]).Export(4),
							"Fridge" => ((Fridge)Items[i]).Export(4),
							"Table" => ((Table)Items[i]).Export(4),
							_ => ((Container)Items[i]).Export(4),
						};
						continue;
					}
					if (Items[i] is Display) {
						retStr += ((Display)Items[i]).Export(4);
						continue;
					}
					retStr += $"    {Items[i].Export()}\n";
				}
			}
			return $"{retStr}  End Floor {floor}{(roomID > -2 ? $", Room {roomID}" : "")}\n";
		}

		/// <summary>
		/// Toggles whether or not lights are on
		/// </summary>
		/// <returns>ColorText object stating new status of lights</returns>
		public ColorText ToggleLights() {
			Lights = Lights ? false : true;
			return new ColorText($"Lights turned {(Lights ? "on" : "off")}.");
		}

		/// <summary>
		/// Adds an item to the floor
		/// </summary>
		/// <param name="i">Item to add</param>
		/// <param name="roomID">Room to add the item to</param>
		public void AddItem(IItem i, int roomID) {
			Items.Add(i ?? throw new ArgumentNullException(nameof(i)));
			Room.Add(roomID >= RoomNames.Count || roomID < -1 ? -1 : roomID);
		}

		/// <summary>
		/// Removes item i from the floor
		/// </summary>
		/// <param name="i">Item to remove</param>
		public void RemoveItem(int i) {
			if (i < 0 || i >= Items.Count)
				return;
			Items.RemoveAt(i);
			Room.RemoveAt(i);
		}

		/// <summary>
		/// Removes item i from the floor
		/// </summary>
		/// <param name="i">Item to remove</param>
		public void RemoveItem(IItem i) {
			if (i is null)
				throw new ArgumentNullException(nameof(i));
			Room.RemoveAt(Items.IndexOf(i));
			Items.Remove(i);
		}

		/// <summary>
		/// Removes item 'sIN' from item 'iN'
		/// </summary>
		/// <param name="iN">Item containing item to remove</param>
		/// <param name="sIN">Item to remove</param>
		/// <returns>True if successful, False if item doesn't contain sub-items, or sub-item doesn't exist</returns>
		public bool RemoveItem(int iN, int sIN) {
			int removeFromFloor = -1;
			if (iN < 0 || iN >= Items.Count)
				return false;
			for (int i = 0; i < Items.Count; i++) {
				switch (Items[iN].Type) {
					case "Container":
					case "Bookshelf":
					case "Fridge":
						if (((Container)Items[iN]).GetItem(sIN) == Items[i])
							removeFromFloor = i;
						break;
					case "Display":
						if (((Display)Items[iN]).GetDevice(sIN) == Items[i])
							removeFromFloor = i;
						break;
				}
			}
			switch (Items[iN].Type) {
				case "Container":
					((Container)Items[iN]).RemoveItem(sIN);
					break;
				case "Display":
					((Display)Items[iN]).Disconnect(sIN);
					break;
				default:
					return false;
			}
			if (removeFromFloor > -1)
				Items.RemoveAt(removeFromFloor);
			return true;
		}

		/// <summary>
		/// Gets item i from floor
		/// </summary>
		/// <param name="i">Item to retrieve</param>
		/// <returns>Requested item if it exists, otherwise returns new Empty</returns>
		public IItem GetItem(int i) => i >= 0 && i < Items.Count ? Items[i] : new Empty();

		/// <summary>
		/// Gets item 'sI' from item 'i' from floor
		/// </summary>
		/// <param name="i">Item containing item to retrieve</param>
		/// <param name="sI">Item to retrieve</param>
		/// <returns>Requested item if it exists, otherwise returns new Empty</returns>
		public IItem GetItem(int i, int sI) {
			if (i >= 0 && i < Items.Count) {
				IItem ret_val = Items[i];
				if (Items[i] is Container)
					ret_val = ((Container)Items[i]).GetItem(sI);
				if (Items[i] is Display)
					ret_val = ((Display)Items[i]).GetDevice(sI);
				return ret_val;
			}
			return new Empty();
		}

		/// <summary>
		/// ToString override
		/// </summary>
		/// <returns>Amount of items on floor</returns>
		public override string ToString() => $"This floor has {Items.Count} items on it.";
	}
}
