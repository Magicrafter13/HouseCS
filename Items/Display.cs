using System.Collections.Generic;
using System;
using HouseCS.ConsoleUtils;

namespace HouseCS.Items
{
	public class Display : IItem
	{
		private static readonly string typeS = "Display";
		private readonly bool isMonitor;
		private readonly List<IItem> connectedTo = new List<IItem>();
		private readonly double sizeInch;

		public Display() : this(false, new List<IItem>(), 20.0) { }
		public Display(bool mon, List<IItem> con, double sin)
		{
			isMonitor = mon;
			connectedTo = con;
			sizeInch = sin > 0 ? sin : 20.0;
		}
		public bool HasItem(IItem test) {
			foreach (IItem i in connectedTo) if (i == test) return true;
			return false;
		}
		public ColorText Connect(IItem item)
		{
			if (HasItem(item)) return new ColorText(new string[] { $"This ", "Item", " is already connected." }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
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
		public ColorText Disconnect(int item)
		{
			if (item < 0 || item >= connectedTo.Count)
				return connectedTo.Count == 0
					? new ColorText(new string[] { "Display", " has no devices connected!" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White })
					: new ColorText(new string[] { "Display", " only has ", connectedTo.Count.ToString(), $" device{(connectedTo.Count > 1 ? "s" : string.Empty)} connected to it." }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
			connectedTo.RemoveAt(item);
			return new ColorText(new string[] { $"\nDevice {item}", " disconnected", ".\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White });
		}
		public ColorText Disconnect(IItem d) => connectedTo.Remove(d)
			? new ColorText(new string[] { "\nDevice", ", ", "disconnected\n" }, new ConsoleColor[] { ConsoleColor.DarkYellow, ConsoleColor.White, ConsoleColor.DarkBlue })
			: new ColorText(new string[] { "No matching ", "Device", " found." }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White });
		public int DeviceCount => connectedTo.Count;
		public IItem GetDevice(int i) => connectedTo[i];
		public string Type => typeS;
		public string SubType => typeS;
		public ColorText ListInfo(bool before_not_after) => new ColorText(new string[] { before_not_after ? $"{sizeInch}\" {(isMonitor ? "Monitor" : "TV")} (" : $") - {connectedTo.Count} devices are connected to it" }, new ConsoleColor[] { ConsoleColor.White });
		public ColorText ToText()
		{
			List<string> retStr = new List<string>() { $"{sizeInch}\" {(isMonitor ? "Monitor" : "TV")} (", connectedTo.Count.ToString(), " devices):" };
			List<ConsoleColor> retClr = new List<ConsoleColor>() { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White };
			for (int i = 0; i < connectedTo.Count; i++) {
				retStr.Add($"\n\t{i.ToString()}");
				retClr.Add(ConsoleColor.Cyan);
				retStr.Add(": ");
				retClr.Add(ConsoleColor.White);
				ColorText left = connectedTo[i].ListInfo(true);
				foreach (string str in left.Lines) retStr.Add(str);
				foreach (ConsoleColor clr in left.Colors) retClr.Add(clr);
				retStr.Add(connectedTo[i].Type);
				retClr.Add(ConsoleColor.Yellow);
				ColorText right = connectedTo[i].ListInfo(false);
				foreach (string str in right.Lines) retStr.Add(str);
				foreach (ConsoleColor clr in right.Colors) retClr.Add(clr);
			}
			return new ColorText(retStr.ToArray(), retClr.ToArray());
		}
	}
}
