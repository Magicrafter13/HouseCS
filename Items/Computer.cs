using System;
using HouseCS.ConsoleUtils;

namespace HouseCS.Items
{
	public class Computer : IItem
	{
		private string computerType;
		private bool isOn = false;
		private static int totalComps = 0;
		private readonly int id;
		private static readonly string typeS = "Computer";
		private string brand;
		private string family;
		private string model;

		public void Reset(string b, string f, string m, bool state, string type)
		{
			computerType = type;
			isOn = state;
			brand = b;
			family = f;
			model = m;
		}
		public Computer() : this("Generic", "PC", "", false, "Desktop") { }
		public Computer(string b, string f, string m, bool state, string type)
		{
			Reset(b, f, m, state, type);
			id = totalComps;
			totalComps++;
		}
		public bool HasItem(IItem test) => false;
		public void TurnOn() => isOn = true;
		public void TurnOff() => isOn = false;
		public string Type => typeS;
		public string SubType => typeS;
		public ColorText ListInfo(bool beforeNotAfter) => new ColorText(new string[] { beforeNotAfter ? $"{brand} " : $", turned {(isOn ? "on" : "off")}" }, new ConsoleColor[] { ConsoleColor.White });
		public ColorText ToText() => new ColorText(new string[] { $"{computerType} Computer, ID:{id}\nCurrently powered {(isOn ? "on" : "off")}\nIt is a(n) {brand} {family} {model}" }, new ConsoleColor[] { ConsoleColor.White });
	}
}
