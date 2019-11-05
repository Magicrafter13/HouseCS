using HouseCS.ConsoleUtils;
using System;
using System.Collections.Generic;

namespace HouseCS.Items {
	/// <summary>
	/// Printer, stores ability to fax, scan, and print in color
	/// </summary>
	public class Printer : IItem {
		private static readonly string typeS = "Printer";

		/// <summary>
		/// Represents whether or not the printer can send faxes
		/// </summary>
		public bool CanFax { get; private set; }

		/// <summary>
		/// Represents whether or not the printer can scan documents
		/// </summary>
		public bool CanScan { get; private set; }

		/// <summary>
		/// Represents whether or not the printer prints in color
		/// </summary>
		public bool HasColor { get; private set; }

		/// <summary>
		/// Name of printer
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// string of Item parent type
		/// </summary>
		public string Type => typeS;

		/// <summary>
		/// string of Item sub type
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
				if ((CanFax && key.Equals("Fax", StringComparison.OrdinalIgnoreCase)) ||
				(CanScan && key.Equals("Scan", StringComparison.OrdinalIgnoreCase)) ||
				(HasColor && key.Equals("Color", StringComparison.OrdinalIgnoreCase)) ||
				key.Equals(Name, StringComparison.OrdinalIgnoreCase)) {
					output.Add(ListInfo(true));
					output.Add(new ColorText(typeS));
					output.Add(ListInfo(false));
				}
			}
			return output;
		}

		/// <summary>
		/// Exports Item information
		/// </summary>
		/// <returns>String containing a constructor for the Item</returns>
		public string Export() => $"new Printer({(CanFax ? "true" : "false")}, {(CanScan ? "true" : "false")}, {(HasColor ? "true" : "false")}, \"{Name}\"),";

		/// <summary>
		/// Don't use
		/// </summary>
		/// <param name="item">test Item</param>
		/// <returns>false</returns>
		public bool HasItem(IItem item) => false;

		/// <summary>
		/// Sets the name of the Printer
		/// </summary>
		/// <param name="name">New name</param>
		public void Rename(string name) => Name = name;

		/// <summary>
		/// Minor details for list
		/// </summary>
		/// <param name="beforeNotAfter">True for left side, False for right side</param>
		/// <returns>ColorText object of minor printer details</returns>
		public ColorText ListInfo(bool beforeNotAfter) => new ColorText(beforeNotAfter ? $"{(Name.Equals(string.Empty) ? string.Empty : $"{Name} - ")}{(HasColor ? "Color " : "Black/White ")}" : CanFax || CanScan ? ", with " + (CanFax ? "Fax" + (CanScan ? ", and Scanner" : string.Empty) : "Scanner") : string.Empty);

		/// <summary>
		/// Information about Printer
		/// </summary>
		/// <returns>ColorText object of important info</returns>
		public ColorText ToText() => new ColorText(new string[] { "Printer:\n", $"\tHas Color: {(HasColor ? "Yes" : "No")}\n\tCan Fax: {(CanFax ? "Yes" : "No")}\n\tCan Scan: {(CanScan ? "Yes" : "No")}\n\tName: {Name}" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White });

		/// <summary>
		/// Creates a printer that can't fax, but it can scan and print in color, and puts it in room -1
		/// </summary>
		public Printer() : this(false, true, true, string.Empty) { }

		/// <summary>
		/// Creates a printer, with the given parameters of faxing, scaning, color printing, and name
		/// </summary>
		/// <param name="canFax">Whether or not this Printer is capable of faxing</param>
		/// <param name="canScan">Whether or not this Printer has a built in scanner</param>
		/// <param name="hasColor">Whether or not this Printer uses color</param>
		/// <param name="name">Name of Printer</param>
		public Printer(bool canFax, bool canScan, bool hasColor, string name) {
			CanFax = canFax;
			CanScan = canScan;
			HasColor = hasColor;
			Rename(name);
		}
	}
}
