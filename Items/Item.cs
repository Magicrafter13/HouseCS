namespace HouseCS.Items
{
	public interface IItem
	{
		string Type { get; }

		string ListInfo(bool beforeNotAfter);
		string ToString();
	}
}
