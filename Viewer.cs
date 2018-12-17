using System.Collections.Generic;
using System;
using HouseCS.ConsoleUtils;
using HouseCS.Items;
using HouseCS.Items.Containers;

namespace HouseCS
{
	public class Viewer
	{
		public IItem curItem;
		private House curHouse;

		public Viewer() : this(new House()) { }
		public Viewer(House h)
		{
			CurFloor = 0;
			curItem = new Empty();
			curHouse = h;
		}
		public bool IsItem(int i) => i >= 0 && i < curHouse.GetFloor(CurFloor).Size();
		public IItem GetItem(int i) => curHouse.GetItem(CurFloor, i);
		public bool GoFloor(int f) {
			bool check = f >= 0 && f < curHouse.Size;
			if (check) CurFloor = f;
			return check;
		}

		public ColorText GetViewCurItem() {
			List<string> retStr = new List<string>() { $"Object type is: {curItem.Type}\n\n" };
			List<ConsoleColor> retClr = new List<ConsoleColor>() { ConsoleColor.White };
			ColorText itm = curItem.ToText();
			foreach (string str in itm.Lines)
				retStr.Add(str);
			foreach (ConsoleColor clr in itm.Colors)
				retClr.Add(clr);
			return new ColorText(retStr.ToArray(), retClr.ToArray());
		}
		public int FloorSize => curHouse.GetFloor(CurFloor).Size();
		public int PageCount(int s, int e, string t, int l) => curHouse.PageCount(CurFloor, s, e, t, l);
		public ColorText List(int s, int e, string t, int l, int p) => curHouse.List(CurFloor, s, e, t, l, p);
		public ColorText List() => curHouse.List(CurFloor);
		public ColorText List(int s, int e) => curHouse.List(CurFloor, s, e, "*", FloorSize, 0);
		public ColorText List(string type) => curHouse.List(CurFloor, type);
		public void AddItem(IItem i) => curHouse.AddItem(CurFloor, i);
		public void RemoveItem(int iN, int sIN) {
			IItem temp = curHouse.GetFloor(CurFloor).GetItem(iN, sIN);
			if (temp == curItem) curItem = new Empty();
			if (temp.HasItem(curItem)) if (!(temp is Display)) curItem = new Empty();
			//any Item that can have this item will have it removed - currently no sub items have their own sub items
			if (curHouse.GetFloor(CurFloor).RemoveItem(iN, sIN)) {
				foreach (Floor f in curHouse.GetFloors) {
					foreach (IItem i in f.Items) {
						if (i.HasItem(temp)) {
							switch (i.Type) {
								case "Container": ((Container)i).RemoveItem(temp); break;
								case "Display": ((Display)i).Disconnect(temp); break;
							}
						}
					}
				}
			}
		}
		public void RemoveItem(IItem iN) {
			IItem temp = iN;
			if (temp == curItem) curItem = new Empty();
			if (temp.HasItem(curItem)) if (!(temp is Display)) curItem = new Empty();
			//any Item that can have this item will have it removed
			foreach (Floor f in curHouse.GetFloors) {
				foreach (IItem i in f.Items) {
					if (i.HasItem(temp)) {
						switch (i.Type) {
							case "Container": ((Container)i).RemoveItem(temp); break;
							case "Display": ((Display)i).Disconnect(temp); break;
						}
					}
				}
			}
			curHouse.GetFloor(CurFloor).RemoveItem(iN);
		}
		public void RemoveItem(int iN) => RemoveItem(curHouse.GetFloor(CurFloor).GetItem(iN));
		public int CurFloor { get; private set; }
		public string GoUp()
		{
			CurFloor++;
			if (CurFloor < curHouse.Size)
				return $"\nWelcome to floor {CurFloor}.\n";
			CurFloor--;
			return "\nYou are currently on the top floor, floor unchanged.\n";
		}
		public string GoDown()
		{
			if (CurFloor <= 0)
				return "\nYou are currently on the bottom floor, floor unchanged.\n";
			CurFloor--;
			return $"\nWelcome to floor {CurFloor}.\n";
		}
		public bool ChangeItemFocus(int i) {
			bool check = i >= 0 && i < curHouse.GetFloor(CurFloor).Size();
			if (check) curItem = curHouse.GetItem(CurFloor, i);
			return check;
		}
		public int ChangeItemFocus(int i, int sI) {
			if (i >= 0 && i < curHouse.GetFloor(CurFloor).Size()) {
				curItem = curHouse.GetItem(CurFloor, i, sI);
				return curItem is Empty ? 1 : 0;
			}
			return 2;
		}
		public void ChangeHouseFocus(House h)
		{
			CurFloor = 0;
			curItem = new Empty();
			curHouse = h;
		}
		public override string ToString() => $"\tCurrent Floor: {CurFloor}\n\tCurrent Item Type: {curItem.Type}";
	}
}
