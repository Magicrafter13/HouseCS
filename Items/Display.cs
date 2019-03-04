using System;
using System.Collections.Generic;
using HouseCS.ConsoleUtils;
using HouseCS.Items.Containers;

namespace HouseCS.Items {
	/// <summary>
	/// Display, can be a monitor or a TV, and can have devices connected to it, also has a set size
	/// </summary>
	public class Display : IItem {
		private static readonly string typeS = "Display";

		private readonly List<IItem> connectedTo;

		/// <summary>
		/// Room the display is in
		/// </summary>
		public int RoomID { get; private set; }

		/// <summary>
		/// Displays size in inches
		/// </summary>
		public double SizeInch { get; private set; }

		/// <summary>
		/// Whether or not the Display is a monitor, or a TV
		/// </summary>
		public bool IsMonitor { get; private set; }

		/// <summary>
		/// How many devices are connected to the display
		/// </summary>
		public int DeviceCount => connectedTo.Count;

		/// <summary>
		/// string of Item type
		/// </summary>
		public string Type => typeS;

		/// <summary>
		/// string of Item sub-type
		/// </summary>
		public string SubType => typeS;

		/// <summary>
		/// Matches keyword against Item data
		/// </summary>
		/// <param name="keywords">Keywords to search for</param>
		/// <returns>String output if keywords matched</returns>
		string Search(List<string> keywords) {
			string output = string.Empty;
			foreach (string key in keywords)
				if (key.Equals((IsMonitor ? "Monitor" : "Display"), StringComparison,OrdinalIgnoreCase))
					output += ListInfo(true) + typeS + ListInfo(false);
			return output;
		}

		/// <summary>
		/// Exports Display information
		/// </summary>
		/// <param name="space">How many spaces to start the string with</param>
		/// <returns>String of display constructor</returns>
		public string Export(int space) {
			string retStr = string.Empty;
			for (int i = 0; i < space; i++)
				retStr += " ";
			retStr += $"new Container({(IsMonitor ? "true" : "false")}, new List<IItem>() {{\n";
			for (int i = 0; i < connectedTo.Count; i++) {
				if (connectedTo[i] is Container) {
					switch (connectedTo[i].SubType) {
						case "Bookshelf":
							retStr += ((Bookshelf)connectedTo[i]).Export(space + 2);
							break;
						case "Dresser":
							retStr += ((Dresser)connectedTo[i]).Export(space + 2);
							break;
						case "Fridge":
							retStr += ((Fridge)connectedTo[i]).Export(space + 2);
							break;
						case "Table":
							retStr += ((Table)connectedTo[i]).Export(space + 2);
							break;
						default:
							retStr += ((Container)connectedTo[i]).Export(space + 2);
							break;
					}
					continue;
				}
				if (connectedTo[i] is Display) {
					retStr += ((Display)connectedTo[i]).Export(space + 2);
					continue;
				}
				for (int s = 0; s < space + 2; s++)
					retStr += " ";
				retStr += connectedTo[i].Export();
			}
			for (int i = 0; i < space; i++)
				retStr += " ";
			return $"{retStr}}}, {SizeInch}, {RoomID}),";
		}

		/// <summary>
		/// Exports Display information
		/// </summary>
		/// <returns>String of display constructor</returns>
		public string Export() => $"new Display({(IsMonitor ? "true" : "false")}, new List<IItem>() {{ /*Connected Items*/ }}, {SizeInch}, {RoomID}),";

		/// <summary>
		/// Checks if Item is connected
		/// </summary>
		/// <param name="item">test Item</param>
		/// <returns>True if Item is connected, false if not</returns>
		public bool HasItem(IItem item) {
			foreach (IItem i in connectedTo)
				if (i == item)
					return true;
			return false;
		}

		/// <summary>
		/// Connects Item to display
		/// </summary>
		/// <param name="item">connecting Item</param>
		/// <returns>ColorText object showing the Item is connected, or tells the user the Item is already connected</returns>
		public ColorText Connect(IItem item) {
			if (HasItem(item))
				return new ColorText(new string[] { $"This ", "Item", " is already connected." }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
			connectedTo.Add(item);
			List<string> retStr = new List<string>();
			List<ConsoleColor> retClr = new List<ConsoleColor>();
			ColorText left = item.ListInfo(true);
			foreach (string str in left.Lines)
				retStr.Add(str);
			foreach (ConsoleColor clr in left.Colors)
				retClr.Add(clr);
			retStr.Add(item.Type);
			retClr.Add(ConsoleColor.White);
			ColorText right = item.ListInfo(false);
			foreach (string str in right.Lines)
				retStr.Add(str);
			foreach (ConsoleColor clr in right.Colors)
				retClr.Add(clr);
			retStr.Add(" connected to this monitor.\n");
			retClr.Add(ConsoleColor.White);
			return new ColorText(retStr.ToArray(), retClr.ToArray());
		}

		/// <summary>
		/// Disconnects Item from display
		/// </summary>
		/// <param name="item">Index of disconnecting Item</param>
		/// <returns>ColorText object showing the Item was disconnected, or tells the user the Item isn't connected</returns>
		public ColorText Disconnect(int item) {
			if (item < 0 || item >= connectedTo.Count)
				return connectedTo.Count == 0
					? new ColorText(new string[] { "Display", " has no devices connected!" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White })
					: new ColorText(new string[] { "Display", " only has ", connectedTo.Count.ToString(), $" device{(connectedTo.Count > 1 ? "s" : string.Empty)} connected to it." }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
			connectedTo.RemoveAt(item);
			return new ColorText(new string[] { $"\nDevice {item}", " disconnected", ".\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White });
		}

		/// <summary>
		/// Disconnects Item from display
		/// </summary>
		/// <param name="item">disconnecting Item</param>
		/// <returns>ColorText object showing the Item was disconnected, or tells the user the Item isn't connected</returns>
		public ColorText Disconnect(IItem item) => connectedTo.Remove(item)
			? new ColorText(new string[] { "\nDevice", ", ", "disconnected\n" }, new ConsoleColor[] { ConsoleColor.DarkYellow, ConsoleColor.White, ConsoleColor.DarkBlue })
			: new ColorText(new string[] { "No matching ", "Device", " found." }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White });

		/// <summary>
		/// Gets a connected device
		/// </summary>
		/// <param name="item">Index of Item</param>
		/// <returns>Item object of connected device</returns>
		public IItem GetDevice(int item) => connectedTo[item];

		/// <summary>
		/// Minor details for list
		/// </summary>
		/// <param name="beforeNotAfter">True for left side, False for right side</param>
		/// <returns>ColorText object of minor display details</returns>
		public ColorText ListInfo(bool beforeNotAfter) => new ColorText(new string[] { beforeNotAfter ? $"{SizeInch}\" {(IsMonitor ? "Monitor" : "TV")} (" : $") - {connectedTo.Count} devices are connected to it" }, new ConsoleColor[] { ConsoleColor.White });

		/// <summary>
		/// Information about display
		/// </summary>
		/// <returns>ColorText object of important info</returns>
		public ColorText ToText() {
			List<string> retStr = new List<string>() { $"{SizeInch}\" {(IsMonitor ? "Monitor" : "TV")} (", connectedTo.Count.ToString(), " devices):" };
			List<ConsoleColor> retClr = new List<ConsoleColor>() { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White };
			for (int i = 0; i < connectedTo.Count; i++) {
				retStr.Add($"\n\t{i.ToString()}");
				retClr.Add(ConsoleColor.Cyan);
				retStr.Add(": ");
				retClr.Add(ConsoleColor.White);
				ColorText left = connectedTo[i].ListInfo(true);
				foreach (string str in left.Lines)
					retStr.Add(str);
				foreach (ConsoleColor clr in left.Colors)
					retClr.Add(clr);
				retStr.Add(connectedTo[i].Type);
				retClr.Add(ConsoleColor.Yellow);
				ColorText right = connectedTo[i].ListInfo(false);
				foreach (string str in right.Lines)
					retStr.Add(str);
				foreach (ConsoleColor clr in right.Colors)
					retClr.Add(clr);
			}
			return new ColorText(retStr.ToArray(), retClr.ToArray());
		}

		/// <summary>
		/// Creates a TV, with no connected devices, that is 20 inches
		/// </summary>
		public Display() : this(false, new List<IItem>(), 20.0, -1) { }

		/// <summary>
		/// Creates a set sized display, with a set type, and a list of connected devices
		/// </summary>
		/// <param name="isMonitor">Whether or not it's a monitor</param>
		/// <param name="connectedDevs">List of connected devices</param>
		/// <param name="inchSize">Display size in inches</param>
		/// <param name="room">Room for display</param>
		public Display(bool isMonitor, List<IItem> connectedDevs, double inchSize, int room) {
			IsMonitor = isMonitor;
			connectedTo = connectedDevs;
			SizeInch = inchSize > 0 ? inchSize : 20.0;
			RoomID = room;
		}
	}
}
