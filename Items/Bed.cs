using System;
using HouseCS.ConsoleUtils;

namespace HouseCS.Items {
	public class Bed : IItem {
		public static readonly string[] types = { "King", "Queen", "Twin", "Single" };
		private readonly bool adjustable;
		private readonly int bedType;
		private static readonly string typeS = "Bed";

		public Bed() : this(false, 2) { }
		public Bed(bool a, int t) {
			adjustable = a;
			bedType = t >= 0 && t < types.Length ? t : 2;
		}
		public bool HasItem(IItem test) => false;
		public string Type => typeS;
		public string SubType => Type;
		public ColorText ListInfo(bool beforeNotAfter) => new ColorText(new string[] { beforeNotAfter ? $"{types[bedType]} " : adjustable ? " - Adjustable" : "" }, new ConsoleColor[] { ConsoleColor.White });
		public ColorText ToText() => new ColorText(new string[] { $"{(adjustable ? "Adjustable" : "Non adjustable")} {types[bedType]} size bed" }, new ConsoleColor[] { ConsoleColor.White });
	}
}