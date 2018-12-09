namespace HouseCS.Items
{
	public class GameConsole : IItem
	{
		private static readonly string typeS = "Console";
		public static readonly string[] types = { "Console", "Handheld", "Hybrid System" };
		private readonly int sysType;
		private readonly string company;
		private readonly string system;

		public GameConsole() : this(0, "Generi-sys", "Generic System 1000") { }
		public GameConsole(int type, string c, string s)
		{
			sysType = type >= 0 && type < types.Length ? type : 0;
			company = c;
			system = s;
		}
		public string Type => typeS;
		public string ListInfo(bool beforeNotAfter) => beforeNotAfter ? company + " " : $" - {types[sysType]}";
		public override string ToString() => $"This Video Game {types[sysType]}, is a {company}\n{system}";
	}
}
