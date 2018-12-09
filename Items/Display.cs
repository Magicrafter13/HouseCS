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
		public string Connect(IItem item)
		{
			connectedTo.Add(item);
			return $"{item.ListInfo(true)}{item.Type}{item.ListInfo(false)} connected to this monitor.\n";
		}
		public string Disconnect(int item)
		{
			if (item < 0 || item >= connectedTo.Count)
				return connectedTo.Count == 0
					? "Display has no devices connected!"
					: $"Display only has {connectedTo.Count} device{(connectedTo.Count > 1 ? "s" : "")} connected to it.";
			connectedTo.RemoveAt(item);
			return $"\nDevice {item} removed.\n";
		}
		public int DeviceCount => connectedTo.Count;
		public IItem GetDevice(int i) => connectedTo[i];
		public string Type => typeS;
		public string ListInfo(bool before_not_after) => before_not_after ? $"{sizeInch}\" {(isMonitor ? "Monitor" : "TV")} (" : $") - {connectedTo.Count} devices are connected to it";
		public override string ToString()
		{
			string ret_val = string.Empty;
			for (int i = 0; i < connectedTo.Count; i++)
				ret_val += $"\n{i}: {connectedTo[i].ListInfo(true)}{connectedTo[i].Type}{connectedTo[i].ListInfo(false)}";
			return $"{sizeInch}\" {(isMonitor ? "Monitor" : "TV")} ({connectedTo.Count} devices):{ret_val}";
		}
	}
}
