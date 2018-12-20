using System;

namespace HouseCS.ConsoleUtils {
	/// <summary>
	/// Array of strings, and Array of colors. Used for outputting colored text
	/// </summary>
	public struct ColorText {
		/// <summary>
		/// ColorText object with white empty string
		/// </summary>
		public static ColorText Empty => new ColorText(new string[] { string.Empty }, new ConsoleColor[] { ConsoleColor.White });

		/// <summary>
		/// text string array
		/// </summary>
		public string[] Lines { get; private set; }

		/// <summary>
		/// text color array
		/// </summary>
		public ConsoleColor[] Colors { get; private set; }

		/// <summary>
		/// Creates a ColorText object with a white line
		/// </summary>
		/// <param name="line">line to be white</param>
		public ColorText(string line) : this(new string[] { line }, new ConsoleColor[] { ConsoleColor.White }) { }

		/// <summary>
		/// Creates a ColorText object with a color line
		/// </summary>
		/// <param name="line">text to be color</param>
		/// <param name="color">color for text</param>
		public ColorText(string line, ConsoleColor color) : this(new string[] { line }, new ConsoleColor[] { color }) { }

		/// <summary>
		/// Creates a ColorText object with lines set to colors
		/// </summary>
		/// <param name="lines">strings to be colored</param>
		/// <param name="colors">colors for strings</param>
		public ColorText(string[] lines, ConsoleColor[] colors) {
			Lines = lines;
			Colors = colors;
		}
	}
}
