using System;
using HouseCS.ConsoleUtils;

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
		/// Room the bed is in
		/// </summary>
		public int RoomID { get; private set; }

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
		/// Exports Bed information
		/// </summary>
		/// <returns>String of bed constructor</returns>
		public string Export() {
			return $"new Bed({(Adjustable ? "true" : "false")}, {BedType}, {RoomID}),";
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
		public Bed() : this(false, 2, -1) { }

		/// <summary>
		/// Creates a bed, set adjustability, and set size
		/// </summary>
		/// <param name="adjustable">True if bed moves, False if not</param>
		/// <param name="type">Index of bed type</param>
		/// <param name="room">Room the bed is in</param>
		public Bed(bool adjustable, int type, int room) {
			Adjustable = adjustable;
			BedType = type >= 0 && type < types.Length ? type : 2;
			RoomID = room;
		}
	}
}