using HouseCS.ConsoleUtils;

namespace HouseCS.Items
{
	public interface IItem
	{
		string Type { get; }
		string SubType { get; }

		bool HasItem(IItem test);
		ColorText ListInfo(bool beforeNotAfter);
		ColorText ToText();
	}
}
