using HouseCS.ConsoleUtils;
using System;
using System.Collections.Generic;

namespace HouseCS.Items
{
	/// <summary>
	/// Bed - has several types
	/// </summary>
	public class Bed : IItem
	{
		/// <summary> Bed sizes </summary>
		public static readonly string[] types = { "King", "Queen", "Twin", "Single" };

		private const string typeS = "Bed";

		/// <summary>
		/// Creates non adjustable Twin bed
		/// </summary>
		public Bed() : this(false, 2, string.Empty) { }

		/// <summary>
		/// Creates a bed
		/// </summary>
		/// <param name="adjustable">True if bed moves, False if not</param>
		/// <param name="type">Index of bed type</param>
		/// <param name="name">Name of the bed</param>
		public Bed(bool adjustable, int type, string name)
		{
			Adjustable = adjustable;
			BedType = type >= 0 && type < types.Length ? type : 2;
			Rename(name ?? throw new ArgumentNullException(nameof(name)));
		}

		/// <summary> Name of bed </summary>
		public string Name { get; private set; }

		/// <summary> string of Item type </summary>
		public string Type => typeS;

		/// <summary> string of Item sub-type </summary>
		public string SubType => typeS;

		/// <summary> Boolean for whether or not the bed moves </summary>
		private bool Adjustable { get; set; }

		/// <summary> Set bed size </summary>
		private int BedType { get; set; }

		/// <summary>
		/// Matches keywords against item data
		/// </summary>
		/// <param name="keywords">Keywords to search for</param>
		/// <returns>ColorText object of items found</returns>
		public List<ColorText> Search(List<string> keywords)
		{
			if (keywords is null)
				throw new ArgumentNullException(nameof(keywords));
			List<ColorText> output = new List<ColorText>();
			foreach (string key in keywords) {
				if (key.Equals(types[BedType], StringComparison.OrdinalIgnoreCase) ||
				(key.ToLower().Equals("adjustable") && Adjustable) ||
				key.Equals(Name, StringComparison.OrdinalIgnoreCase)) {
					output.Add(ListInfo(true));
					output.Add(new ColorText(typeS));
					output.Add(ListInfo(false));
				}
			}
			return output;
		}

		/// <summary>
		/// Exports bed information
		/// </summary>
		/// <returns>Copyable constructor of bed</returns>
		public string Export() => $"new Bed({(Adjustable ? "true" : "false")}, {BedType}, \"{Name}\"),";

		/// <summary>
		/// Don't use
		/// </summary>
		/// <param name="item">test Item</param>
		/// <returns>false</returns>
		public bool HasItem(IItem item) => false;

		/// <summary>
		/// Sets the name of the Bed
		/// </summary>
		/// <param name="name">New name</param>
		public void Rename(string name) => Name = name ?? throw new ArgumentNullException(nameof(name));

		/// <summary>
		/// Minor details for list
		/// </summary>
		/// <param name="beforeNotAfter">True for left side, False for right side</param>
		/// <returns>ColorText object of minor bed details</returns>
		public ColorText ListInfo(bool beforeNotAfter) => new ColorText(new string[] { beforeNotAfter ? $"{(string.IsNullOrEmpty(Name) ? types[BedType] : Name)} " : Adjustable ? " - Adjustable" : "" }, new ConsoleColor[] { ConsoleColor.White });

		/// <summary>
		/// Information about bed
		/// </summary>
		/// <returns>Adjustability, size, and name</returns>
		public ColorText ToText() => new ColorText(new string[] { $"{(Adjustable ? "Adjustable" : "Non adjustable")} {types[BedType]} size bed{(string.IsNullOrEmpty(Name) ? string.Empty : $", {Name}")}" }, new ConsoleColor[] { ConsoleColor.White });
	}
}
