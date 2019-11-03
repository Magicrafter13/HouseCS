using HouseCS.ConsoleUtils;
using HouseCS.Items;
using HouseCS.Items.Containers;
using System;
using System.Collections.Generic;

namespace HouseCS {
	/// <summary>
	/// Object containing Items, and lights that are on or off.
	/// </summary>
	public class Floor {
		/// <summary>
		/// List of all room names on floor
		/// </summary>
		public List<string> RoomNames { get; private set; }

		/// <summary>
		/// List of all Items on the floor.
		/// </summary>
		public List<IItem> Items { get; set; }

		/// <summary>
		/// List of Room numbers for all Items on the floor
		/// </summary>
		public List<int> Room { get; set; }

		/// <summary>
		/// Boolean representation of whether or not the lights are turned on, on this floor.
		/// </summary>
		public bool Lights { get; private set; }

		/// <summary>
		/// Searches floor for items matching certain keywords
		/// </summary>
		/// <param name="room">Room to search</param>
		/// <param name="itemType">Item type to search for</param>
		/// <param name="keywords">Keywords to search for</param>
		/// <returns>String output of Items found</returns>
		public List<ColorText> Search(int room, string itemType, List<string> keywords) {
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
					if (itemType.Equals(string.Empty) || item.Type.Equals(itemType, StringComparison.OrdinalIgnoreCase)) {
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
		public void AddRoom(string room) => RoomNames.Add(room);

		/// <summary>
		/// Removes a room from the Floor
		/// </summary>
		/// <param name="room">Room to remove</param>
		/// <param name="deleteItems">False moves items from this room to the hall</param>
		/// <returns>Output of command</returns>
		public string RemoveRoom(int room, bool deleteItems) {
			string output = $"Room {room}, {RoomNames[room]} removed.\n";
			RoomNames.RemoveAt(room);
			for (int i = 0; i < Items.Count; i++) {
				if (Room[i] == room) {
					if (deleteItems) {
						output += $"{Items[i].ListInfo(true).Lines[0]}{Items[i].SubType}{Items[i].ListInfo(false).Lines[0]} removed.\n";
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
		/// Exports Item information
		/// </summary>
		/// <param name="floor">Floor number</param>
		/// <returns>String with all Items on the floor</returns>
		public string Export(int floor) => Export(floor, -2);

		/// <summary>
		/// Exports Item information
		/// </summary>
		/// <param name="floor">Floor number</param>
		/// <param name="roomID">Room number</param>
		/// <returns>String with all Items on the floor</returns>
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
		/// Toggles Boolean Lights, and returns ColorText to tell the user what state the Lights are now in.
		/// </summary>
		/// <returns>ColorText object saying if the Lights are on or off</returns>
		public ColorText ToggleLights() {
			Lights = Lights ? false : true;
			return new ColorText($"Lights turned {(Lights ? "on" : "off")}.");
		}

		/// <summary>
		/// Adds an Item to this floors Item List
		/// </summary>
		/// <param name="i">Item to add</param>
		/// <param name="roomID">Room number the item will be in</param>
		public void AddItem(IItem i, int roomID) {
			Items.Add(i);
			Room.Add((roomID >= RoomNames.Count ? -1 : roomID));
		}

		/// <summary>
		/// Removes Item i from the floors List of Items (RemoveAt)
		/// </summary>
		/// <param name="i">Index of Item in List</param>
		public void RemoveItem(int i) {
			Items.RemoveAt(i);
			Room.RemoveAt(i);
		}

		/// <summary>
		/// Removes Item i from the floors List of Items
		/// </summary>
		/// <param name="i">Item object</param>
		public void RemoveItem(IItem i) {
			Room.RemoveAt(Items.IndexOf(i));
			Items.Remove(i);
		}

		/// <summary>
		/// Removes Item sIN from Item iN (Only works with Items that contain Items)
		/// </summary>
		/// <param name="iN">int of Parent Item in floors List</param>
		/// <param name="sIN">int of Sub Item in Parent Item</param>
		/// <returns>True if successful, False if Item doesn't contain sub Items, or sub Item doesn't exist</returns>
		public bool RemoveItem(int iN, int sIN) {
			int removeFromFloor = -1;
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
		/// Gets Item i from floors Item List
		/// </summary>
		/// <param name="i">Index of Item on floors Item List</param>
		/// <returns>Requested Item if it exists, otherwise returns new Empty</returns>
		public IItem GetItem(int i) => i >= 0 && i < Items.Count ? Items[i] : new Empty();

		/// <summary>
		/// Gets Item sI from Item i from floors Item List
		/// </summary>
		/// <param name="i">Index of Parent Item on floors Item List</param>
		/// <param name="sI">Index of sub Item in Parent Item</param>
		/// <returns>Requested Item if it exists, otherwise returns new Empty</returns>
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
		/// How many Items are on the floor
		/// </summary>
		public int Size => Items.Count;

		/// <summary>
		/// ToString override
		/// </summary>
		/// <returns>string telling the user how many Items are on the floor</returns>
		public override string ToString() => $"This floor has {Items.Count} items on it.";

		/// <summary>
		/// Creates a floor with an empty List of Items, and with the lights off
		/// </summary>
		public Floor() : this(new List<IItem>(), new List<int>(), false, new List<string>() { "Room" }) { }

		/// <summary>
		/// Creates a floor with a set List of Items
		/// </summary>
		/// <param name="i">List of Items for the floor</param>
		/// <param name="r">List of Room ID's for the Items</param>
		/// <param name="l">Sets state of lights, true = on, false = off</param>
		/// <param name="roomNames">Names of each room on the floor</param>
		public Floor(List<IItem> i, List<int> r, bool l, List<string> roomNames) {
			Items = i;
			if (i.Count == r.Count)
				Room = r;
			else {
				Room = new List<int>(new int[Items.Count]);
				for (int i1 = 0; i1 < Room.Count; i1++)
					Room[i1] = -1;
			}
			Lights = l;
			RoomNames = roomNames;
		}

		/// <summary>
		/// Creates a floor with a set List of Items, and the lights off
		/// </summary>
		/// <param name="i">List of Items for the floor</param>
		public Floor(List<IItem> i) : this(i, new List<int>(new int[i.Count + 1]), false, new List<string>()) { }

		/// <summary>
		/// Creates a floor with the lights on or off
		/// </summary>
		/// <param name="l">Lights, true = on, false = off</param>
		public Floor(bool l) : this(new List<IItem>(), new List<int>(new int[] { -1 }), l, new List<string>()) { }

		/// <summary>
		/// Creates a floor with set rooms, and no Items, with lights off
		/// </summary>
		/// <param name="rooms">Room names</param>
		public Floor(List<string> rooms) : this(new List<IItem>(), new List<int>(new int[] { -1 }), false, rooms) { }

		/// <summary>
		/// Creates a floor with Items, set lights, and no rooms
		/// </summary>
		/// <param name="items">Items on floor</param>
		/// <param name="lights">Whether lights are on or off</param>
		public Floor(List<IItem> items, bool lights) : this(items, new List<int>(new int[items.Count + 1]), lights, new List<string>()) { }

		/// <summary>
		/// Creates a floor with Items, rooms, and lights off
		/// </summary>
		/// <param name="items">Items on floor</param>
		/// <param name="rooms">Rooms on floor</param>
		public Floor(List<IItem> items, List<string> rooms) : this(items, new List<int>(new int[items.Count + 1]), false, rooms) { }

		/// <summary>
		/// Creates a floor with no Items, rooms, and set lights
		/// </summary>
		/// <param name="lights">Lights, true = on, false = off</param>
		/// <param name="rooms">Rooms on floor</param>
		public Floor(bool lights, List<string> rooms) : this(new List<IItem>(), new List<int>(new int[] { -1 }), lights, rooms) { }
	}
}
