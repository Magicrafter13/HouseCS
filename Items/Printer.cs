using System;
using System.Collections.Generic;
using HouseCS.ConsoleUtils;

namespace HouseCS.Items {
	public class Printer : IItem {
		private static readonly string typeS = "Printer";

		public bool CanFax { get; private set; }

		public bool CanScan { get; private set; }

		public bool HasColor { get; private set; }

		/// <summary>
		/// Room the Item is in
		/// </summary>
		public int RoomID { get; private set; }

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
		string Search(List<string> keywords) {
			string output = string.Empty;
			foreach (string key in keywords)
				if ((CanFax && key.Equals("Fax", StringComparison.OrdinalIgnoreCase)) ||
				(CanScan && key.Equals("Scan", StringComparison.OrdinalIgnoreCase)) ||
				(HasColor && key.Equals("Color", StringComparison.OrdinalIgnoreCase)))
					output += ListInfo(true) + typeS + ListInfo(false);
			return output;
		}

		/// <summary>
		/// Exports Item information
		/// </summary>
		/// <returns>String containing a constructor for the Item</returns>
		public string Export() => $"new Printer({(CanFax ? "true" : "false")}, {(CanScan ? "true" : "false")}, {(HasColor ? "true" : "false")}, {RoomID}),";

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
		/// <returns>ColorText object of minor printer details</returns>
		public ColorText ListInfo(bool beforeNotAfter) => new ColorText(beforeNotAfter ? HasColor ? "Color " : "Black/White " : CanFax || CanScan ? ", with " + (CanFax ? "Fax" + (CanScan ? ", and Scanner" : string.Empty) : "Scanner") : string.Empty);

		/// <summary>
		/// Information about Printer
		/// </summary>
		/// <returns>ColorText object of important info</returns>
		public ColorText ToText() => new ColorText(new string[] { "Printer:\n", $"\tHas Color: {(HasColor ? "Yes" : "No")}\n\tCan Fax: {(CanFax ? "Yes" : "No")}\n\tCan Scan: {(CanScan ? "Yes" : "No")}" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White });

		public Printer() {
			CanFax = false;
			CanScan = true;
			HasColor = true;
			RoomID = -1;
		}
		public Printer(bool canFax, bool canScan, bool hasColor, int room) {
			CanFax = canFax;
			CanScan = canScan;
			HasColor = hasColor;
			RoomID = room;
		}
	}
}
