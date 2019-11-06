using System;

namespace HouseCS.ConsoleUtils
{
	/// <summary>
	/// Array of strings, and Array of colors. Used for outputting colored text
	/// </summary>
	public struct ColorText
	{
		private readonly ConsoleColor[] colors;

		private readonly string[] lines;

		/// <summary>
		/// Creates a ColorText object with a white line
		/// </summary>
		/// <param name="line">line to be white</param>
		public ColorText(string line) : this(new string[] { line }, new ConsoleColor[] { ConsoleColor.White }) { }

		/// <summary>
		/// Creates a ColorText object with a color line
		/// </summary>
		/// <param name="line">Text</param>
		/// <param name="color">Text's color</param>
		public ColorText(string line, ConsoleColor color) : this(new string[] { line }, new ConsoleColor[] { color }) { }

		/// <summary>
		/// Creates a ColorText object with lines set to colors
		/// </summary>
		/// <param name="l">Each string</param>
		/// <param name="c">Color for each string</param>
		public ColorText(string[] l, ConsoleColor[] c)
		{
			lines = l;
			colors = c;
		}

		/// <summary> ColorText object with white empty string </summary>
		public static ColorText Empty => new ColorText(new string[] { string.Empty }, new ConsoleColor[] { ConsoleColor.White });

		/// <summary> Text strings </summary>
		public string[] GetLines() => lines;

		/// <summary> Color for each string </summary>
		public ConsoleColor[] Colors() => colors;
	}
}
