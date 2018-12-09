namespace HouseCS.Items
{
	public class Empty : IItem
	{
		private static readonly string message = "You have no items/objects selected";
		private static readonly string typeS = "No Item";

		public Empty() { }
		public IItem GetSub(int i) => new Book("This item doesn't contain other items.", "(I don't think it should be possible to see this...)", 2018);
		public string Type => typeS;
		public string ListInfo(bool beforeNotAfter) => beforeNotAfter ? "" : "";
		public override string ToString() => message;
	}
}
