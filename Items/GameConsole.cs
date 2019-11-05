using HouseCS.ConsoleUtils;
using System;
using System.Collections.Generic;

namespace HouseCS.Items {
	/// <summary>
	/// Console, a company, a system name, and a set type
	/// </summary>
	public class GameConsole : IItem {
		private static readonly string typeS = "Console";

		/// <summary>
		/// Types of consoles
		/// </summary>
		public static readonly string[] types = { "Console", "Handheld", "Hybrid System" };

		/// <summary>
		/// Console type
		/// </summary>
		public int SysType { get; private set; }

		/// <summary>
		/// Console company
		/// </summary>
		public string Company { get; private set; }

		/// <summary>
		/// Console system
		/// </summary>
		public string System { get; private set; }

		/// <summary>
		/// Name of console
		/// </summary>
		public string Name { get; private set; }

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
		public List<ColorText> Search(List<string> keywords) {
			List<ColorText> output = new List<ColorText>();
			foreach (string key in keywords) {
				if (key.Equals(types[SysType], StringComparison.OrdinalIgnoreCase) ||
					key.Equals(Company, StringComparison.OrdinalIgnoreCase) ||
					key.Equals(System, StringComparison.OrdinalIgnoreCase) ||
					key.Equals(Name, StringComparison.OrdinalIgnoreCase)) {
					output.Add(new ColorText($"{types[SysType]} - {Company} {System}{(Name.Equals(string.Empty) ? string.Empty : $", {Name}")}"));
				}
			}
			return output;
		}

		/// <summary>
		/// Exports GameConsole information
		/// </summary>
		/// <returns>String of gameconsole constructor</returns>
		public string Export() {
			return $"new GameConsole({SysType}, \"{Company}\", \"{System}\", \"{Name}\"),";
		}

		/// <summary>
		/// Don't use
		/// </summary>
		/// <param name="item">test Item</param>
		/// <returns>false</returns>
		public bool HasItem(IItem item) => false;

		/// <summary>
		/// Sets the name of the Console
		/// </summary>
		/// <param name="name">New name</param>
		public void Rename(string name) => Name = name;

		/// <summary>
		/// Minor details for list
		/// </summary>
		/// <param name="beforeNotAfter">True for left side, False for right side</param>
		/// <returns>ColorText object of minor console details</returns>
		public ColorText ListInfo(bool beforeNotAfter) => new ColorText(new string[] { beforeNotAfter ? string.Empty : $" - {types[SysType]}{(Name.Equals(string.Empty) ? string.Empty : $", {Name}")}" }, new ConsoleColor[] { ConsoleColor.White });

		/// <summary>
		/// Information about console
		/// </summary>
		/// <returns>ColorText object of important info</returns>
		public ColorText ToText() => new ColorText(new string[] { $"This Video Game {types[SysType]}, is a {Company}\n{System}\nAnd is called \"{Name}\"" }, new ConsoleColor[] { ConsoleColor.White });

		/// <summary>
		/// Creates a console, Generic System 1000 from Generi-sys
		/// </summary>
		public GameConsole() : this(0, "Generi-sys", "Generic System 1000", "Lame") { }

		/// <summary>
		/// Creates a 'type, system' from 'company', called 'name'
		/// </summary>
		/// <param name="type">Console type</param>
		/// <param name="company">Console company</param>
		/// <param name="system">Console system</param>
		/// <param name="name">Name of Console</param>
		public GameConsole(int type, string company, string system, string name) {
			SysType = type >= 0 && type < types.Length ? type : 0;
			Company = company;
			System = system;
			Rename(name);
		}
	}
}
