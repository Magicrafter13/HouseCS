using HouseCS.ConsoleUtils;
using System.Collections.Generic;

namespace HouseCS.Items {
	/// <summary>
	/// Interface for all floor Items (the basis of this software)
	/// </summary>
	public interface IItem {

		/// <summary>
		/// string of Item parent type
		/// </summary>
		string Type { get; }

		/// <summary>
		/// string of Item sub type
		/// </summary>
		string SubType { get; }

		/// <summary>
		/// string of Item's assigned name
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Matches keyword against Item data
		/// </summary>
		/// <param name="keywords">Keywords to search for</param>
		/// <returns>String output if keywords matched</returns>
		List<ColorText> Search(List<string> keywords);

		/// <summary>
		/// Exports Item information
		/// </summary>
		/// <returns>String containing a constructor for the Item</returns>
		string Export();

		/// <summary>
		/// Tests if Item has sub Item
		/// </summary>
		/// <param name="item">test Item</param>
		/// <returns>True if Item has sub Item, False if not</returns>
		bool HasItem(IItem item);

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
