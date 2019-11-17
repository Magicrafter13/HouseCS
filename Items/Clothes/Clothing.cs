using HouseCS.ConsoleUtils;
using System;
using System.Collections.Generic;

namespace HouseCS.Items.Clothes {
	/// <summary>
	/// Clothing, is generic - use child classes to be more specific
	/// </summary>
	public class Clothing : IItem {
		private const string typeS = "Clothing";

		/// <summary>
		/// Creates a black piece of clothing, with no name
		/// </summary>
		public Clothing() : this("Black", string.Empty) { }

		/// <summary>
		/// Creates an article of clothing
		/// </summary>
		/// <param name="color">Color for clothes</param>
		/// <param name="name">Name of clothing</param>
		public Clothing(string color, string name)
		{
			Color = color ?? throw new ArgumentNullException(nameof(color));
			Rename(name);
		}

		/// <summary> Clothing's color </summary>
		public string Color { get; set; }

		/// <summary> Name of clothing </summary>
		public string Name { get; private set; }

		/// <summary> string of Item type </summary>
		public string Type => typeS;

		/// <summary> string of Item sub-type </summary>
		public string SubType => typeS;

		/// <summary>
		/// Creates specified clothing
		/// </summary>
		/// <param name="type">Clothing type</param>
		/// <returns>Requested clothing type</returns>
		public static IItem Create(string type) => type is null ? throw new ArgumentNullException(nameof(type)) : (type.ToLower()) switch
		{
			"shirt" => new Shirt(),
			"pants" => new Pants(),
			_ => new Clothing(),
		};

		/// <summary>
		/// Matches keywords against item data
		/// </summary>
		/// <param name="keywords">Keywords to search for</param>
		/// <returns>String output if keywords matched</returns>
		public List<ColorText> Search(List<string> keywords) {
			if (keywords is null)
				throw new ArgumentNullException(nameof(keywords));
			List<ColorText> output = new List<ColorText>();
			foreach (string key in keywords) {
				if (key.Equals(Color, StringComparison.OrdinalIgnoreCase) ||
					key.Equals(Name, StringComparison.OrdinalIgnoreCase)) {
					output.Add(ListInfo(true));
					output.Add(new ColorText(typeS));
					output.Add(ListInfo(false));
				}
			}
			return output;
		}

		/// <summary>
		/// Exports clothing information
		/// </summary>
		/// <returns>Copyable constructor of clothing</returns>
		public string Export() => $"new Clothing(\"{Color}\", \"{Name}\"),";

		/// <summary>
		/// Don't use
		/// </summary>
		/// <param name="item">test Item</param>
		/// <returns>false</returns>
		public bool HasItem(IItem item) => false;

		/// <summary>
		/// Sets the name of the clothing
		/// </summary>
		/// <param name="name">New name</param>
		public void Rename(string name) => Name = name ?? throw new ArgumentNullException(nameof(name));

		/// <summary>
		/// Minor details for list
		/// </summary>
		/// <param name="beforeNotAfter">True for left side, False for right side</param>
		/// <returns>ColorText object of minor clothing details</returns>
		public ColorText ListInfo(bool beforeNotAfter) => new ColorText(beforeNotAfter ? $"{(Name.Equals(string.Empty) ? Color : Name)} " : " - Generic", ConsoleColor.White);

		/// <summary>
		/// Information about clothing
		/// </summary>
		/// <returns>ColorText object of important info</returns>
		public ColorText ToText() => new ColorText($"This is a Generic piece of clothing, it is {Color}{(Name.Equals(string.Empty) ? string.Empty : $", and labeled {Name}")}", ConsoleColor.White);
	}
}
