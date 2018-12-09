using System.Collections.Generic;
using HouseCS.Items;

namespace HouseCS
{
	public class Floor
	{
		private readonly List<IItem> Items;

		public Floor() : this(new List<IItem>()) { }
		public Floor(List<IItem> i) => Items = i;
		public void AddItem(IItem i) => Items.Add(i);
		public void RemoveItem(int i) => Items.RemoveAt(i);
		public List<IItem> GetItems() => Items;
		public IItem GetItem(int i) => i >= 0 && i < Items.Count ? Items[i] : new Empty();
		public IItem GetItem(int i, int si)
		{
			if (i >= 0 && i < Items.Count)
			{
				IItem ret_val = Items[i];
				if (Items[i] is Bookshelf)
					ret_val = ((Bookshelf)Items[i]).GetBook(si);
				if (Items[i] is Display)
					ret_val = ((Display)Items[i]).GetDevice(si);
				return ret_val;
			}
			return new Empty();
		}
		public int Size() => Items.Count;
		public override string ToString() => $"This floor has {Items.Count} items on it.";
	}
}
