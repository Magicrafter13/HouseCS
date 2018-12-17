using System.Collections.Generic;
using HouseCS.Items;
using HouseCS.Items.Containers;

namespace HouseCS
{
	public class Floor
	{
		public Floor() : this(new List<IItem>()) { }
		public Floor(List<IItem> i) => Items = i;
		public void AddItem(IItem i) => Items.Add(i);
		public void RemoveItem(int i) => Items.RemoveAt(i);
		public void RemoveItem(IItem i) => Items.Remove(i);
		public bool RemoveItem(int iN, int sIN) {
			int removeFromFloor = -1;
			for (int i = 0; i < Items.Count; i++) {
				switch(Items[iN].Type) {
				case "Container": case "Bookshelf": case "Fridge":
					if (((Container)Items[iN]).GetItem(sIN) == Items[i]) removeFromFloor = i;
					break;
				case "Display":
					if (((Display)Items[iN]).GetDevice(sIN) == Items[i]) removeFromFloor = i;
					break;
				}
			}
			switch (Items[iN].Type) {
			case "Container":
				((Container)Items[iN]).RemoveItem(sIN);
				break;
			case "Display":
				((Display)Items[iN]).Disconnect(sIN);
				break;
			default: return false;
			}
			if (removeFromFloor > -1) Items.RemoveAt(removeFromFloor);
			return true;
		}
		public List<IItem> Items { get; set; }
		public IItem GetItem(int i) => i >= 0 && i < Items.Count ? Items[i] : new Empty();
		public IItem GetItem(int i, int sI) {
			if (i >= 0 && i < Items.Count)
			{
				IItem ret_val = Items[i];
				if (Items[i] is Container)
					ret_val = ((Container)Items[i]).GetItem(sI);
				if (Items[i] is Display)
					ret_val = ((Display)Items[i]).GetDevice(sI);
				return ret_val;
			}
			return new Empty();
		}
		public int Size() => Items.Count;
		public override string ToString() => $"This floor has {Items.Count} items on it.";
	}
}
