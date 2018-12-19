using System;

namespace HouseCS.ConsoleUtils {
	public struct ColorText {
		public ColorText(string line) : this(new string[] { line }, new ConsoleColor[] { ConsoleColor.White }) { }
		public ColorText(string line, ConsoleColor color) : this(new string[] { line }, new ConsoleColor[] { color }) { }
		public ColorText(string[] lines, ConsoleColor[] colors) {
			Lines = lines;
			Colors = colors;
		}

		public static ColorText Empty => new ColorText(new string[] { string.Empty }, new ConsoleColor[] { ConsoleColor.White });
		public string[] Lines { get; private set; }
		public ConsoleColor[] Colors { get; private set; }
	}
}
