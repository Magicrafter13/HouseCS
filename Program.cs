using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HouseCS.ConsoleUtils;
using HouseCS.Items;
using HouseCS.Items.Clothes;
using HouseCS.Items.Containers;

namespace HouseCS {
	internal class Program {
		private static readonly int verMajor = 2;
		private static readonly int verMinor = 0;
		private static readonly int verFix = 0;
		private static string CurVer => $"{verMajor}.{verMinor}.{verFix}";
		public static readonly string RED = "31";
		public static readonly string GREEN = "32";
		public static readonly string YELLOW = "33";
		public static readonly string BLUE = "34";
		public static readonly string PURPLE = "35";
		public static readonly string CYAN = "36";
		public static readonly string WHITE = "37";
		public static void WriteColor(string[] lines, ConsoleColor[] colors) {
			if (lines.Length != colors.Length)
				return;
			for (int i = 0; i < colors.Length; i++) {
				Console.ForegroundColor = colors[i];
				Console.Write(lines[i]);
			}
			Console.ResetColor();
		}
		public static void WriteColor(ColorText text) => WriteColor(text.Lines, text.Colors);
		public static void WriteColor(ColorText[] lines) {
			foreach (ColorText line in lines)
				WriteColor(line.Lines, line.Colors);
		}
		public static void WriteColorLine(string[] lines, ConsoleColor[] colors) {
			WriteColor(lines, colors);
			Console.WriteLine();
		}
		private static void Help(string cmd) {
			switch (cmd) {
				case "add":
					WriteColor(new string[] { "\nSyntax", " is: ", "add ", "item ", "[arg]\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.Green });
					WriteColor(new string[] { "\titem", " - must be a valid type\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White });
					WriteColor(new string[] { "\t arg", " - causes you to be prompted for the required info to create a new\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White });
					WriteColor(new string[] { "\t                Item", " of this type (without ", "arg", ", a default ", "Item", " is created)\n\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
					WriteColor(new string[] { "Adds", " Item ", "to the current floor\n\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.Yellow, ConsoleColor.White });
					break;
				case "attach":
					WriteColor(new string[] { "\nSyntax", " is: ", "attach ", "src dst ", "[-d]\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.Green });
					WriteColor(new string[] { "\tsrc", " - ", "integer", " of an ", "Item", " on the current floor (when used with ", "-d", ", ", "src\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Red });
					WriteColor(new string[] { "\t      must be the ", "integer", " of the ", "Item", " that is attached)\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
					WriteColor(new string[] { "\tdst", " - ", "integer", " of an ", "Item", " on the current floor\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
					WriteColor(new string[] { "\t -d", " - ", "detaches", " source", " from ", "destination\n\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.DarkRed, ConsoleColor.White, ConsoleColor.DarkRed });
					WriteColor(new string[] { "[De/A]ttaches ", "src", " [from/to] ", "dst", ".\n\n" }, new ConsoleColor[] { ConsoleColor.DarkBlue, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White });
					break;
				case "clear":
					WriteColor(new string[] { "\nSyntax", " is: ", "clear\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue });
					WriteColor(new string[] { "Clears", " the console, and places cursor at home position\n\n" }, new ConsoleColor[] { ConsoleColor.DarkBlue, ConsoleColor.White });
					break;
				case "down":
					WriteColor(new string[] { "\nSyntax", " is: ", "down\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue });
					WriteColor(new string[] { "Moves to the next floor ", "down", ", unless you are at the bottom\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White });
					break;
				case "exit":
					WriteColor(new string[] { "\nSyntax", " is: ", "exit\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue });
					WriteColor(new string[] { "Stops the program, and returns to your command line/operating environment\n\n" }, new ConsoleColor[] { ConsoleColor.White });
					break;
				case "grab":
					WriteColor(new string[] { "\nSyntax", " is: ", "grab ", "item\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.Red });
					WriteColor(new string[] { "\titem", " - ", "integer", " of ", "Item", " (see ", "list", ")\n\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White });
					WriteColor(new string[] { "Changes the \"Viewer\"'s current ", "Item\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow });
					break;
				case "help":
					WriteColor(new string[] { "\nSyntax", " is: ", "help ", "[command]\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.Red });
					WriteColor(new string[] { "\tcommand", " - a valid ", "command\n\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Blue });
					WriteColor(new string[] { "Colors:\n" }, new ConsoleColor[] { ConsoleColor.White });
					WriteColor(new string[] { "\t   R", "e", "d", " - warning -or- argument name (usually an integer)\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.DarkRed, ConsoleColor.Red, ConsoleColor.White });
					WriteColor(new string[] { "\t         dark: usually expanded name of a commands argument (to show\n" }, new ConsoleColor[] { ConsoleColor.White });
					WriteColor(new string[] { "\t         meaning)\n" }, new ConsoleColor[] { ConsoleColor.White });
					WriteColor(new string[] { "\tY", "e", "l", "l", "o", "w", " - Item\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.DarkYellow, ConsoleColor.Yellow, ConsoleColor.DarkYellow, ConsoleColor.Yellow, ConsoleColor.White });
					WriteColor(new string[] { "\t         dark: when talking about an Item but not using the exact term\n" }, new ConsoleColor[] { ConsoleColor.White });
					WriteColor(new string[] { "\t         \"Item\" (or the exact name of an Item)\n" }, new ConsoleColor[] { ConsoleColor.White });
					WriteColor(new string[] { "\t G", "r", "e", "e", "n", " - string argument (type it as it appears [without any brackets])\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.DarkGreen, ConsoleColor.Green, ConsoleColor.DarkGreen, ConsoleColor.Green, ConsoleColor.White });
					WriteColor(new string[] { "\t         dark: (no use yet)\n" }, new ConsoleColor[] { ConsoleColor.White });
					WriteColor(new string[] { "\t  C", "y", "a", "n", " - an integer for use when a command requires an item number\n" }, new ConsoleColor[] { ConsoleColor.Cyan, ConsoleColor.DarkCyan, ConsoleColor.Cyan, ConsoleColor.DarkCyan, ConsoleColor.White });
					WriteColor(new string[] { "\t         dark: Item integer for sub-items (ie: a book in a bookshelf)\n" }, new ConsoleColor[] { ConsoleColor.White });
					WriteColor(new string[] { "\t  B", "l", "u", "e", " - command\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.DarkBlue, ConsoleColor.Blue, ConsoleColor.DarkBlue, ConsoleColor.White });
					WriteColor(new string[] { "\t         dark: when referencing the command without using its exact name\n\n" }, new ConsoleColor[] { ConsoleColor.White });
					break;
				case "info":
					WriteColor(new string[] { "\nSyntax", " is: ", "info\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue });
					WriteColor(new string[] { "Returns ", "info", " about the current 'Viewer'\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White });
					break;
				case "list":
					WriteColor(new string[] { "\nSyntax", " is: ", "list ", "[", "item", "] [(", "-h", " / ", "-f", ")] [", "-r ", "start end", "] [", "-p", "] [", "-i ", "Item", "]\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.Yellow, ConsoleColor.White });
					WriteColor(new string[] { "\t   item", " - ", "integer", " of ", "Item", " (see ", "list", ")\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White });
					WriteColor(new string[] { "\t-h / -f", " - will show the \"Viewer\"'s current ", "Item\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Yellow });
					WriteColor(new string[] { "\t          Long version is ", "--hand", " or ", "--focus\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Green });
					WriteColor(new string[] { "\t     -r", " - will ", "list", " Items", " between [", "start", "] and [", "end", "]\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White });
					WriteColor(new string[] { "\t          Long version is ", "--range\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green });
					WriteColor(new string[] { "\t     -p", " - ", "lists", " all ", "Items", " on the floor one page at a time (page is\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
					WriteColor(new string[] { "\t          defined as 20 lines)\n" }, new ConsoleColor[] { ConsoleColor.White });
					WriteColor(new string[] { "\t          Long version is ", "--page\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green });
					WriteColor(new string[] { "\t-i ", "Item", " - ", "lists", " all ", "Items", " of type ", "Item", " (", "Item", " string)\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
					WriteColor(new string[] { "\t          Long version is ", "--item\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green });
					WriteColor(new string[] { "Used for getting info about an ", "Item", ", or multiple ", "Items", ".\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White });
					break;
				case "move":
					WriteColor(new string[] { "\nSyntax", " is: ", "move ", "item floor\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.Red });
					WriteColor(new string[] { "\t item", " - ", "integer", " of ", "Item", " (see ", "list", ")\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White });
					WriteColor(new string[] { "\tfloor", " - ", "integer", " of floor (or: ", "<", " for next floor down or ", ">", " for next floor\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White });
					WriteColor(new string[] { "\t        up)\n\n" }, new ConsoleColor[] { ConsoleColor.White });
					WriteColor(new string[] { "Moves", " an ", "Item", " from your current floor to the specified floor.\n\n" }, new ConsoleColor[] { ConsoleColor.DarkBlue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
					break;
				case "remove":
					WriteColor(new string[] { "\nSyntax", " is: ", "remove ", "item\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.Red });
					WriteColor(new string[] { "\titem", " - ", "integer", " of ", "Item", " (see ", "list", ")\n\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White });
					WriteColor(new string[] { "Removes", " specified ", "Item", " from current floor.\n\n" }, new ConsoleColor[] { ConsoleColor.DarkBlue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
					break;
				case "up":
					WriteColor(new string[] { "\nSyntax", " is: ", "up\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue });
					WriteColor(new string[] { "Moves to the next floor ", "up", ", unless you are at the top\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White });
					break;
				case "ver":
					WriteColor(new string[] { "\nSyntax", " is: ", "ver\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue });
					WriteColor(new string[] { "Tells you the current ", "version", " of the Check Command Interpretter\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White });
					break;
				default:
					WriteColor(new string[] { "Code error!!! (Please report, as this message shouldn't be possible to see.)" }, new ConsoleColor[] { ConsoleColor.Red });
					break;
			}
		}
		private static bool EqualsIgnoreCaseOr(string test, string[] strs) {
			for (int i = 0; i < strs.Length; i++)
				if (string.Equals(test, strs[i], StringComparison.OrdinalIgnoreCase))
					return true;
			return false;
		}
		private static bool MatchesAnd(string[] strs, string match) {
			for (int i = 0; i < strs.Length; i++)
				if (!Regex.IsMatch(strs[i], match))
					return false;
			return true;
		}
		private static bool CanGoInside(string src, string dst) {
			switch (dst.ToLower()) {
				case "bookshelf":
					return src.Equals("book", StringComparison.OrdinalIgnoreCase);
				case "container":
					switch (src.ToLower()) {
						case "empty":
						case "fridge":
							return false;
						default:
							return true;
					}
				case "display":
					switch (src.ToLower()) {
						case "computer":
						case "console":
							return true;
						default:
							return false;
					}
				case "empty":
					return false;
				case "fridge":
					switch (src.ToLower()) {
						case "empty":
						case "fridge":
							return false;
						default:
							return true;
					}
				case "dresser":
					switch (src.ToLower()) {
						case "clothing":
							return true;
						default:
							return false;
					}
				case "table":
					switch (src.ToLower()) {
						case "fridge":
						case "empty":
						case "table":
							return false;
						default:
							return true;
					}
				default:
					return false;
			}
		}
		public static IItem CreateContainer(string type) {
			switch (type.ToLower()) {
				case "fridge":
					return new Fridge();
				case "bookshelf":
					return new Bookshelf();
				case "dresser":
					return new Dresser();
				case "table":
					return new Table();
				default:
					return new Container();
			}
		}
		public static IItem CreateClothing(string type) {
			switch (type.ToLower()) {
				case "shirt":
					return new Shirt();
				case "pants":
					return new Pants();
				default:
					return new Clothing();
			}
		}
		public static string[,] enviVar = {
			{"interactive", "false", "bool", "user"},
			{"temperature", "70.0", "double", "system"},
			{"house", "0", "int", "system"}
		};

		private static void Main(string[] args) {
			string command;
			string[] cmds;

			//This is to keep the contents of my actual house a little more private.
			//Just make your own .java file that returns Items.
			ItemImport.InitializeItems();
			List<House> houseData = ItemImport.houses;
			List<Viewer> viewers = new List<Viewer>();
			foreach (House h in houseData)
				viewers.Add(new Viewer(h));

			House my_house = houseData[0];
			Viewer user = viewers[0];
			bool here = true;

			while (here) {
				Console.Write("> ");
				command = Console.ReadLine();
				string[] temp_arr = Regex.Split(command, " +");
				cmds = new string[temp_arr.Length];
				cmds = temp_arr; //I don't really think it matters if it's a clone or not...
				if (cmds.Length > 0) {
					switch (cmds[0].ToLower()) {
						case "visit":
							if (cmds.Length == 2) {
								if (Regex.IsMatch(cmds[1], "[0-9]+")) {
									int dst = int.Parse(cmds[1]);
									if (dst < houseData.Count) {
										user = viewers[dst];
										Console.WriteLine($"\nWelcome to House {dst}.\n");
									} else
										Console.WriteLine("There aren't that many Houses! (Remember: the first House is #0)");
								} else
									WriteColor(new string[] { "House number must be a positive ", "Integer", $", that is less than {houseData.Count.ToString()}." }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
							} else
								Console.WriteLine($"Visit which house? (There are {houseData.Count})");
							break;
						case "use":
							if (cmds.Length > 1) {
								switch (cmds[1]) {
									case "light":
									case "lights":
										if (cmds.Length == 2)
											WriteColor(new ColorText[] { new ColorText("\n"), user.CurHouse.GetFloor(user.CurFloor).ToggleLights(), new ColorText("\n\n") });
										else
											WriteColor(new string[] { "use", $" {cmds[1]}, does have extra arguments." }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
										break;
									default:
										WriteColor(new string[] { $"{cmds[1]} cannot be '", "used", "'." }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White });
										break;
								}
							} else
								WriteColor(new string[] { "Use", " what?\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
							break;
						case "set":
							bool validVar;
							switch (cmds.Length) {
								case 1:
									Console.Write("\n\tEnvironment Variables:\n\n");
									for (int v = 0; v < enviVar.GetLength(0); v++)
										Console.WriteLine($"{enviVar[v, 0]} = {enviVar[v, 1]}");
									Console.WriteLine();
									break;
								case 2:
									validVar = false;
									for (int v = 0; v < enviVar.GetLength(0); v++) {
										if (enviVar[v, 0].Equals(cmds[1], StringComparison.OrdinalIgnoreCase)) {
											Console.Write($"\n{enviVar[v, 0]} = {enviVar[v, 1]}\n\n");
											validVar = true;
										}
									}
									if (!validVar)
										Console.Write($"{cmds[1]} is not a valid variable.\n");
									break;
								case 3:
									validVar = false;
									int i;
									for (i = 0; i < enviVar.GetLength(0); i++) {
										if (enviVar[i, 0].Equals(cmds[1], StringComparison.OrdinalIgnoreCase)) {
											validVar = true;
											break;
										}
									}
									if (validVar) {
										if (enviVar[i, 3].Equals("user")) {
											switch (enviVar[i, 2]) {
												case "bool":
													if (EqualsIgnoreCaseOr(cmds[2], new string[] { "false", "true", "0", "1" })) {
														enviVar[i, 1] = EqualsIgnoreCaseOr(cmds[2], new string[] { "true", "1" }) ? "true" : "false";
													} else

														Console.Write($"\n{enviVar[i, 0]} stores a boolean value, must be true or false. (0 and 1 are acceptible)\n");
													break;
												case "int":
												case "double":
													if (Regex.IsMatch(cmds[2], "-?[0-9]+([.]{1}[0-9]+)?")) {
														double newVal = double.Parse(cmds[2]);
														enviVar[i, 1] = enviVar[i, 2].Equals("int") ? newVal.ToString() : newVal.ToString();
													} else
														Console.Write($"\n{enviVar[i, 0]} stores a numeric value, must only contain a number, - is optional, if variable stores a double, you may provide a decimal value\n");
													break;
												default:
													Console.Write($"\n{enviVar[i, 0]} did not have a recognized value type, be cautious, and report this bug.\n");
													goto case "string";
												case "string":
													enviVar[i, 1] = cmds[2];
													break;
											}
											//enviVar[i][1] = cmds[2];
											Console.Write($"\n{enviVar[i, 0]} = {enviVar[i, 1]}\n\n");
										} else
											Console.Write($"{enviVar[i, 0]} is a system variable, and cannot be changed by the user.\n");
									} else
										Console.Write($"{cmds[1]} is not a valid variable.\n");
									break;
								default:
									Console.Write("Invalid amount of arguments.\n");
									break;
							}
							break;
						case "attach":
							#region
							if (cmds.Length > 1) {
								if (Regex.IsMatch(cmds[1], "[0-9]+")) {
									if (cmds.Length > 2) {
										if (Regex.IsMatch(cmds[2], "[0-9]+")) {
											int src = Math.Abs(int.Parse(cmds[1]));
											int dst = Math.Abs(int.Parse(cmds[2]));
											if (cmds.Length > 3) {
												if (string.Equals(cmds[3], "-d", StringComparison.OrdinalIgnoreCase)) {
													IItem dst_i = user.GetItem(dst);
													switch (dst_i.Type) {
														case "Container":
															IItem tempItem = ((Container)dst_i).GetItem(src);
															if (!(tempItem is Empty)) {
																user.AddItem(tempItem);
																WriteColor(((Container)dst_i).RemoveItem(src));
															} else
																WriteColor(new string[] { "The ", dst_i.SubType, " doesn't have that many ", "Items", " in it.\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White });
															break;
														case "Display":
															Console.WriteLine(((Display)dst_i).Disconnect(src));
															break;
														default:
															WriteColor(new string[] { "That ", "Item", " cannot have things ", "detached", " from it.\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White });
															break;
													}
												} else
													WriteColor(new string[] { "Invalid argument, did you mean ", "-d", "?\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White });
											} else if (user.IsItem(src) && user.IsItem(dst)) {
												IItem src_i = user.GetItem(src);
												IItem dst_i = user.GetItem(dst);
												switch (dst_i.Type) {
													case "Container":
														if (CanGoInside(src_i is Clothing ? "Clothing" : src_i.SubType, dst_i.SubType)) {
															user.RemoveItem(src);
															WriteColor(new ColorText[] { ((Container)dst_i).AddItem(src_i), new ColorText("\n") });
														} else
															Console.WriteLine($"A {src_i.SubType}, cannot be put-in/attached-to a {dst_i.SubType}");
														break;
													case "Display":
														if (src_i is Computer || src_i is GameConsole)
															WriteColor(new ColorText[] { new ColorText("\n"), ((Display)dst_i).Connect(src_i), new ColorText("\n") });
														else
															WriteColor(new string[] { "Item ", src.ToString(), " cannot connect to a ", "Display", ".\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
														break;
													default:
														Console.Write("Item cannot have things attached to it.\n");
														break;
												}
											} else
												WriteColor(new string[] { "The floor only has ", user.FloorSize.ToString(), " items.\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
										} else
											WriteColor(new string[] { "Item", " must be an ", "integer", ".\n" }, new ConsoleColor[] { ConsoleColor.DarkRed, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
									} else
										WriteColor(new string[] { "\nAttach", " it to what?\n\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
								} else
									WriteColor(new string[] { "Item", " must be an ", "integer", ".\n" }, new ConsoleColor[] { ConsoleColor.DarkRed, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
							} else
								WriteColor(new string[] { "\nAttach", " what to what?\n\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
							break;
						#endregion
						case "move":
							#region
							if (cmds.Length > 1) {
								if (Regex.IsMatch(cmds[1], "[0-9]+")) {
									if (cmds.Length > 2) {
										if (Regex.IsMatch(cmds[2], "([0-9]+)|<|>")) {
											IItem old_item = user.curItem;
											int item = int.Parse(cmds[1]);
											int destination = Regex.IsMatch(cmds[2], "[0-9]+") ? int.Parse(cmds[2]) : (cmds[2].Equals("<") ? user.CurFloor - 1 : user.CurFloor + 1);
											int oldFloor = user.CurFloor;
											if (user.ChangeItemFocus(item)) {
												if (user.GoFloor(destination)) {
													IItem tempItem = user.curItem;
													user.AddItem(tempItem);
													user.GoFloor(oldFloor);
													user.RemoveItem(item);
													user.curItem = tempItem;
													WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", "Item ", "moved ", $"to floor {destination}\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.DarkBlue, ConsoleColor.White }), user.curItem.ToText(), new ColorText("\n\n") });
												} else
													Console.Write("Floor does not exist.\n");
											} else
												WriteColor(new string[] { "Item", " does not exist.\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White });
											user.curItem = old_item;
										} else
											WriteColor(new string[] { "Floor must be an ", "integer", ", or: ", "<", " or ", ">", ".\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White });
									} else
										WriteColor(new string[] { "\nMove", " it where?\n\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
								} else
									WriteColor(new string[] { "Item", " must be an ", "integer", ".\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
							} else
								WriteColor(new string[] { "\nMove", " what, and where?\n\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
							break;
						#endregion
						case "grab":
						case "select":
							#region
							if (cmds.Length > 1) {
								if (Regex.IsMatch(cmds[1], "[0-9]+")) {
									if (cmds.Length > 2) {
										if (Regex.IsMatch(cmds[2], "[0-9]+")) {
											switch (user.ChangeItemFocus(int.Parse(cmds[1]), int.Parse(cmds[2]))) {
												case 0:
													WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", "Item", " selected: (of type ", user.curItem.Type, ")\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }), user.curItem.ToText(), new ColorText("\n\n") });
													break;
												case 1:
													WriteColor(new string[] { "Either ", "Item ", cmds[1], " doesn't have any ", "sub-Items", ", or the ", "integer", " you entered is too high.\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
													break;
												case 2:
													WriteColor(new string[] { $"\"{cmds[1]}\" is invalid, must be less than the floor ", "Item", " size of: ", $"{user.FloorSize.ToString()}\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan });
													break;
												default:
													WriteColor(new string[] { "ERROR: get sub-item did not return 0, 1, or 2. Please report this!\n" }, new ConsoleColor[] { ConsoleColor.Red });
													break;
											}
										} else
											WriteColor(new string[] { $"\"{cmds[2]}\" is not a valid ", "integer\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan });
									} else if (user.ChangeItemFocus(int.Parse(cmds[1])))
										WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", "Item", " selected: (of type ", user.curItem.Type, ")\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }), user.curItem.ToText(), new ColorText("\n\n") });
									else
										WriteColor(new string[] { $"\"{cmds[1]}\" is invalid, must be less than the floor ", "Item", " size of: ", $"{user.FloorSize.ToString()}\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan });
								} else
									WriteColor(new string[] { $"\"{cmds[1]}\" is not a valid ", "integer\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan });
							} else
								WriteColor(new string[] { "\nGrab", " what?\n\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
							break;
						#endregion
						case "remove":
							#region
							if (cmds.Length > 1) {
								if (Regex.IsMatch(cmds[1], "[0-9]+")) {
									IItem tempItem = user.curItem;
									bool validAnswer = false;
									if (cmds.Length > 2) {
										if (Regex.IsMatch(cmds[2], "[0-9]+")) {
											switch (user.ChangeItemFocus(int.Parse(cmds[1]), int.Parse(cmds[2]))) {
												case 0:
													while (!validAnswer) {
														WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", "Item", " is:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }), user.curItem.ToText(), new ColorText("\n\n") });
														WriteColor(new string[] { "Are you sure you want to delete this? [Y/N] > " }, new ConsoleColor[] { ConsoleColor.Red });
														user.curItem = tempItem;
														string yenu = Console.ReadLine().ToUpper();
														Console.WriteLine();
														switch (yenu) {
															case "Y":
																user.RemoveItem(int.Parse(cmds[1]), int.Parse(cmds[2]));
																goto case "N";
															case "N":
																validAnswer = true;
																break;
														}
													}
													break;
												case 1:
													WriteColor(new string[] { "This ", "Item", " either has no ", "sub-Items", " on it, or the ", "integer", " is too high\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
													break;
												case 2:
													WriteColor(new string[] { "This floor only has ", user.FloorSize.ToString(), " items on it\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
													break;
											}
										} else
											WriteColor(new string[] { $"\"{cmds[2]}\" is not a valid ", "integer\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan });
									} else if (user.ChangeItemFocus(int.Parse(cmds[1]))) {
										while (!validAnswer) {
											WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", "Item", " is:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }), user.curItem.ToText(), new ColorText("\n\n") });
											WriteColor(new string[] { "Are you sure you want to delete this? [Y/N] > " }, new ConsoleColor[] { ConsoleColor.Red });
											user.curItem = tempItem;
											string yenu = Console.ReadLine().ToUpper();
											Console.WriteLine();
											switch (yenu) {
												case "Y":
													user.RemoveItem(int.Parse(cmds[1]));
													goto case "N";
												case "N":
													validAnswer = true;
													break;
											}
										}
									} else
										WriteColor(new string[] { "This floor only has ", user.FloorSize.ToString() }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
								} else
									WriteColor(new string[] { $"\"{cmds[1]}\" is not a valid ", "integer\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan });
							} else
								WriteColor(new string[] { "\nRemove", " what?\n\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
							break;
						#endregion
						case "list":
						case "look":
							#region
							if (cmds.Length > 1) {
								if (EqualsIgnoreCaseOr(cmds[1], new string[] { "--hand", "--focus", "-h", "-f" }))
									WriteColor(new ColorText[] { new ColorText("\n"), user.GetViewCurItem(), new ColorText("\n\n") });
								else if (cmds.Length > 2 || EqualsIgnoreCaseOr(cmds[1], new string[] { "-p", "--page" })) {
									bool page = false;
									int rangeStart = 0;
									int rangeEnd = user.FloorSize;
									string searchType = "*";
									int invalidArg = 0;
									for (int i = 1; i < cmds.Length; i++) {
										if (EqualsIgnoreCaseOr(cmds[i], new string[] { "-i", "--item" }) && (i + 1 < cmds.Length)) {
											searchType = cmds[i + 1];
											i++;
											continue;
										}
										if (EqualsIgnoreCaseOr(cmds[i], new string[] { "-r", "--range" }) && (i + 2 < cmds.Length)) {
											if (Regex.IsMatch(cmds[i + 1], "[0-9]+") && Regex.IsMatch(cmds[i + 2], "[0-9]+")) {
												rangeStart = int.Parse(cmds[i + 1]);
												rangeEnd = int.Parse(cmds[i + 2]);
												i += 2;
												continue;
											}
										}
										if (EqualsIgnoreCaseOr(cmds[i], new string[] { "-p", "--page" })) {
											page = true;
											continue;
										}
										invalidArg = i;
									}
									if (invalidArg == 0) {
										if (enviVar[0, 1].Equals("false") || user.CurHouse.GetFloor(user.CurFloor).Lights) {
											if (page) {
												int pageCount = user.PageCount(rangeStart, rangeEnd, searchType, 20);
												switch (pageCount) {
													case 0:
														WriteColor(new string[] { "No ", "Items", " match your criteria.\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
														break;
													case -1:
														Console.Write($"{searchType} is not a valid type.\n");
														break;
													case -2:
														Console.Write("Floor is empty.\n");
														break;
													case -3:
														Console.Write("Range start must be greater than or equal to range end.\n");
														break;
													case -4:
														Console.Write("Range start must be greater than or equal to 0.\n");
														break;
													default:
														for (int i = 0; i < pageCount; i++) {
															WriteColorLine(new string[] { "\n\tFloor ", "Listing", " - Page ", (i + 1).ToString() }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.DarkBlue, ConsoleColor.Cyan });
															bool endTest = i + 1 < pageCount;
															WriteColor(user.List(rangeStart, rangeEnd, searchType, 20, i));
															if (endTest) {
																Console.Write("Press enter to continue > ");
																Console.ReadLine();
															} else
																Console.WriteLine();
														}
														break;
												}
											} else
												WriteColor(new ColorText[] { user.List(rangeStart, rangeEnd, searchType, user.FloorSize, 0), new ColorText("\n") });
										} else
											Console.Write("\nYou can't see anything, the floor is completely dark!\n\n");
									} else
										Console.Write($"{cmds[invalidArg]} is not a valid argument.\n");
								} else if (Regex.IsMatch(cmds[1], "[0-9]+")) {
									if (int.Parse(cmds[1]) < user.FloorSize) {
										IItem tempItem = user.curItem;
										user.ChangeItemFocus(int.Parse(cmds[1]));
										switch (user.curItem.Type) {
											case "Container":
												WriteColor(new string[] { "This ", "Item", " is a ", user.curItem.SubType, ", would you like to see:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												WriteColor(new string[] { "(Y) A specific ", "Item", "\n(N) Just the overall contents\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												while (true) {
													Console.Write("[Y/N] > ");
													string temp = Console.ReadLine().ToUpper();
													int iC = ((Container)user.curItem).Size;
													if (temp.Equals("Y") && iC > 0) {
														WriteColor(new string[] { "\nWhich ", "Item", ":\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
														while (true) {
															WriteColor(new string[] { "[", "0", "-", (iC - 1).ToString(), "] > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
															int im = int.Parse(Console.ReadLine());
															if (im < iC) {
																WriteColor(new ColorText[] { new ColorText("\n"), ((Container)user.curItem).GetItem(im).ToText() });
																break;
															} else
																WriteColor(new string[] { "There aren't that many ", "Items", " in this ", user.curItem.SubType }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow });
														}
													} else if (temp.Equals("N") || ((Container)user.curItem).Size == 0)
														WriteColor(new ColorText[] { new ColorText("\n"), user.GetViewCurItem() });
													else
														WriteColor(new string[] { user.curItem.SubType, " is empty." }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White });
													Console.WriteLine();
													if (EqualsIgnoreCaseOr(temp, new string[] { "Y", "N" }))
														break;
												}
												Console.WriteLine();
												break;
											case "Display":
												WriteColor(new string[] { "This ", "Item", " is a ", "Display", ", would you like to see:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												WriteColor(new string[] { "(Y) A specific device\n(N) Just the ", "Display\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow });
												while (true) {
													Console.Write("[Y/N] > ");
													string temp = Console.ReadLine().ToUpper();
													if (temp.Equals("Y") && ((Display)user.curItem).DeviceCount > 0) {
														Console.Write("\nWhich device:\n\n");
														while (true) {
															WriteColor(new string[] { "[", "0", "-", (((Display)user.curItem).DeviceCount - 1).ToString(), "] > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
															int dv = int.Parse(Console.ReadLine());
															if (dv < ((Display)user.curItem).DeviceCount) {
																WriteColor(new ColorText[] { new ColorText("\n"), ((Display)user.curItem).GetDevice(dv).ToText() });
																break;
															} else
																WriteColor(new string[] { user.curItem.SubType, " doesn't have that many ", "Items" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.Yellow, ConsoleColor.Yellow });
														}
													} else if (temp.Equals("N") || ((Display)user.curItem).DeviceCount == 0)
														WriteColor(new ColorText[] { new ColorText("\n"), user.GetViewCurItem() });
													Console.WriteLine();
													if (EqualsIgnoreCaseOr(temp, new string[] { "Y", "N" }))
														break;
												}
												Console.WriteLine();
												break;
											case "Book":
											case "Computer":
											case "Console":
											case "Bed":
											case "Clothing":
												WriteColor(new ColorText[] { new ColorText("\n"), user.GetViewCurItem(), new ColorText("\n\n") });
												break;
										}
										user.curItem = tempItem;
									} else
										WriteColor(new string[] { "This floor only has ", user.FloorSize.ToString(), " Items", " on it\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.White });
								} else
									WriteColor(new string[] { $"\"{cmds[1]}\" is not a valid ", "integer\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan });
							} else if (enviVar[0, 1].Equals("false") || user.CurHouse.GetFloor(user.CurFloor).Lights)
								WriteColor(new ColorText[] { user.List(), new ColorText("\n") });
							else
								Console.Write("\nYou can't see anything, the floor is completely dark!\n\n");
							break;
						#endregion
						case "add":
							#region
							if (cmds.Length > 1) {
								switch (cmds[1].ToLower()) {
									case "book":
										Book tempBook = new Book();
										if (cmds.Length > 2) {
											if (string.Equals(cmds[2], "arg", StringComparison.OrdinalIgnoreCase)) {
												WriteColor(new string[] { "\nEnter ", "Book", " Title > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												string title = Console.ReadLine();
												WriteColor(new string[] { "\nEnter ", "Book", " Author > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												string author = Console.ReadLine();
												Console.Write("\nEnter Publishing Year > ");
												int year = int.Parse(Console.ReadLine());
												tempBook.Reset(title, author, year);
												WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", "Book", " added:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }), tempBook.ToText(), new ColorText("\n\n") });
											} else
												WriteColor(new string[] { "\nInvalid 2nd argument, did you mean ", "arg", "?\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White });
										} else
											WriteColor(new string[] { "\nNew ", "Book", " added to floor ", user.CurFloor.ToString(), ".\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
										user.AddItem(tempBook);
										break;
									case "computer":
										Computer tempComp = new Computer();
										if (cmds.Length > 2) {
											if (string.Equals(cmds[2], "arg", StringComparison.OrdinalIgnoreCase)) {
												WriteColor(new string[] { "\nWhat kind of ", "Computer", " is it? (Desktop, Laptop, etc) > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												string computer = Console.ReadLine();
												WriteColor(new string[] { "\nComputer", " Brand (ie: HP, Microsoft) > " }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White });
												string brand = Console.ReadLine();
												WriteColor(new string[] { "Computer", " Family (ie: Pavilion, Surface) > " }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White });
												string family = Console.ReadLine();
												WriteColor(new string[] { "Computer", " Model (ie: dv6, Pro 3) > " }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White });
												string model = Console.ReadLine();
												Console.Write("\nIs it on? (Invalid input will default to no)\nYes or no? [Y/N] > ");
												string is_on = Console.ReadLine().ToUpper();
												tempComp.Reset(brand, family, model, is_on.Equals("Y"), computer);
												WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", "Computer", " added:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }), tempComp.ToText(), new ColorText("\n\n") });
											} else
												WriteColor(new string[] { "\nInvalid 2nd argument, did you mean ", "arg", "?\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White });
										} else
											WriteColor(new string[] { "\nNew ", "Computer", " added to floor ", user.CurFloor.ToString(), ".\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
										user.AddItem(tempComp);
										break;
									case "console":
										GameConsole tempConsole = new GameConsole();
										if (cmds.Length > 2) {
											if (string.Equals(cmds[2], "arg", StringComparison.OrdinalIgnoreCase)) {
												for (int i = 0; i < GameConsole.types.Length; i++)
													WriteColor(new string[] { i.ToString(), $": {GameConsole.types[i]} " }, new ConsoleColor[] { ConsoleColor.Cyan, ConsoleColor.White });
												Console.WriteLine();
												WriteColor(new string[] { "\nEnter ", "Console", " Type > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												int tempType = int.Parse(Console.ReadLine());
												WriteColor(new string[] { "\nEnter ", "Console", " Manufacturer (ie Nintendo) > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												string com = Console.ReadLine();
												WriteColor(new string[] { "\nEnter ", "Console", " Name (ie GameCube) > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												string sys = Console.ReadLine();
												tempConsole = new GameConsole(tempType, com, sys);
												WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", "Console", " added:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }), tempConsole.ToText(), new ColorText("\n\n") });
											} else
												WriteColor(new string[] { "\nInvalid 2nd argument, did you mean ", "arg", "?\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White });
										} else
											WriteColor(new string[] { "\nNew ", "Console", " added to floor ", user.CurFloor.ToString(), ".\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
										user.AddItem(tempConsole);
										break;
									case "display":
										Display tempDisp = new Display();
										if (cmds.Length > 2) {
											if (string.Equals(cmds[2], "arg", StringComparison.OrdinalIgnoreCase)) {
												Console.Write("\nIs it a Monitor (Y) or a TV (N)?\nWill default to (Y)es if next input is invalid.\n[Y/N] > ");
												string is_mon = Console.ReadLine().ToUpper();
												WriteColor(new string[] { "\nType the number for each device connected to this ", "Display", " seperated by a space.\n(Optional)\n> " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												string[] conDevs = Regex.Split(Console.ReadLine(), " +");
												List<IItem> validDevs = new List<IItem>();
												List<int> added = new List<int>();
												List<int> notAdded = new List<int>();
												List<string> notNumber = new List<string>();
												foreach (string dev in conDevs) {
													if (Regex.IsMatch(dev, "[0-9]+")) {
														int devID = int.Parse(dev);
														if (devID >= 0 && devID < user.FloorSize && CanGoInside(user.GetItem(devID).Type, "Display"))
															added.Add(devID);
														else
															notAdded.Add(devID);
													} else
														notNumber.Add(dev);
												}
												Console.Write("\nAdded: ");
												foreach (int num in added)
													Console.Write(num + " ");
												Console.Write("\n\nNot added: ");
												foreach (int num in notAdded)
													Console.Write(num + " ");
												Console.Write("\n\nNot a number: ");
												foreach (string str_num in notNumber)
													Console.Write(str_num + " ");
												WriteColor(new string[] { "\n\nEnter the ", "Display's", " size in inches (decimals allowed) > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White });
												double size = double.Parse(Console.ReadLine());
												List<IItem> new_items = new List<IItem>();
												foreach (int id in added)
													new_items.Add(user.GetItem(id));
												tempDisp = new Display(is_mon.Equals("N"), new_items, size);
												WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", "Display", " added:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }), tempDisp.ToText(), new ColorText("\n\n") });
											} else
												WriteColor(new string[] { "\nInvalid 2nd argument, did you mean ", "arg", "?\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White });
										} else
											WriteColor(new string[] { "\nNew ", "Display", " added to floor ", user.CurFloor.ToString(), ".\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
										user.AddItem(tempDisp);
										break;
									case "bed":
										Bed tempBed = new Bed();
										if (cmds.Length > 2) {
											if (cmds[2].Equals("arg", StringComparison.OrdinalIgnoreCase)) {
												WriteColor(new string[] { "\nIs this ", "Bed", " adjustable? (Invalid input will default to N)\n[Y/N] > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												bool canMove = Console.ReadLine().ToUpper().Equals("Y");
												Console.WriteLine();
												for (int i = 0; i < Bed.types.Length; i++)
													WriteColor(new string[] { "[", i.ToString(), $"] {Bed.types[i]} " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
												WriteColor(new string[] { "\nInvalid input defaults to ", "2" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan });
												WriteColor(new string[] { "\n[", "0", "-", (Bed.types.Length - 1).ToString(), "] > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
												string typeInput = Console.ReadLine();
												int bedType = Regex.IsMatch(typeInput, "[0-9]+") && int.Parse(typeInput) < Bed.types.Length ? int.Parse(typeInput) : 2;
												tempBed = new Bed(canMove, bedType);
												WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", "Bed", " added:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }), tempBed.ToText(), new ColorText("\n\n") });
											} else
												WriteColor(new string[] { "\nInvalid 2nd argument, did you mean ", "arg", "?\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White });
										} else
											WriteColor(new string[] { "\nNew ", "Bed", " added to floor ", user.CurFloor.ToString(), ".\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
										user.AddItem(tempBed);
										break;
									case "container":
										WriteColor(new string[] { "\nEnter the ", "Container", " sub-type:\n\tie: Container, Bookshelf, Fridge, etc. (Defaults to Container)\n\n> " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
										string type = Console.ReadLine();
										IItem tempCon = CreateContainer(type);
										if (cmds.Length > 2) {
											if (cmds[2].Equals("arg", StringComparison.OrdinalIgnoreCase)) {
												WriteColor(new string[] { "\nType the number for each ", "Item", " to be put inside this ", tempCon.SubType, " seperated by a space.\n(Optional)\n> " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												string[] objs = Regex.Split(Console.ReadLine(), " +");
												Console.WriteLine();
												List<IItem> validObjs = new List<IItem>();
												List<int> added = new List<int>();
												List<int> notAdded = new List<int>();
												List<string> notNumber = new List<string>();
												foreach (string itm in objs) {
													if (Regex.IsMatch(itm, "[0-9]+")) {
														int itmID = int.Parse(itm);
														if (itmID >= 0 && itmID < user.FloorSize)
															added.Add(itmID);
														else
															notAdded.Add(itmID);
													} else
														notNumber.Add(itm);
												}
												List<IItem> toAdd = new List<IItem>();
												foreach (int num in added)
													toAdd.Add(user.GetItem(num));
												foreach (IItem i in toAdd) {
													if (CanGoInside(i is Clothing ? "Clothing" : i.Type, tempCon.Type)) {
														user.RemoveItem(i);
														((Container)tempCon).AddItem(i);
													}
												}
												WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", tempCon.SubType, " created:\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.Yellow, ConsoleColor.White }), tempCon.ToText(), new ColorText("\n\n") });
											} else
												WriteColor(new string[] { "\nInvalid 2nd argument, did you mean ", "arg", "?\n\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.White });
										} else
											WriteColor(new string[] { "\nNew ", tempCon.SubType, " added to floor ", user.CurFloor.ToString(), ".\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
										user.AddItem(tempCon);
										break;
									case "clothing":
										WriteColor(new string[] { "\nEnter the ", "Clothing", " sub-type:\n\tie: Clothing, Shirt, Pants, etc. (Defaults to Clothing)\n\n> " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
										string clothType = Console.ReadLine();
										IItem tempCloth = CreateClothing(clothType);
										if (cmds.Length > 2) {
											if (cmds[2].Equals("arg", StringComparison.OrdinalIgnoreCase)) {
												Console.Write("\nEnter Clothing color > ");
												((Clothing)tempCloth).Color = Console.ReadLine();
												WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", tempCloth.SubType, " created:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.Yellow }), tempCloth.ToText(), new ColorText("\n\n") });
											} else
												WriteColor(new string[] { "\nInvalid 2nd argument, did you mean ", "arg", "?\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White });
										} else
											WriteColor(new string[] { "\nNew ", tempCloth.SubType, " added to floor ", user.CurFloor.ToString(), ".\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
										user.AddItem(tempCloth);
										break;
									default:
										WriteColor(new string[] { $"\"{cmds[1]}\" is not a valid ", "Item", " type:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
										for (int i = 0; i < cmds.Length; i++)
											Console.Write(cmds[i] + " ");
										Console.WriteLine();
										Help("add");
										break;
								}
							} else
								Console.Write("\nInvalid syntax, requires at least one argument\n\n");
							break;
						#endregion
						case "status":
						case "info":
							#region
							Console.WriteLine($"\n{user}\n");
							break;
						#endregion
						case ">":
						case "up":
							#region
							Console.WriteLine(user.GoUp());
							break;
						#endregion
						case "<":
						case "down":
							#region
							Console.WriteLine(user.GoDown());
							break;
						#endregion
						case "":
							break;
						case "help":
							#region
							if (cmds.Length > 1) {
								switch (cmds[1].ToLower()) {
									case "add":
										Help("add");
										break;
									case "attach":
										Help("attach");
										break;
									case "clear":
									case "cls":
										Help("clear");
										break;
									case "down":
										Help("down");
										break;
									case "exit":
									case "quit":
										Help("exit");
										break;
									case "grab":
									case "select":
										Help("grab");
										break;
									case "help":
										Help("help");
										break;
									case "info":
									case "status":
										Help("info");
										break;
									case "list":
									case "look":
										Help("list");
										break;
									case "move":
										Help("move");
										break;
									case "remove":
										Help("remove");
										break;
									case "up":
										Help("up");
										break;
									case "ver":
									case "version":
										Help("ver");
										break;
									default:
										Console.Write("\nNo help was found for this command.\n\n");
										break;
								}
							} else {
								Console.WriteLine();
								WriteColor(new string[] { "          add", " - adds an ", "Item", " to the current floor\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
								WriteColor(new string[] { "       attach", " - attaches (or detaches) one ", "Item", " to (from) another\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
								WriteColor(new string[] { "  clear / cls", " - clears the screen\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
								WriteColor(new string[] { "         down", " - goes down 1 floor\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
								WriteColor(new string[] { "  exit / quit", " - stops the program\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
								WriteColor(new string[] { "grab / select", " - sets which ", "Item", " you are currently focused on\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
								WriteColor(new string[] { "         help", " - displays this screen, and others\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
								WriteColor(new string[] { "info / status", " - shows information about you and the ", "House", " you are currently in\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
								WriteColor(new string[] { "  list / look", " - shows the ", "Items", " on the current floor, or shows info about a\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
								WriteColor(new string[] { "                specific ", "Item\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow });
								WriteColor(new string[] { "         move", " - moves an ", "Item", " to another floor\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
								WriteColor(new string[] { "       remove", " - removes an ", "Item", " from the current floor\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
								WriteColor(new string[] { "           up", " - goes up 1 floor\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
								WriteColor(new string[] { "ver / version", " - displays information about the command interpretter\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
								Console.Write("\ntype help (command) for more detailed information about a specific command\n\n");
							}
							break;
						#endregion
						case "clear":
						case "cls":
							#region
							Console.Clear();
							break;
						#endregion
						case "exit":
						case "quit":
							#region
							here = false;
							break;
						#endregion
						case "ver":
						case "version":
							#region
							WriteColor(new string[] { "\nCh", "ec", "k", $" Command Interpretter\n\tVersion {CurVer}\n\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.White });
							break;
						#endregion
						default:
							#region
							Console.Write($"\"{cmds[0]}\" is not a valid command:\n");
							for (int i = 0; i < cmds.Length; i++)
								Console.Write(cmds[i] + " ");
							Console.WriteLine();
							break;
							#endregion
					}
				}
			}
		}
	}
}
