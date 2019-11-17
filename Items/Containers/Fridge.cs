using HouseCS.ConsoleUtils;
using System;
using System.Collections.Generic;

namespace HouseCS.Items.Containers
{
	/// <summary>
	/// Fridges may contain many items, though they do have some limitations, they may also have freezers, however they are not yet implemented in any meaningful way.
	/// </summary>
	public class Fridge : Container, IItem
	{
		private const string typeS = "Fridge";

		private double temperature = 35.0;

		private bool celsius = false;

		private double freezerTemp = 0.0;

		private bool freezerCelsius = false;

		/// <summary>
		/// Creates empty fridge, without freezer, and with no name
		/// </summary>
		public Fridge() : this(new List<IItem>(), false, string.Empty) { }

		/// <summary>
		/// Creates fridge
		/// </summary>
		/// <param name="items">Items in fridge</param>
		/// <param name="hasFreezer">Whether or not this fridge has a freezer</param>
		/// <param name="name">Name of fridge</param>
		public Fridge(List<IItem> items, bool hasFreezer, string name) : base(items, name) => HasFreezer = hasFreezer;

		/// <summary>
		/// Whether or not this fridge has a freezer
		/// </summary>
		public bool HasFreezer { get; private set; }

		/// <summary>
		/// string of item sub-type
		/// </summary>
		public new string SubType => typeS;

		/// <summary>
		/// Matches keywords against item data
		/// </summary>
		/// <param name="keywords">Keywords to search for</param>
		/// <returns>String output if keywords matched</returns>
		public new List<ColorText> Search(List<string> keywords)
		{
			if (keywords is null)
				throw new ArgumentNullException(nameof(keywords));
			List<ColorText> output = new List<ColorText>();
			foreach (string key in keywords) {
				if ((Size == 0 && key.Equals("Empty", StringComparison.OrdinalIgnoreCase)) ||
				(HasFreezer && key.Equals("Freezer", StringComparison.OrdinalIgnoreCase)) ||
				key.Equals(Name, StringComparison.OrdinalIgnoreCase)) {
					output.Add(ListInfo(true));
					output.Add(new ColorText(typeS));
					output.Add(ListInfo(false));
				}
			}
			if (Size > 0) {
				for (int i = 0; i < Items.Count; i++) {
					List<ColorText> temp = Items[i].Search(keywords);
					if (output.Count == 0 && temp.Count > 0) output.Add(new ColorText("Fridge:\n"));
					if (temp.Count > 0) {
						output.Add(new ColorText($"\t{i}: "));
						output.AddRange(temp);
						output.Add(new ColorText("\n"));
					}
				}
			}
			return output;
		}

		/// <summary>
		/// Exports fridge information
		/// </summary>
		/// <param name="space">How many spaces to start the string with</param>
		/// <returns>Copyable constructor of fridge</returns>
		public new string Export(int space)
		{
			string retStr = string.Empty;
			for (int i = 0; i < space; i++)
				retStr += " ";
			retStr += $"new Fridge(new List<IItem>() {{{(Items.Count > 0 ? "\n" : " ")}";
			for (int i = 0; i < Items.Count; i++) {
				if (Items[i] is Container) {
					retStr += Items[i].SubType switch
					{
						"Bookshelf" => ((Bookshelf)Items[i]).Export(space + 2),
						"Dresser" => ((Dresser)Items[i]).Export(space + 2),
						"Fridge" => ((Fridge)Items[i]).Export(space + 2),
						"Table" => ((Table)Items[i]).Export(space + 2),
						_ => ((Container)Items[i]).Export(space + 2),
					};
					continue;
				}
				if (Items[i] is Display) {
					retStr += $"{((Display)Items[i]).Export(space + 2)}\n";
					continue;
				}
				for (int s = 0; s < space + 2; s++)
					retStr += " ";
				retStr += $"{Items[i].Export()}\n";
			}
			if (Items.Count > 0)
				for (int i = 0; i < space; i++)
					retStr += " ";
			return $"{retStr}}}, {(HasFreezer ? "true" : "false")}, \"{Name}\"),\n";
		}

		/// <summary>
		/// Converts between celsius and farenheit for the fridge, or the freezer
		/// </summary>
		/// <param name="setCelsius">True to set to celsius, false for farenheit</param>
		/// <param name="setFreezer">True to set freezer, false for fridge</param>
		public void ChangeType(bool setCelsius, bool setFreezer)
		{
			if (setFreezer) {
				if (freezerCelsius != setCelsius)
					freezerTemp = setCelsius ? (freezerTemp - 32.0) * 5.0 / 9.0 : freezerTemp * 9.0 / 5.0 + 32.0;
				freezerCelsius = setCelsius;
			}
			else {
				if (celsius != setCelsius)
					temperature = setCelsius ? (temperature - 32.0) * 5.0 / 9.0 : temperature * 9.0 / 5.0 + 32.0;
				celsius = setCelsius;
			}
		}

		/// <summary>
		/// Switches temperature of fridge or freezer to celsius
		/// </summary>
		/// <param name="freezer">True to set freezer, False to set fridge</param>
		[Obsolete("Deprecated method, please use ChangeType(true, bool);")]
		public void ToCelsius(bool freezer) => ChangeType(true, freezer);

		/// <summary>
		/// Switches temperature of fridge or freezer to farenheit
		/// </summary>
		/// <param name="freezer">True to set freezer, False to set fridge</param>
		[Obsolete("Deprecated method, please use ChangeType(false, bool);")]
		public void ToFarenheit(bool freezer) => ChangeType(false, freezer);

		/// <summary>
		/// Increment the temperature by 1
		/// </summary>
		[Obsolete("Deprecated method, please use TempChange(double) instead")]
		public void TempInc() => temperature += 1.0;

		/// <summary>
		/// Decrement the temperature by 1
		/// </summary>
		[Obsolete("Deprecated method, please use TempChange(double) instead")]
		public void TempDec() => temperature -= 1.0;

		/// <summary>
		/// Changes temperature of fridge or freezer
		/// </summary>
		/// <param name="newTemp">Degrees to add to temperature</param>
		/// <param name="freezer">True to set freezer temp, false for fridge</param>
		public void ChangeTemp(double newTemp, bool freezer)
		{
			if (freezer)
				freezerTemp += newTemp;
			else
				temperature += newTemp;
		}

		/// <summary>
		/// Changes temperature of fridge
		/// </summary>
		/// <param name="newTemp">New temperature for fridge</param>
		[Obsolete("Deprecated method, please use ChangeTemp(double, false) instead")]
		public void TempChange(double newTemp) => ChangeTemp(newTemp, false);

		/// <summary>
		/// Resets temperature, and sets to farenheit
		/// </summary>
		[Obsolete("Deprecated method, please use ChangeType and ChangeTemp")]
		public void TempReset()
		{
			ChangeType(false, false);
			ChangeType(false, true);
			ChangeTemp(35.0 - temperature, false);
			ChangeTemp(0.0 - freezerTemp, true);
		}

		/// <summary>
		/// Minor details for list
		/// </summary>
		/// <param name="beforeNotAfter">True for left side, False for right side</param>
		/// <returns>ColorText object of minor fridge details</returns>
		public new ColorText ListInfo(bool beforeNotAfter) => beforeNotAfter
			? new ColorText($"{(string.IsNullOrEmpty(Name) ? string.Empty : $"{Name} - ")}{temperature}° ", ConsoleColor.White)
			: Size > 0
				? new ColorText(new string[] { " - ", Size.ToString(), " Items", $"{(HasFreezer ? $", with {freezerTemp}° Freezer - " : "")}" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.White })
				: new ColorText(new string[] { " - ", "Empty", $"{(HasFreezer ? $", with {freezerTemp}° Freezer - " : "")}" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White });

		/// <summary>
		/// Information about fridge
		/// </summary>
		/// <returns>ColorText object of important info</returns>
		public new ColorText ToText()
		{
			List<string> retStr = new List<string>() { "Items", " in this ", "Fridge", ":" };
			List<ConsoleColor> retClr = new List<ConsoleColor>() { ConsoleColor.DarkYellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White };
			for (int i = 0; i < Size; i++) {
				retStr.Add($"\n\t{i.ToString()}");
				retClr.Add(ConsoleColor.Cyan);
				retStr.Add(": ");
				retClr.Add(ConsoleColor.White);
				ColorText left = GetItem(i).ListInfo(true);
				foreach (string str in left.GetLines())
					retStr.Add(str);
				foreach (ConsoleColor clr in left.Colors())
					retClr.Add(clr);
				retStr.Add(GetItem(i).SubType);
				retClr.Add(ConsoleColor.Yellow);
				ColorText right = GetItem(i).ListInfo(false);
				foreach (string str in right.GetLines())
					retStr.Add(str);
				foreach (ConsoleColor clr in right.Colors())
					retClr.Add(clr);
			}
			retStr.Add("\nEnd of ");
			retClr.Add(ConsoleColor.White);
			retStr.Add("Fridge");
			retClr.Add(ConsoleColor.Yellow);
			retStr.Add(" contents.");
			retClr.Add(ConsoleColor.White);
			return new ColorText(retStr.ToArray(), retClr.ToArray());
		}
	}
}
