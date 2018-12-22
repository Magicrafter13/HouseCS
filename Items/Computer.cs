﻿using System;
using HouseCS.ConsoleUtils;

namespace HouseCS.Items {
	/// <summary>
	/// Computer, has a Brand, Family, Model, power state, and unique id
	/// </summary>
	public class Computer : IItem {
		private static int totalComps = 0;

		private readonly int id;

		private static readonly string typeS = "Computer";

		/// <summary>
		/// Room the computer is in
		/// </summary>
		public int RoomID { get; private set; }

		/// <summary>
		/// string to indicate type of computer
		/// </summary>
		public string ComputerType { get; private set; }

		/// <summary>
		/// Whether or not the computer is powered on
		/// </summary>
		public bool IsOn { get; private set; }

		/// <summary>
		/// Computer brand
		/// </summary>
		public string Brand { get; private set; }

		/// <summary>
		/// Computer family line
		/// </summary>
		public string Family { get; private set; }

		/// <summary>
		/// Computer family model
		/// </summary>
		public string Model { get; private set; }

		/// <summary>
		/// string of Item type
		/// </summary>
		public string Type => typeS;

		/// <summary>
		/// string of Item sub-type
		/// </summary>
		public string SubType => typeS;

		/// <summary>
		/// Does the same as the constructor, sets brand, family, model, power state, and type
		/// </summary>
		/// <param name="brand">Computer brand</param>
		/// <param name="family">Computer family line</param>
		/// <param name="model">Computer family model</param>
		/// <param name="state">Computer power state</param>
		/// <param name="type">Computer type</param>
		/// <param name="id">Room ID</param>
		public void Reset(string brand, string family, string model, bool state, string type, int id) {
			ComputerType = type;
			IsOn = state;
			Brand = brand;
			Family = family;
			Model = model;
			RoomID = id;
		}

		/// <summary>
		/// Don't use
		/// </summary>
		/// <param name="item">test Item</param>
		/// <returns>false</returns>
		public bool HasItem(IItem item) => false;

		/// <summary>
		/// Turns on the computer
		/// </summary>
		public void TurnOn() => IsOn = true;

		/// <summary>
		/// Turns off the computer
		/// </summary>
		public void TurnOff() => IsOn = false;

		/// <summary>
		/// Minor details for list
		/// </summary>
		/// <param name="beforeNotAfter">True for left side, False for right side</param>
		/// <returns>ColorText object of minor computer details</returns>
		public ColorText ListInfo(bool beforeNotAfter) => new ColorText(new string[] { beforeNotAfter ? string.Empty : $", turned {(IsOn ? "on" : "off")}" }, new ConsoleColor[] { ConsoleColor.White });

		/// <summary>
		/// Information about computer
		/// </summary>
		/// <returns>ColorText object of important info</returns>
		public ColorText ToText() => new ColorText(new string[] { $"{ComputerType} Computer, ID:{id}\nCurrently powered {(IsOn ? "on" : "off")}\nIt is a(n) {Brand} {Family} {Model}" }, new ConsoleColor[] { ConsoleColor.White });

		/// <summary>
		/// Creates a "Generic" brand, "PC", [no model], Desktop computer that is turned off
		/// </summary>
		public Computer() : this("Generic", "PC", "", false, "Desktop", -1) { }

		/// <summary>
		/// Creates a brand, family model, computer, with a set power state, and of set type
		/// </summary>
		/// <param name="brand">Computer brand</param>
		/// <param name="family">Computer family line</param>
		/// <param name="model">Computer family model</param>
		/// <param name="state">Computer power state</param>
		/// <param name="type">Computer type</param>
		/// <param name="room">Room for computer</param>
		public Computer(string brand, string family, string model, bool state, string type, int room) {
			Reset(brand, family, model, state, type, room);
			id = totalComps;
			totalComps++;
		}
	}
}