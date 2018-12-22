using System;
using System.Collections.Generic;
using HouseCS.ConsoleUtils;

namespace HouseCS.Items.Containers {
	/// <summary>
	/// Fridge, has Items
	/// </summary>
	public class Fridge : Container, IItem {
		private const string typeS = "Fridge";

		private double temperature = 35.0;

		private bool celsius = false;

		private double freezerTemp = 0.0;

		private bool freezerCelsius = false;

		/// <summary>
		/// Whether or not this fridge has a freezer
		/// </summary>
		public bool HasFreezer { get; private set; }

		/// <summary>
		/// string of Item sub-type
		/// </summary>
		public new string SubType => typeS;

		/// <summary>
		/// Switches temperature of fridge or freezer to celsius
		/// </summary>
		/// <param name="freezer">True to set freezer, False to set fridge</param>
		public void ToCelsius(bool freezer) {
			if (freezer) {
				if (!freezerCelsius) {
					freezerCelsius = true;
					freezerTemp = (freezerTemp - 32.0) * 5.0 / 9.0;
				}
			} else {
				if (!celsius) {
					celsius = true;
					temperature = (temperature - 32.0) * 5.0 / 9.0;
				}
			}
		}

		/// <summary>
		/// Switches temperature of fridge or freezer to farenheit
		/// </summary>
		/// <param name="freezer">True to set freezer, False to set fridge</param>
		public void ToFarenheit(bool freezer) {
			if (freezer) {
				if (freezerCelsius) {
					freezerCelsius = false;
					freezerTemp = freezerTemp * 9.0 / 5.0 + 32.0;
				}
			} else {
				if (celsius) {
					celsius = false;
					temperature = temperature * 9.0 / 5.0 + 32.0;
				}
			}
		}

		/// <summary>
		/// Increment the temperature by 1
		/// </summary>
		public void TempInc() => temperature += 1.0;

		/// <summary>
		/// Decrement the temperature by 1
		/// </summary>
		public void TempDec() => temperature -= 1.0;

		/// <summary>
		/// Changes temperature of fridge
		/// </summary>
		/// <param name="newTemp">New temperature for fridge</param>
		public void TempChange(double newTemp) => temperature += newTemp;

		/// <summary>
		/// Resets temperature, and sets to farenheit
		/// </summary>
		public void TempReset() {
			celsius = false;
			temperature = 35.0;
		}

		/// <summary>
		/// Minor details for list
		/// </summary>
		/// <param name="beforeNotAfter">True for left side, False for right side</param>
		/// <returns>ColorText object of minor fridge details</returns>
		public new ColorText ListInfo(bool beforeNotAfter) => beforeNotAfter
			? new ColorText($"{temperature}° ", ConsoleColor.White)
			: Size > 0
				? new ColorText(new string[] { " - ", Size.ToString(), " Items", $"{(HasFreezer ? $", with {freezerTemp}° Freezer - " : "")}" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.White })
				: new ColorText(new string[] { " - ", "Empty", $"]{(HasFreezer ? $", with {freezerTemp}° Freezer - " : "")}" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White });

		/// <summary>
		/// Information about fridge
		/// </summary>
		/// <returns>ColorText object of important info</returns>
		public new ColorText ToText() {
			List<string> retStr = new List<string>() { "Items", " in this ", "Fridge", ":" };
			List<ConsoleColor> retClr = new List<ConsoleColor>() { ConsoleColor.DarkYellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White };
			for (int i = 0; i < Size; i++) {
				retStr.Add($"\n\t{i.ToString()}");
				retClr.Add(ConsoleColor.Cyan);
				retStr.Add(": ");
				retClr.Add(ConsoleColor.White);
				ColorText left = GetItem(i).ListInfo(true);
				foreach (string str in left.Lines)
					retStr.Add(str);
				foreach (ConsoleColor clr in left.Colors)
					retClr.Add(clr);
				retStr.Add(GetItem(i).SubType);
				retClr.Add(ConsoleColor.Yellow);
				ColorText right = GetItem(i).ListInfo(false);
				foreach (string str in right.Lines)
					retStr.Add(str);
				foreach (ConsoleColor clr in right.Colors)
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

		/// <summary>
		/// Creates empty fridge, without freezer
		/// </summary>
		public Fridge() : this(new List<IItem>(), false, -1) { }

		/// <summary>
		/// Creates fridge with Items, without freezer
		/// </summary>
		/// <param name="items">Items in fridge</param>
		/// <param name="hasFreezer">Whether or not this fridge has a freezer</param>
		/// <param name="room">Room for fridge</param>
		public Fridge(List<IItem> items, bool hasFreezer, int room) : base(items, room) => HasFreezer = hasFreezer;

		/*/// <summary>
		/// Creates empty fridge, with or without a freezer
		/// </summary>
		/// <param name="hasFreezer">Whether or not this fridge has a freezer</param>
		public Fridge(bool hasFreezer) : base() => HasFreezer = hasFreezer;

		/// <summary>
		/// Creates empty fridge with Items, with or without a freezer
		/// </summary>
		/// <param name="items">Items in fridge</param>
		/// <param name="hasFreezer">Whether or not this fridge has a freezer</param>
		public Fridge(List<IItem> items, bool hasFreezer) : base(items) => HasFreezer = hasFreezer;*/
	}
}