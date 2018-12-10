namespace HouseCS.Items
{
	public interface IItem
	{
		string Type { get; }

		bool HasItem(IItem test);
		string ListInfo(bool beforeNotAfter);
		string ToString();
	}
}
