using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HouseCS.ConsoleUtils;
using HouseCS.Items;

namespace HouseCS {
	internal class Program {
		private static readonly int verMajor = 1;
		private static readonly int verMinor = 1;
		private static readonly int verFix = 1;
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
			foreach (ColorText line in lines) WriteColor(line.Lines, line.Colors);
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
					WriteColor(new string[] { "\t                Item", " of this type (without ", "arg", " a default ", "Item", " is created)\n\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
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
					WriteColor(new string[] { "Moves to the next floor ", "down", " unless you are at the bottom\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White });
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
					WriteColor(new string[] { "\t     -p", " - ", "lists", " all ", "Items", " on the floor one page at a time (page is\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White });
					WriteColor(new string[] { "\t          defined as 20 lines)\n" }, new ConsoleColor[] { ConsoleColor.White });
					WriteColor(new string[] { "\t          Long version is ", "--page\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green });
					WriteColor(new string[] { "\t-i ", "Item", " - ", "lists", " all ", "Items", " of type ", "Item", " (", "Item", " string)\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
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
					WriteColor(new string[] { "\titem", " - ", "integer", " of ", "Item", " (see ", "list", ")\n\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Blue });
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

		private static void Main(string[] args) {
			string command;
			string[] cmds;

			House my_house = new House(2, 2);
			Viewer user = new Viewer(my_house);
			bool here = true;

			//This is to keep the contents of my actual house a little more private.
			//Just make your own .java file that returns Items.
			for (int i = 0; i < ItemImport.bookshelfs.Length; i++)
				my_house.AddItem(ItemImport.bookshelfs_f[i], ItemImport.bookshelfs[i]);
			for (int i = 0; i < ItemImport.computers.Length; i++)
				my_house.AddItem(ItemImport.computers_f[i], ItemImport.computers[i]);
			for (int i = 0; i < ItemImport.consoles.Length; i++)
				my_house.AddItem(ItemImport.consoles_f[i], ItemImport.consoles[i]);
			for (int i = 0; i < ItemImport.displays.Length; i++)
				my_house.AddItem(ItemImport.displays_f[i], ItemImport.displays[i]);
			for (int i = 0; i < ItemImport.beds.Length; i++)
				my_house.AddItem(ItemImport.beds_f[i], ItemImport.beds[i]);

			while (here) {
				Console.Write("> ");
				command = Console.ReadLine();
				string[] temp_arr = Regex.Split(command, " +");
				cmds = new string[temp_arr.Length];
				cmds = temp_arr; //I don't really think it matters if it's a clone or not...
				if (cmds.Length > 0) {
					switch (cmds[0].ToLower()) {
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
														case "Bookshelf":
															if (src < ((Bookshelf)dst_i).BookCount)
																user.AddItem(((Bookshelf)dst_i).GetBook(src));
															WriteColor(((Bookshelf)dst_i).RemoveBook(src));
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
													case "Bookshelf":
														if (src_i is Book) {
															user.RemoveItem(src);
															((Bookshelf)dst_i).AddBook((Book)src_i);
														} else
															Console.Write("Item " + src + " is not a book.\n");
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
											int old_floor = user.CurFloor;
											if (user.ChangeItemFocus(item)) {
												if (user.GoFloor(destination)) {
													user.AddItem(user.curItem);
													user.GoFloor(old_floor);
													user.RemoveItem(item);
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
								else if (EqualsIgnoreCaseOr(cmds[1], new string[] { "-i", "--item" })) {
									if (cmds.Length > 2)
										WriteColor(new ColorText[] { user.List(cmds[2]), new ColorText("\n") });
									else
										WriteColor(new string[] { "No ", "Item", " type specified.\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White });
								} else if (EqualsIgnoreCaseOr(cmds[1], new string[] { "-p", "--page" })) {
									for (int i = 0; i < user.FloorSize / 20 + (user.FloorSize % 20 == 0 ? 0 : 1); i++) {
										WriteColor(new string[] { "\n\tFloor ", "Listing", $" - Page {i + 1}" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White });
										bool end_test = 20 * (i + 1) < user.FloorSize;
										WriteColor(new ColorText[] { user.List(20 * i, end_test ? 20 * (i + 1) : user.FloorSize), new ColorText("\n\n") });
										if (end_test) {
											Console.Write("Press enter to continue > ");
											Console.ReadLine();
										}
									}
								} else if (EqualsIgnoreCaseOr(cmds[1], new string[] { "-r", "--range" })) {
									if (cmds.Length > 3 && MatchesAnd(new string[] { cmds[2], cmds[3] }, "[0-9]+"))
										WriteColor(new ColorText[] { user.List(int.Parse(cmds[2]), int.Parse(cmds[3]) + 1), new ColorText("\n") });
									else
										WriteColor(new string[] { "range", " requires ", "2 integers\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Cyan });
								} else if (Regex.IsMatch(cmds[1], "[0-9]+")) {
									if (int.Parse(cmds[1]) < user.FloorSize) {
										IItem temp_item = user.curItem;
										user.ChangeItemFocus(int.Parse(cmds[1]));
										switch (user.curItem.Type) {
											case "Bookshelf":
												WriteColor(new string[] { "This ", "Item", " is a ", "Bookshelf", ", would you like to see:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												WriteColor(new string[] { "(Y) A specific ", "Book", "\n(N) Just the ", "Bookshelf\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow });
												while (true) {
													Console.Write("[Y/N] > ");
													string temp = Console.ReadLine().ToUpper();
													int bC = ((Bookshelf)user.curItem).BookCount;
													if (temp.Equals("Y") && bC > 0) {
														WriteColor(new string[] { "\nWhich ", "Book:\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow });
														while (true) {
															WriteColor(new string[] { "[", "0", "-", (bC - 1).ToString(), "] > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
															int bk = int.Parse(Console.ReadLine());
															if (bk < bC) {
																WriteColor(new ColorText[] { new ColorText("\n"), ((Bookshelf)user.curItem).GetBook(bk).ToText() });
																break;
															}
														}
													}
													if (temp.Equals("N") || ((Bookshelf)user.curItem).BookCount == 0)
														WriteColor(new ColorText[] { new ColorText("\n"), user.GetViewCurItem() });
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
															}
														}
													}
													if (temp.Equals("N") || ((Display)user.curItem).DeviceCount == 0)
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
												WriteColor(new ColorText[] { new ColorText("\n"), user.GetViewCurItem(), new ColorText("\n\n") });
												break;
										}
										user.curItem = temp_item;
									} else
										WriteColor(new string[] { $"This floor only has {user.FloorSize}", " Items", " on it\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White });
								} else
									WriteColor(new string[] { $"\"{cmds[1]}\" is not a valid ", "integer\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan });
							} else
								WriteColor(new ColorText[] { user.List(), new ColorText("\n") });
							break;
						#endregion
						case "add":
							#region
							if (cmds.Length > 1) {
								switch (cmds[1].ToLower()) {
									case "bookshelf":
										Bookshelf tempShelf = new Bookshelf();
										if (cmds.Length > 2) {
											if (string.Equals(cmds[2], "arg", StringComparison.OrdinalIgnoreCase)) {
												WriteColor(new string[] { "\nHow many ", "books", " will be on this ", "shelf", "? > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White });
												int length = int.Parse(Console.ReadLine());
												Console.WriteLine();
												for (int i = 0; i < length; i++) {
													WriteColor(new string[] { "Book ", $"{i.ToString()}\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.Cyan });
													WriteColor(new string[] { "\nEnter ", "Book", " Title > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
													string title = Console.ReadLine();
													WriteColor(new string[] { "\nEnter ", "Book", " Author > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
													string author = Console.ReadLine();
													Console.Write("\nEnter Publishing Year > ");
													int year = int.Parse(Console.ReadLine());
													Console.WriteLine();
													tempShelf.AddBook(new Book(title, author, year));
												}
												WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", "Bookshelf", " created:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }), tempShelf.ToText(), new ColorText("\n\n") });
											} else
												WriteColor(new string[] { "\nInvalid 2nd argument, did you mean ", "arg", "?\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White });
										} else
											WriteColor(new string[] { "\nNew ", "Bookshelf", " added to floor ", user.CurFloor.ToString(), ".\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
										user.AddItem(tempShelf);
										break;
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
												string type = Console.ReadLine();
												WriteColor(new string[] { "\nComputer", " Brand (ie: HP, Microsoft) > " }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White });
												string brand = Console.ReadLine();
												WriteColor(new string[] { "Computer", " Family (ie: Pavilion, Surface) > " }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White });
												string family = Console.ReadLine();
												WriteColor(new string[] { "Computer", " Model (ie: dv6, Pro 3) > " }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White });
												string model = Console.ReadLine();
												Console.Write("\nIs it on? (Invalid input will default to no)\nYes or no? [Y/N] > ");
												string is_on = Console.ReadLine().ToUpper();
												tempComp.Reset(brand, family, model, is_on.Equals("Y"), type);
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
														if (devID >= 0 && devID < user.FloorSize)
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
									default:
										WriteColor(new string[] { $"\"{cmds[1]}\" is not a valid ", "Item", " type:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
										for (int i = 0; i < cmds.Length; i++)
											Console.Write(cmds[i] + " ");
										Console.WriteLine(); Help("add");
										break;
								}
							} else
								Console.Write("\nInvalid syntax, requires at least one argument\n\n");
							break;
						#endregion
						case "status":
						case "info":
							#region
							Console.WriteLine("\n" + user + "\n");
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
							Console.Write("\"" + cmds[0] + "\" is not a valid command:\n");
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
