using System;
using System.Collections.Generic;
using HouseCS.ConsoleUtils;

namespace HouseCS.Items.Containers {
	/// <summary>
	/// Container, has Items
	/// </summary>
	public class Container : IItem {
		private const string typeS = "Container";

		/// <summary>
		/// Items in container
		/// </summary>
		public List<IItem> Items { get; private set; }

		/// <summary>
		/// How many Items are in the container
		/// </summary>
		public int Size => Items.Count;

		/// <summary>
		/// string of Item type
		/// </summary>
		public string Type => typeS;

		/// <summary>
		/// string of Item sub-type
		/// </summary>
		public string SubType => typeS;

		/// <summary>
		/// Gets an Item from the container
		/// </summary>
		/// <param name="item">Index of Item</param>
		/// <returns>Returns Item, or new Empty if not found</returns>
		public IItem GetItem(int item) => item < 0 || item >= Items.Count ? new Empty() : Items[item];

		/// <summary>
		/// Adds Item to container
		/// </summary>
		/// <param name="item">Item to add</param>
		/// <returns>ColorText object saying the object is now in the container, or telling the user why it can't be placed on</returns>
		public ColorText AddItem(IItem item) {
			if (item == this)
				return new ColorText("You can't put something inside itself!", ConsoleColor.White);
			if (HasItem(item))
				return new ColorText(new string[] { "That ", "Item", $" is already in this {typeS}! (I don't think this message should be able to be seen.)" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
			Items.Add(item);
			return new ColorText(new string[] { "\nItem", " added ", "to this ", typeS, ".\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.DarkBlue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
		}

		/// <summary>
		/// Remove Item by index
		/// </summary>
		/// <param name="item">Index of Item</param>
		/// <returns>ColorText object saying the Item was removed, or showing the index as being invalid</returns>
		public ColorText RemoveItem(int item) {
			if (item < 0 || item >= Items.Count) {
				List<string> retStr = new List<string>();
				List<ConsoleColor> retClr = new List<ConsoleColor>();
				retStr.Add("This ");
				retClr.Add(ConsoleColor.White);
				retStr.Add(typeS);
				retClr.Add(ConsoleColor.Yellow);
				if (Items.Count > 0) {
					retStr.Add(" only has ");
					retClr.Add(ConsoleColor.White);
					retStr.Add(Items.Count.ToString());
					retClr.Add(ConsoleColor.Cyan);
					retStr.Add(" items in it.");
					retClr.Add(ConsoleColor.White);
				} else {
					retStr.Add(" is ");
					retClr.Add(ConsoleColor.White);
					retStr.Add("Empty");
					retClr.Add(ConsoleColor.Yellow);
				}
				return new ColorText(retStr.ToArray(), retClr.ToArray());
			}
			Items.RemoveAt(item);
			return new ColorText(new string[] { "\nItem ", "removed", ".\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.DarkBlue, ConsoleColor.White });
		}

		/// <summary>
		/// Removes Item from container
		/// </summary>
		/// <param name="item">Item to remove</param>
		/// <returns>ColorText object saying the Item was removed, or that the Item isn't in the container</returns>
		public ColorText RemoveItem(IItem item) => Items.Remove(item) ? new ColorText(new string[] { "\nItem ", "removed", ".\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.DarkBlue, ConsoleColor.White }) : new ColorText(new string[] { "No matching ", "Item", " found" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });

		/// <summary>
		/// Whether or not Item is in the container
		/// </summary>
		/// <param name="item">test Item</param>
		/// <returns>True if the Item is in the container, False if it isn't</returns>
		public bool HasItem(IItem item) {
			foreach (IItem i in Items)
				if (i == item)
					return true;
			return false;
		}

		/// <summary>
		/// Minor details for list
		/// </summary>
		/// <param name="beforeNotAfter">True for left side, False for right side</param>
		/// <returns>ColorText object of minor container details</returns>
		public ColorText ListInfo(bool beforeNotAfter) => beforeNotAfter ? ColorText.Empty : Items.Count > 0 ? new ColorText(new string[] { ", ", Items.Count.ToString(), " Items" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.Yellow }) : new ColorText(new string[] { ", ", "Empty" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow });

		/// <summary>
		/// Information about container
		/// </summary>
		/// <returns>ColorText object of important info</returns>
		public ColorText ToText() {
			List<string> retStr = new List<string>() { typeS };
			List<ConsoleColor> retClr = new List<ConsoleColor>() { ConsoleColor.Yellow };
			for (int i = 0; i < Items.Count; i++) {
				retStr.Add($"\n\t{i.ToString()}");
				retClr.Add(ConsoleColor.Cyan);
				retStr.Add(": ");
				retClr.Add(ConsoleColor.White);
				ColorText left = Items[i].ListInfo(true);
				foreach (string str in left.Lines)
					retStr.Add(str);
				foreach (ConsoleColor clr in left.Colors)
					retClr.Add(clr);
				retStr.Add(Items[i].SubType);
				retClr.Add(ConsoleColor.Yellow);
				ColorText right = Items[i].ListInfo(false);
				foreach (string str in right.Lines)
					retStr.Add(str);
				foreach (ConsoleColor clr in right.Colors)
					retClr.Add(clr);
			}
			retStr.Add("\nEnd of ");
			retClr.Add(ConsoleColor.White);
			retStr.Add("Container");
			retClr.Add(ConsoleColor.Yellow);
			retStr.Add(" contents.");
			retClr.Add(ConsoleColor.White);
			return new ColorText(retStr.ToArray(), retClr.ToArray());
		}

		/// <summary>
		/// Creates an empty container
		/// </summary>
		public Container() : this(new List<IItem>()) { }

		/// <summary>
		/// Creates a container with a List of Items
		/// </summary>
		/// <param name="items"></param>
		public Container(List<IItem> items) => Items = items;
	}
}