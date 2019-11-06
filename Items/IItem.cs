using HouseCS.ConsoleUtils;
using System.Collections.Generic;

namespace HouseCS.Items
{
	/// <summary>
	/// Interface for all floor Items (the basis of this software)
	/// </summary>
	public interface IItem
	{

		/// <summary> String of item parent type </summary>
		string Type { get; }

		/// <summary> String of item sub type </summary>
		string SubType { get; }

		/// <summary> String of item's assigned name </summary>
		string Name { get; }

		/// <summary>
		/// Matches keywords against item data
		/// </summary>
		/// <param name="keywords">Keywords to search for</param>
		/// <returns>String output if keywords matched</returns>
		List<ColorText> Search(List<string> keywords);

		/// <summary>
		/// Exports item information
		/// </summary>
		/// <returns>Copyable constructor of the item</returns>
		string Export();

		/// <summary>
		/// Tests if item has sub item
		/// </summary>
		/// <param name="item">Test item</param>
		/// <returns>True if item has sub item, False if not</returns>
		bool HasItem(IItem item);

		/// <summary>
		/// Changes the name of the item
		/// </summary>
		/// <param name="name">New name</param>
		public void Rename(string name);

		/// <summary>
		/// Minor details for list
		/// </summary>
		/// <param name="beforeNotAfter">True for left side, False for right side</param>
		/// <returns>ColorText object of minor Item details</returns>
		ColorText ListInfo(bool beforeNotAfter);

		/// <summary>
		/// Information about Item
		/// </summary>
		/// <returns>ColorText object of important info</returns>
		ColorText ToText();
	}
}
