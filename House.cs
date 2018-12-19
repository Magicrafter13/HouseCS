using System;
using System.Collections.Generic;
using HouseCS.ConsoleUtils;
using HouseCS.Items;

namespace HouseCS {
	public class House {
		private static readonly string[] colors = { "White", "Red", "Brown", "Orange", "Yellow", "Green", "Blue", "Purple", "Pink", "Black" };
		public static readonly string[] types = { "*", "Bed", "Book", "Computer", "Console", "Display",
			"Bookshelf", "Container", "Dresser", "Fridge", "Table",
			"Clothing", "Pants", "Shirt" };
		private readonly int color;

		private void InitializeFloors() {
			for (int i = 0; i < GetFloors.Length; i++)
				GetFloors[i] = new Floor();
		}
		public House() : this(0, 1) { }
		public House(int c, int f) {
			color = c >= 0 && c <= 9 ? c : 0;
			Size = f > 0 ? f : 1;
			GetFloors = new Floor[Size];
			InitializeFloors();
		}
		public House(int c, Floor[] fs) {
			color = c >= 0 && c <= 9 ? c : 0;
			Size = fs.Length;
			GetFloors = fs;
		}
		public int PageCount(int f, int rangeStart, int rangeEnd, String searchType, int pageLength) {
			bool validType = false;
			foreach (String t in types)
				if (searchType.Equals(t, StringComparison.OrdinalIgnoreCase))
					validType = true;
			if (!validType)
				return -1;
			if (GetFloors[f].Size() == 0)
				return -2;
			if (rangeStart >= rangeEnd)
				return -3;
			if (rangeStart < 0)
				return -4;
			int items = 0;
			for (int i = rangeStart; i < rangeEnd; i++) {
				if (i > GetFloors[f].Size())
					continue;
				if (searchType.Equals("*") ||
					searchType.Equals(GetFloors[f].GetItem(i).SubType, StringComparison.OrdinalIgnoreCase) ||
					searchType.Equals(GetFloors[f].GetItem(i).Type)) {
					items++;
				}
			}
			return items / pageLength + (items % pageLength == 0 ? 0 : 1);
		}
		public ColorText List(int f, int s, int e, string type, int pageLength, int page) {
			bool validType = false;
			foreach (string t in types)
				if (type.Equals(t, StringComparison.OrdinalIgnoreCase))
					validType = true;
			if (!validType)
				return new ColorText(new string[] { type, " is not a valid ", "Item", " type." }, new ConsoleColor[] { ConsoleColor.DarkYellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
			if (GetFloors[f].Size() == 0)
				return new ColorText(new string[] { "Floor is empty!" }, new ConsoleColor[] { ConsoleColor.White });
			if (s >= e)
				return new ColorText(new string[] { "Start", " must be less than ", "End" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Red });
			if (s < 0)
				return new ColorText(new string[] { "Start", " must be greater than or equal to ", "0" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan });
			List<string> retStr = new List<string>() { "\n" };
			List<ConsoleColor> retClr = new List<ConsoleColor>() { ConsoleColor.White };
			List<IItem> items = new List<IItem>();
			List<int> itemIds = new List<int>();
			for (int i = s; i < e; i++) {
				if (i > GetFloors[f].Size())
					continue;
				if (type.Equals("*") ||
					type.Equals(GetFloors[f].GetItem(i).Type, StringComparison.OrdinalIgnoreCase) ||
					type.Equals(GetFloors[f].GetItem(i).SubType, StringComparison.OrdinalIgnoreCase)) {
					items.Add(GetFloors[f].GetItem(i));
					itemIds.Add(i);
				}
			}
			if (items.Count == 0)
				return new ColorText(new string[] { "Floor has no ", $"{type} Items", "." }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
			for (int i = pageLength * page; i < pageLength * (page + 1); i++) {
				if (i >= items.Count)
					continue;
				retStr.Add($"{itemIds[i]}");
				retClr.Add(ConsoleColor.Cyan);
				retStr.Add(": ");
				retClr.Add(ConsoleColor.White);
				ColorText left = items[i].ListInfo(true);
				foreach (string str in left.Lines)
					retStr.Add(str);
				foreach (ConsoleColor clr in left.Colors)
					retClr.Add(clr);
				retStr.Add(items[i].SubType);
				retClr.Add(ConsoleColor.Yellow);
				ColorText right = items[i].ListInfo(false);
				foreach (string str in right.Lines)
					retStr.Add(str);
				foreach (ConsoleColor clr in right.Colors)
					retClr.Add(clr);
				retStr.Add("\n");
				retClr.Add(ConsoleColor.White);
			}
			return new ColorText(retStr.ToArray(), retClr.ToArray());
		}
		public ColorText List(int f) => List(f, 0, GetFloors[f].Size(), "*", GetFloors[f].Size(), 0);
		public ColorText List(int f, string type) => List(f, 0, GetFloors[f].Size(), type, GetFloors[f].Size(), 0);
		public int Size { get; }
		public bool AddItem(int f, IItem i) {
			bool check = f >= 0 && f < Size;
			if (check)
				GetFloors[f].AddItem(i);
			return check;
		}
		public IItem GetItem(int f, int i) => GetFloors[f].GetItem(i);
		public IItem GetItem(int f, int i, int si) => GetFloors[f].GetItem(i, si);
		public Floor GetFloor(int f) => GetFloors[f];

		public Floor[] GetFloors { get; }

		public override string ToString() => $"Color: {colors[color]}\nFloors: {Size}";
	}
}
