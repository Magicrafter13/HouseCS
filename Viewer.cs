using HouseCS.Items;

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
		public bool GoFloor(int f)
		{
			if (f < 0 || f >= curHouse.Size)
				return false;
			CurFloor = f;
			return true;
		}
		public string ViewCurItem => $"Object type is: {curItem.Type}\n\n{curItem}";
		public int FloorSize => curHouse.GetFloor(CurFloor).Size();
		public string List() => curHouse.List(CurFloor);
		public string List(int s, int e) => curHouse.List(CurFloor, s, e, "*");
		public string List(string type) => curHouse.List(CurFloor, type);
		public void AddItem(IItem i) => curHouse.AddItem(CurFloor, i);
		public void RemoveItem(int r)
		{
			IItem temp = curHouse.GetFloor(CurFloor).GetItem(r);
			//any Item that can have this item will have it removed
			foreach (Floor f in curHouse.GetFloors)
			{
				foreach (IItem i in f.GetItems())
				{
					switch (i.Type)
					{
						case "Bookshelf":
							for (int b = 0; b < ((Bookshelf)i).BookCount; b++)
								if (((Bookshelf)i).GetBook(b) == temp)
									((Bookshelf)i).RemoveBook(b);
							break;
						case "Display":
							for (int d = 0; d < ((Display)i).DeviceCount; d++)
								if (((Display)i).GetDevice(d) == temp)
									((Display)i).Disconnect(d);
							break;
					}
				}
			}
			curHouse.GetFloor(CurFloor).RemoveItem(r);
		}
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
		public bool ChangeItemFocus(int i)
		{
			if (i < 0 || i >= curHouse.GetFloor(CurFloor).Size())
				return false;
			curItem = curHouse.GetItem(CurFloor, i);
			return true;
		}
		public int ChangeItemFocus(int i, int si)
		{
			if (i < 0 || i >= curHouse.GetFloor(CurFloor).Size())
				return 2;
			curItem = curHouse.GetItem(CurFloor, i, si);
			return curItem is Empty ? 1 : 0;
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
