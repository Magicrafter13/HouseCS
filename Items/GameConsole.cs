using System;
using HouseCS.ConsoleUtils;

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
		/// string of Item type
		/// </summary>
		public string Type => typeS;

		/// <summary>
		/// string of Item sub-type
		/// </summary>
		public string SubType => typeS;

		/// <summary>
		/// Don't use
		/// </summary>
		/// <param name="item">test Item</param>
		/// <returns>false</returns>
		public bool HasItem(IItem item) => false;

		/// <summary>
		/// Minor details for list
		/// </summary>
		/// <param name="beforeNotAfter">True for left side, False for right side</param>
		/// <returns>ColorText object of minor console details</returns>
		public ColorText ListInfo(bool beforeNotAfter) => new ColorText(new string[] { beforeNotAfter ? string.Empty : $" - {types[SysType]}" }, new ConsoleColor[] { ConsoleColor.White });

		/// <summary>
		/// Information about console
		/// </summary>
		/// <returns>ColorText object of important info</returns>
		public ColorText ToText() => new ColorText(new string[] { $"This Video Game {types[SysType]}, is a {Company}\n{System}" }, new ConsoleColor[] { ConsoleColor.White });

		/// <summary>
		/// Creates a console, Generic System 1000 from Generi-sys
		/// </summary>
		public GameConsole() : this(0, "Generi-sys", "Generic System 1000") { }

		/// <summary>
		/// Creates a type, system from company
		/// </summary>
		/// <param name="type">Console type</param>
		/// <param name="company">Console company</param>
		/// <param name="system">Console system</param>
		public GameConsole(int type, string company, string system) {
			SysType = type >= 0 && type < types.Length ? type : 0;
			Company = company;
			System = system;
		}
	}
}
