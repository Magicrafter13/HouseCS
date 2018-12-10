using System;
using System.Collections.Generic;
using HouseCS.Items;

namespace HouseCS
{
	public class House
	{
		private static readonly string[] colors = { "White", "Red", "Brown", "Orange", "Yellow", "Green", "Blue", "Purple", "Pink", "Black" };
		public static readonly string[] types = { "*", "Book", "Bookshelf", "Computer", "Console", "Display" };
		private readonly int color;

		private void InitializeFloors()
		{
			for (int i = 0; i < GetFloors.Length; i++)
				GetFloors[i] = new Floor();
		}
		public House() : this(0, 1) { }
		public House(int c, int f)
		{
			color = c >= 0 && c <= 9 ? c : 0;
			Size = f > 0 ? f : 1;
			GetFloors = new Floor[Size];
			InitializeFloors();
		}
		public string List(int f, int s, int e, string type)
		{
			bool validType = false;
			foreach (string t in types)
				if (type.Equals(t, StringComparison.OrdinalIgnoreCase))
					validType = true;
			if (!validType)
				return $"{type} is not a valid item type.";
			if (GetFloors[f].Size() == 0)
				return "Floor is empty!";
			if (s >= e)
				return "Start must be less than End";
			if (s < 0)
				return "Start must be greater than or equal to 0";
			string retVal = string.Empty;
			List<IItem> items = new List<IItem>();
			List<int> itemIds = new List<int>();
			for (int i = s; i < e; i++)
			{
				if (i > GetFloors[f].Size())
					continue;
				if (type.Equals("*") || type.Equals(GetFloors[f].GetItem(i).Type, StringComparison.OrdinalIgnoreCase))
				{
					items.Add(GetFloors[f].GetItem(i));
					itemIds.Add(i);
				}
			}
			if (items.Count == 0) return $"Floor has no {type} items.";
			for (int i = 0; i < items.Count; i++)
			{
				retVal += $"{itemIds[i]}: {items[i].ListInfo(true)}{items[i].Type}{items[i].ListInfo(false)}";
				if (i < items.Count - 1) retVal += "\n";
			}
			return $"\n{retVal}\n";
		}
		public string List(int f) => List(f, 0, GetFloors[f].Size(), "*");
		public string List(int f, string type) => List(f, 0, GetFloors[f].Size(), type);
		public int Size { get; }
		public bool AddItem(int f, IItem i) {
			if (f < 0 || f >= Size)
				return false;
			GetFloors[f].AddItem(i);
			return true;
		}
		public IItem GetItem(int f, int i) => GetFloors[f].GetItem(i);
		public IItem GetItem(int f, int i, int si) => GetFloors[f].GetItem(i, si);
		public Floor GetFloor(int f) => GetFloors[f];

		public Floor[] GetFloors { get; }

		public override string ToString() => $"Color: {colors[color]}\nFloors: {Size}";
	}
}
