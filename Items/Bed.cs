using HouseCS.ConsoleUtils;
using System;
using System.Collections.Generic;

namespace HouseCS.Items {
	/// <summary>
	/// Bed, can be adjustable, and there are different sizes
	/// </summary>
	public class Bed : IItem {
		/// <summary>
		/// Bed sizes
		/// </summary>
		public static readonly string[] types = { "King", "Queen", "Twin", "Single" };

		private static readonly string typeS = "Bed";

		/// <summary>
		/// Boolean for whether or not the bed moves
		/// </summary>
		private bool Adjustable { get; set; }

		/// <summary>
		/// Set bed size
		/// </summary>
		private int BedType { get; set; }

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
				if (key.Equals(types[BedType], StringComparison.OrdinalIgnoreCase) ||
				(key.ToLower().Equals("adjustable") && Adjustable)) {
					output.Add(ListInfo(true));
					output.Add(new ColorText(typeS));
					output.Add(ListInfo(false));
				}
			}
			return output;
		}

		/// <summary>
		/// Exports Bed information
		/// </summary>
		/// <returns>String of bed constructor</returns>
		public string Export() {
			return $"new Bed({(Adjustable ? "true" : "false")}, {BedType}),";
		}

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
		/// <returns>ColorText object of minor bed details</returns>
		public ColorText ListInfo(bool beforeNotAfter) => new ColorText(new string[] { beforeNotAfter ? $"{types[BedType]} " : Adjustable ? " - Adjustable" : "" }, new ConsoleColor[] { ConsoleColor.White });

		/// <summary>
		/// Information about bed
		/// </summary>
		/// <returns>ColorText object of important info</returns>
		public ColorText ToText() => new ColorText(new string[] { $"{(Adjustable ? "Adjustable" : "Non adjustable")} {types[BedType]} size bed" }, new ConsoleColor[] { ConsoleColor.White });

		/// <summary>
		/// Creates non adjustable Twin bed
		/// </summary>
		public Bed() : this(false, 2) { }

		/// <summary>
		/// Creates a bed, set adjustability, and set size
		/// </summary>
		/// <param name="adjustable">True if bed moves, False if not</param>
		/// <param name="type">Index of bed type</param>
		public Bed(bool adjustable, int type) {
			Adjustable = adjustable;
			BedType = type >= 0 && type < types.Length ? type : 2;
		}
	}
}
