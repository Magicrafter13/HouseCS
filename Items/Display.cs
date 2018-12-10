using System.Collections.Generic;

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
		public string Connect(IItem item)
		{
			if (HasItem(item)) return $"This {Program.Bright("yellow", "Item")} is already connected.";
			connectedTo.Add(item);
			return $"{item.ListInfo(true)}{item.Type}{item.ListInfo(false)} connected to this monitor.\n";
		}
		public string Disconnect(int item)
		{
			if (item < 0 || item >= connectedTo.Count)
				return connectedTo.Count == 0
					? $"{Program.Bright("yellow", "Display")} has no devices connected!"
					: $"{Program.Bright("yellow", "Display")} only has {Program.Bright("cyan", connectedTo.Count.ToString())} device{(connectedTo.Count > 1 ? "s" : string.Empty)} connected to it.";
			connectedTo.RemoveAt(item);
			return $"\nDevice {item}{Program.Color("blue", " disconnected.\n")}";
		}
		public string Disconnect(IItem d) => connectedTo.Remove(d)
				? $"{Program.Color("yellow", "\nDevice")}, {Program.Color("blue", "disconnected")}\n"
				: $"No matching {Program.Color("yellow", "Device")} found.";
		public int DeviceCount => connectedTo.Count;
		public IItem GetDevice(int i) => connectedTo[i];
		public string Type => typeS;
		public string ListInfo(bool before_not_after) => before_not_after ? $"{sizeInch}\" {(isMonitor ? "Monitor" : "TV")} (" : $") - {connectedTo.Count} devices are connected to it";
		public override string ToString()
		{
			string retVal = $"{sizeInch}\" {(isMonitor ? "Monitor" : "TV")} ({Program.Bright("cyan", connectedTo.Count.ToString())} devices):";
			for (int i = 0; i < connectedTo.Count; i++)
				retVal += $"\n{Program.Bright("cyan", i.ToString())}: {connectedTo[i].ListInfo(true)}{Program.Bright("yellow", connectedTo[i].Type)}{connectedTo[i].ListInfo(false)}";
			return retVal;
		}
	}
}
