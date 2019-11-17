using HouseCS.ConsoleUtils;
using System;
using System.Collections.Generic;

namespace HouseCS.Items.Containers {
	/// <summary>
	/// Containers are one of the best and most flexible items in a house. Especially when named. They can realistically be whatever you want them to be.
	/// </summary>
	public class Container : IItem {
		private const string typeS = "Container";

		/// <summary>
		/// Creates an empty container with no name
		/// </summary>
		public Container() : this(new List<IItem>(), string.Empty) { }

		/// <summary>
		/// Creates a container
		/// </summary>
		/// <param name="items">Items in the container</param>
		/// <param name="name">Name of Container</param>
		public Container(List<IItem> items, string name)
		{
			Items = items;
			Rename(name);
		}

		/// <summary>
		/// Items inside this container
		/// </summary>
		public List<IItem> Items { get; private set; }

		/// <summary>
		/// Name of container
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// How many items are in the container
		/// </summary>
		public int Size => Items.Count;

		/// <summary>
		/// string of item type
		/// </summary>
		public string Type => typeS;

		/// <summary>
		/// string of item sub-type
		/// </summary>
		public string SubType => typeS;

		/// <summary>
		/// Creates specified container
		/// </summary>
		/// <param name="type">Container type</param>
		/// <returns>Requested container type</returns>
		public static IItem Create(string type) => (type.ToLower()) switch
		{
			"fridge" => new Fridge(),
			"bookshelf" => new Bookshelf(),
			"dresser" => new Dresser(),
			"table" => new Table(),
			_ => new Container(),
		};

		/// <summary>
		/// Matches keywords against item data
		/// </summary>
		/// <param name="keywords">Keywords to search for</param>
		/// <returns>String output if keywords matched</returns>
		public List<ColorText> Search(List<string> keywords) {
			List<ColorText> output = new List<ColorText>();
			foreach (string key in keywords) {
				if ((Size == 0 &&
					key.Equals("Empty", StringComparison.OrdinalIgnoreCase)) ||
					key.Equals(Name, StringComparison.OrdinalIgnoreCase)) {
					output.Add(ListInfo(true));
					output.Add(new ColorText(typeS));
					output.Add(ListInfo(false));
				}
			}
			if (Size > 0) {
				for (int i = 0; i < Items.Count; i++) {
					List<ColorText> temp = Items[i].Search(keywords);
					if (output.Count == 0 && temp.Count > 0) output.Add(new ColorText("Container:\n"));
					if (temp.Count > 0) {
						output.Add(new ColorText($"\t{i}: "));
						output.AddRange(temp);
						output.Add(new ColorText("\n"));
					}
				}
			}
			return output;
		}

		/// <summary>
		/// Exports container information
		/// </summary>
		/// <param name="space">How many spaces to start the string with</param>
		/// <returns>Copyable constructor of container</returns>
		public string Export(int space) {
			string retStr = string.Empty;
			for (int i = 0; i < space; i++)
				retStr += " ";
			retStr += $"new {SubType}(new List<IItem>() {{{(Items.Count > 0 ? "\n" : " ")}";
			for (int i = 0; i < Items.Count; i++) {
				if (Items[i] is Container) {
					retStr += Items[i].SubType switch
					{
						"Bookshelf" => ((Bookshelf)Items[i]).Export(space + 2),
						"Dresser" => ((Dresser)Items[i]).Export(space + 2),
						"Fridge" => ((Fridge)Items[i]).Export(space + 2),
						"Table" => ((Table)Items[i]).Export(space + 2),
						_ => ((Container)Items[i]).Export(space + 2),
					};
					continue;
				}
				if (Items[i] is Display) {
					retStr += $"{((Display)Items[i]).Export(space + 2)}\n";
					continue;
				}
				for (int s = 0; s < space + 2; s++)
					retStr += " ";
				retStr += $"{Items[i].Export()}\n";
			}
			if (Items.Count > 0)
				for (int i = 0; i < space; i++)
					retStr += " ";
			return $"{retStr}}}, \"{Name}\"),\n";
		}

		/// <summary>
		/// Exports container information
		/// </summary>
		/// <returns>Copyable constructor of container</returns>
		public string Export() => $"new Container(new List<IItem>() {{ /*Items in Container*/ }}, \"{Name}\"),";


		/// <summary>
		/// Gets an item from the container
		/// </summary>
		/// <param name="item">Item you want</param>
		/// <returns>Returns requested item, or an Empty if item doesn't exist</returns>
		public IItem GetItem(int item) => item < 0 || item >= Items.Count ? new Empty() : Items[item];

		/// <summary>
		/// Adds item to container
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
		/// Remove item by index
		/// </summary>
		/// <param name="item">Item to remove</param>
		/// <returns>ColorText object saying the item was removed, or warns if item didn't exist</returns>
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
				}
				else {
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
		/// Removes item from container
		/// </summary>
		/// <param name="item">Item to remove</param>
		/// <returns>ColorText object saying the Item was removed, or that the item isn't in the container</returns>
		public ColorText RemoveItem(IItem item) => Items.Remove(item) ? new ColorText(new string[] { "\nItem ", "removed", ".\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.DarkBlue, ConsoleColor.White }) : new ColorText(new string[] { "No matching ", "Item", " found" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });

		/// <summary>
		/// Whether or not item is in the container
		/// </summary>
		/// <param name="item">Test Item</param>
		/// <returns>True if the item is in the container, False if not</returns>
		public bool HasItem(IItem item) {
			foreach (IItem i in Items)
				if (i == item)
					return true;
			return false;
		}

		/// <summary>
		/// Sets the name of the container
		/// </summary>
		/// <param name="name">New name</param>
		public void Rename(string name) => Name = name;

		/// <summary>
		/// Minor details for list
		/// </summary>
		/// <param name="beforeNotAfter">True for left side, False for right side</param>
		/// <returns>ColorText object of minor container details</returns>
		public ColorText ListInfo(bool beforeNotAfter) => beforeNotAfter ? ColorText.Empty : Items.Count > 0 ? new ColorText(new string[] { ", ", Items.Count.ToString(), " Items", Name.Equals(string.Empty) ? string.Empty : $", {Name}" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.White }) : new ColorText(new string[] { ", ", "Empty", Name.Equals(string.Empty) ? string.Empty : $", {Name}" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });

		/// <summary>
		/// Information about container
		/// </summary>
		/// <returns>ColorText object of important info</returns>
		public ColorText ToText() {
			List<string> retStr = new List<string>() { "Items", " in this ", "Container", ":" };
			List<ConsoleColor> retClr = new List<ConsoleColor>() { ConsoleColor.DarkYellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White };
			for (int i = 0; i < Items.Count; i++) {
				retStr.Add($"\n\t{i.ToString()}");
				retClr.Add(ConsoleColor.Cyan);
				retStr.Add(": ");
				retClr.Add(ConsoleColor.White);
				ColorText left = Items[i].ListInfo(true);
				foreach (string str in left.GetLines())
					retStr.Add(str);
				foreach (ConsoleColor clr in left.Colors())
					retClr.Add(clr);
				retStr.Add(Items[i].SubType);
				retClr.Add(ConsoleColor.Yellow);
				ColorText right = Items[i].ListInfo(false);
				foreach (string str in right.GetLines())
					retStr.Add(str);
				foreach (ConsoleColor clr in right.Colors())
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
	}
}
