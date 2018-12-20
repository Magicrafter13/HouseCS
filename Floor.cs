using System.Collections.Generic;
using HouseCS.ConsoleUtils;
using HouseCS.Items;
using HouseCS.Items.Containers;

namespace HouseCS {
	/// <summary>
	/// Object containing Items, and lights that are on or off.
	/// </summary>
	public class Floor {
		/// <summary>
		/// List of all Items on the floor.
		/// </summary>
		public List<IItem> Items { get; set; }

		/// <summary>
		/// Boolean representation of whether or not the lights are turned on, on this floor.
		/// </summary>
		public bool Lights { get; private set; }

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
		public void AddItem(IItem i) => Items.Add(i);

		/// <summary>
		/// Removes Item i from the floors List of Items (RemoveAt)
		/// </summary>
		/// <param name="i">Index of Item in List</param>
		public void RemoveItem(int i) => Items.RemoveAt(i);

		/// <summary>
		/// Removes Item i from the floors List of Items
		/// </summary>
		/// <param name="i">Item object</param>
		public void RemoveItem(IItem i) => Items.Remove(i);

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
		public Floor() : this(new List<IItem>(), false) { }

		/// <summary>
		/// Creates a floor with a set List of Items
		/// </summary>
		/// <param name="i">List of Items for the floor</param>
		/// <param name="l">Sets state of lights, true = on, false = off</param>
		public Floor(List<IItem> i, bool l) {
			Items = i;
			Lights = l;
		}

		/// <summary>
		/// Creates a floor with a set List of Items, and the lights off
		/// </summary>
		/// <param name="i">List of Items for the floor</param>
		public Floor(List<IItem> i) : this(i, false) { }

		/// <summary>
		/// Creates a floor with the lights on or off
		/// </summary>
		/// <param name="l">Lights, true = on, false = off</param>
		public Floor(bool l) : this(new List<IItem>(), l) { }
	}
}
