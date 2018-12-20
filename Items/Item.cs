﻿using HouseCS.ConsoleUtils;

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
