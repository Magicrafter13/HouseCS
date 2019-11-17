using HouseCS.ConsoleUtils;
using System;
using System.Collections.Generic;

namespace HouseCS.Items
{
	/// <summary>
	/// Computer, a slightly more advanced item, has several info strings, as well as power state
	/// </summary>
	public class Computer : IItem
	{
		private static int totalComps = 0;

		private readonly int id;

		private static readonly string typeS = "Computer";

		/// <summary>
		/// Creates a "Generic" brand, "PC", [no model], Desktop computer that is turned off, with no name
		/// </summary>
		public Computer() : this("Generic", "PC", string.Empty, false, "Desktop", string.Empty) { }

		/// <summary>
		/// Creates a computer
		/// </summary>
		/// <param name="brand">Computer brand (ie HP, Dell, Acer)</param>
		/// <param name="family">Computer family line (ie Pavilion, Inspiron, Aspire)</param>
		/// <param name="model">Computer family model (ie dv6, 11, e15)</param>
		/// <param name="state">Computer power state (on / off)</param>
		/// <param name="type">Computer type (ie Desktop, Laptop)</param>
		/// <param name="name">Name of computer</param>
		public Computer(string brand, string family, string model, bool state, string type, string name)
		{
			Reset(brand, family, model, state, type, name);
			id = totalComps;
			totalComps++;
		}

		/// <summary> Computer brand </summary>
		public string Brand { get; private set; }

		/// <summary> Computer family line </summary>
		public string Family { get; private set; }

		/// <summary> Computer family model </summary>
		public string Model { get; private set; }

		/// <summary> Whether or not the computer is powered on </summary>
		public bool IsOn { get; private set; }

		/// <summary> Type of computer </summary>
		public string ComputerType { get; private set; }

		/// <summary> Name of computer </summary>
		public string Name { get; private set; }

		/// <summary> string of Item type </summary>
		public string Type => typeS;

		/// <summary> string of Item sub-type </summary>
		public string SubType => typeS;

		/// <summary>
		/// Sets the name of the computer
		/// </summary>
		/// <param name="name">New name</param>
		public void Rename(string name) => Name = name ?? throw new ArgumentNullException(nameof(name));

		/// <summary>
		/// Sets the brand, family, model, power-state, type, and name of the computer
		/// </summary>
		/// <param name="brand">Computer brand</param>
		/// <param name="family">Computer family line</param>
		/// <param name="model">Computer family model</param>
		/// <param name="state">Computer power state</param>
		/// <param name="type">Computer type</param>
		/// <param name="name">Name of Computer</param>
		public void Reset(string brand, string family, string model, bool state, string type, string name)
		{
			ComputerType = type ?? throw new ArgumentNullException(nameof(type));
			IsOn = state;
			Brand = brand ?? throw new ArgumentNullException(nameof(brand));
			Family = family ?? throw new ArgumentNullException(nameof(family));
			Model = model ?? throw new ArgumentNullException(nameof(model));
			Rename(name);
		}

		/// <summary>
		/// Matches keywords against item data
		/// </summary>
		/// <param name="keywords">Keywords to search for</param>
		/// <returns>String output if keywords matched</returns>
		public List<ColorText> Search(List<string> keywords)
		{
			if (keywords is null)
				throw new ArgumentNullException(nameof(keywords));
			List<ColorText> output = new List<ColorText>();
			foreach (string key in keywords) {
				if (key.Equals(ComputerType, StringComparison.OrdinalIgnoreCase) ||
					key.Equals(Brand, StringComparison.OrdinalIgnoreCase) ||
					key.Equals(Family, StringComparison.OrdinalIgnoreCase) ||
					key.Equals(Model, StringComparison.OrdinalIgnoreCase) ||
					key.Equals(Name, StringComparison.OrdinalIgnoreCase)) {
					output.Add(ListInfo(true));
					output.Add(new ColorText(typeS));
					output.Add(ListInfo(false));
				}
			}
			return output;
		}

		/// <summary>
		/// Exports computer information
		/// </summary>
		/// <returns>Copyable constructor of computer</returns>
		public string Export() => $"new Computer(\"{Brand}\", \"{Family}\", \"{Model}\", {(IsOn ? "true" : "false")}, \"{ComputerType}\", \"{Name}\"),";

		/// <summary>
		/// Don't use
		/// </summary>
		/// <param name="item">test Item</param>
		/// <returns>false</returns>
		public bool HasItem(IItem item) => false;

		/// <summary>
		/// Turns the computer on
		/// </summary>
		public void TurnOn() => IsOn = true;

		/// <summary>
		/// Turns the computer off
		/// </summary>
		public void TurnOff() => IsOn = false;

		/// <summary>
		/// Minor details for list
		/// </summary>
		/// <param name="beforeNotAfter">True for left side, False for right side</param>
		/// <returns>ColorText object of minor computer details</returns>
		public ColorText ListInfo(bool beforeNotAfter) => new ColorText(new string[] { beforeNotAfter ? string.Empty : $", {(Name.Equals(string.Empty) ? Name : $"{Name}, ")}turned {(IsOn ? "on" : "off")}" }, new ConsoleColor[] { ConsoleColor.White });

		/// <summary>
		/// Information about computer
		/// </summary>
		/// <returns>Computer type, ID, power-state, and Brand + Family + Model</returns>
		public ColorText ToText() => new ColorText(new string[] { $"{ComputerType} Computer, ID:{id}\nCurrently powered {(IsOn ? "on" : "off")}\nIt is a(n) {Brand} {Family} {Model}" }, new ConsoleColor[] { ConsoleColor.White });
	}
}
