using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HouseCS.Items;

namespace HouseCS {
	internal class Program {
		private static readonly int verMajor = 1;
		private static readonly int verMinor = 1;
		private static readonly int verFix = 0;
		private static string CurVer => $"{verMajor}.{verMinor}.{verFix}";
		public static readonly string RED = "31";
		public static readonly string GREEN = "32";
		public static readonly string YELLOW = "33";
		public static readonly string BLUE = "34";
		public static readonly string PURPLE = "35";
		public static readonly string CYAN = "36";
		public static readonly string WHITE = "37";
		public static string Bright(string color) {
			switch (color.ToLower()) {
				case "red":
					Console.ForegroundColor = ConsoleColor.Red;
					break;
				case "yellow":
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;
				case "green":
					Console.ForegroundColor = ConsoleColor.Green;
					break;
				case "cyan":
					Console.ForegroundColor = ConsoleColor.Cyan;
					break;
				case "blue":
					Console.ForegroundColor = ConsoleColor.Blue;
					break;
				case "purple":
					Console.ForegroundColor = ConsoleColor.Magenta;
					break;
				case "white":
					Console.ForegroundColor = ConsoleColor.White;
					break;
				case "black":
					Console.ForegroundColor = ConsoleColor.DarkGray;
					break;
				case "reset":
					Console.ResetColor();
					break;
				default:
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write("[!cB]");
					Console.ResetColor();
					break;
			}
			return string.Empty;
		}
		public static string Bright(string color, string text) {
			return Bright(color) + text + Bright("reset");
		}
		public static string Color(String color) {
			switch (color.ToLower()) {
				case "red":
					Console.ForegroundColor = ConsoleColor.DarkRed;
					break;
				case "yellow":
					Console.ForegroundColor = ConsoleColor.DarkYellow;
					break;
				case "green":
					Console.ForegroundColor = ConsoleColor.DarkGreen;
					break;
				case "cyan":
					Console.ForegroundColor = ConsoleColor.DarkCyan;
					break;
				case "blue":
					Console.ForegroundColor = ConsoleColor.DarkBlue;
					break;
				case "purple":
					Console.ForegroundColor = ConsoleColor.DarkMagenta;
					break;
				case "white":
					Console.ForegroundColor = ConsoleColor.Gray;
					break;
				case "black":
					Console.ForegroundColor = ConsoleColor.Black;
					break;
				case "reset":
					Console.ResetColor();
					break;
				default:
					Console.ForegroundColor = ConsoleColor.DarkRed;
					Console.Write("[!cB]");
					Console.ResetColor();
					break;
			}
			return string.Empty;
		}
		public static string Color(string color, string text) {
			return Color(color) + text + Color("reset");
		}
		public static string Alternate(string color, string text) {
			String ret_val = string.Empty;
			for (int i = 0; i < text.Length; i++) ret_val += i % 2 == 0 ? Bright(color, text.Substring(i, i + 1)) : Color(color, text.Substring(i, i + 1));
			return ret_val + Color("reset");
		}
		private static string Help(string cmd) {
			switch (cmd) {
				case "add":
					return $"\n{Bright("purple", "Syntax")} is: {Bright("blue", "add ")}{Bright("red", "item ")}{Bright("green", "[arg]\n\n")}" +
						$"\t{Bright("red", "item")} - must be a valid type\n" +
						$"\t {Bright("green", "arg")} - causes you to be prompted for the requried info to create a new\n" +
						$"\t                {Bright("yellow", "Item")} of this type (without {Bright("green", "arg")}, a default {Bright("yellow", "Item")} is created)\n\n" +
						$"{Color("blue")}Adds{Bright("yellow", " Item ")}to the current floor\n\n";
				case "attach":
					return $"\n{Bright("purple", "Syntax")} is: {Bright("blue", "attach ")}{Bright("red", "src dst ")}{Bright("green", "[-d]\n\n")}" +
						$"\t{Bright("red", "src")} - {Bright("cyan", "integer")} of an {Bright("red", "Item")} on the current floor (when used with {Bright("green", "-d")}, {Bright("red", "src\n")}" +
						$"\t      must be the {Bright("cyan", "integer")} of the {Bright("yellow", "Item")} that is attached)\n" +
						$"\t{Bright("red", "dst")} - {Bright("cyan", "integer")} of an {Bright("yellow", "Item")} on the current floor\n" +
						$"\t {Bright("green", "-d")} - {Color("blue", "detaches")}{Color("red", " source")} from {Color("red", "destination\n\n")}" +
						$"{Color("blue", "[De/A]ttaches ")}{Bright("red", "src")} [from/to] {Bright("red", "dst")}.\n\n";
				case "clear":
					return $"{Bright("purple", "\nSyntax")} is: {Bright("blue", "clear\n\n")}" +
						$"{Color("blue", "Clears")} the console, and places cursor at home position\n\n";
				case "down":
					return $"{Bright("purple", "\nSyntax")} is: {Bright("blue", "down\n\n")}" +
						$"Moves to the next floor {Bright("blue", "down")}, unless you are at the bottom\n\n";
				case "exit":
					return $"{Bright("purple", "\nSyntax")} is: {Bright("blue", "exit\n\n")}" +
						"Stops the program, and returns to your command line/operating environment\n\n";
				case "grab":
					return $"{Bright("purple", "\nSyntax")} is: {Bright("blue", "grab ")}{Bright("red")}item\n\n" +
						$"\titem{Bright("reset")} - {Bright("cyan", "integer")} of {Bright("yellow", "Item")} (see {Bright("blue", "list")})\n\n" +
						$"Changes the \"Viewer\"'s current {Bright("yellow", "Item\n\n")}";
				case "help":
					return $"{Bright("purple", "\nSyntax")} is: {Bright("blue", "help ")}{Bright("red")}[command]\n\n" +
						$"\tcommand{Bright("reset")} - a valid {Color("blue", "command\n\n")}" +
						"Colors:\n" +
						$"\t   {Alternate("red", "red")} - warning -or- argument name (usually an integer)\n" +
						"\t         dark: usually expanded name of a commands argument (to show\n" +
						"\t         meaning)\n" +
						$"\t{Alternate("yellow", "yellow")} - Item\n" +
						"\t         dark: when talking about an Item but not using the exact term\n" +
						"\t         \"Item\" (or the exact name of an Item)\n" +
						$"\t {Alternate("green", "green")} - string argument (type it as it appears [without any brackets])\n" +
						"\t         dark: (no use yet)\n" +
						$"\t  {Alternate("cyan", "cyan")} - an integer for use when a command requires an Item number\n" +
						"\t         dark: Item integer for sub-items (ie: a book in a bookshelf)\n" +
						$"\t  {Alternate("blue", "blue")} - command\n" +
						"\t         dark: when referencing the command without using its exact name\n\n";
				case "info":
					return $"{Bright("purple", "\nSyntax")} is: {Bright("blue", "info\n\n")}" +
						$"Returns {Bright("blue", "info")} about the current 'Viewer'\n\n";
				case "list":
					return $"{Bright("purple", "\nSyntax")} is: {Bright("blue", "list ")}[{Bright("red", "item")}] [({Bright("green", "-h")} / {Bright("green", "-f")})] [{Bright("green", "-r ")}{Bright("red", "start end")}] [{Bright("green", " -p")}] [{Bright("green", "-i ")}{Bright("yellow", "Item")}]\n\n" +
						$"\t   {Bright("red", "item")} - {Bright("cyan", "integer")} of {Bright("yellow", "Item")} (see {Bright("blue", "list")})\n" +
						$"\t{Bright("green", "-h / -f")} - will show the the \"Viewer\"'s current {Bright("yellow", "Item\n")}" +
						$"\t          Long version is {Bright("green", "--hand")} or {Bright("green")}--focus\n" +
						$"\t     -r{Bright("reset")} - will {Bright("blue", "list")} {Color("yellow", "Items")} between [{Bright("red", "start")}] and [{Bright("red", "end")}]\n" +
						$"\t          Long version is {Bright("green")}--range\n" +
						$"\t     -p{Bright("reset")} - {Color("blue", "lists")} all {Color("yellow", "Items")} on the floor one page at a time (page is\n" +
						"\t          defined as 20 lines)\n" +
						$"\t          Long version is {Bright("green")}--page\n" +
						$"\t-i {Bright("yellow", "Item")} - {Color("blue", "lists")} all {Color("yellow", "Items")} of type {Bright("yellow", "Item")} ({Bright("yellow", "Item")} string)\n" +
						$"\t          Long version is {Bright("green", "--item\n\n")}" +
						$"Used for getting info about an {Bright("yellow", "Item")}, or multiple {Color("yellow", "Items")}.\n\n";
				case "move":
					return $"{Bright("purple", "\nSyntax")} is: {Bright("blue", "move ")}{Bright("red")}item floor\n\n" +
						$"\t item{Bright("reset")} - {Bright("cyan", "integer")} of {Bright("yellow", "Item")} (see {Bright("blue", "list")})\n" +
						$"\t{Bright("red", "floor")} - {Bright("cyan", "integer")} of floor (or: {Bright("green", "<")} for next floor down or {Bright("green", ">")} for next floor\n" +
						"\t        up)\n\n" +
						$"{Color("blue", "Moves")} an {Bright("yellow", "Item")} from your current floor to the specified floor.\n\n";
				case "remove":
					return $"{Bright("purple", "\nSyntax")} is: {Bright("blue", "remove ")}{Bright("red")}item\n\n" +
						$"\titem{Bright("reset")} - {Bright("cyan", "integer")} of {Bright("yellow", "Item")} (see {Bright("blue", "list")})\n\n" +
						$"{Color("blue", "Removes")} specified {Bright("yellow", "Item")} from current floor.\n\n";
				case "up":
					return $"{Bright("purple", "\nSyntax")} is: {Bright("blue", "up\n\n")}" +
						$"Moves to the next floor {Bright("blue", "up")}, unless you are at the top\n\n";
				case "ver":
					return $"{Bright("purple", "\nSyntax")} is: {Bright("blue", "vern\n")}" +
						$"Tells you the current {Bright("blue", "version")} of the Check Command Interpretter\n\n";
				default:
					return $"{Bright("red")}Code error!!! (Please report, as this message shouldn't be possible to see.)";
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
															Console.WriteLine(((Bookshelf)dst_i).RemoveBook(src));
															break;
														case "Display":
															Console.WriteLine(((Display)dst_i).Disconnect(src));
															break;
														default:
															Console.Write($"That {Bright("yellow", "Item")} cannot have things {Color("blue", "detached")} from it.\n");
															break;
													}
												} else
													Console.Write($"Invalid argument, did you mean {Bright("green", "-d")}?\n");
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
															Console.WriteLine("\n" + ((Display)dst_i).Connect(src_i));
														else
															Console.Write($"{Bright("yellow", "Item ")}src cannot connect to a {Bright("yellow", "Display")}.\n");
														break;
													default:
														Console.Write("Item cannot have things attached to it.\n");
														break;
												}
											} else
												Console.Write($"The floor only has {Bright("cyan", user.FloorSize.ToString())} items.\n");
										} else
											Console.Write($"{Color("red", "Item")} must be an {Bright("cyan", "integer")}.\n");
									} else
										Console.Write($"{Bright("blue", "\nAttach")} it to what?\n\n");
								} else
									Console.Write($"{Color("red", "Item")} must be an {Bright("cyan", "integer")}.\n");
							} else
								Console.Write($"{Bright("blue", "\nAttach")} what to what?\n\n");
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
													Console.Write($"\nThis {Bright("yellow", "Item ")}{Bright("blue", "moved")} to floor {destination}\n{user.curItem}\n\n");
												} else
													Console.Write("Floor does not exist.\n");
											} else
												Console.Write($"{Bright("yellow", "Item")} does not exist.\n");
											user.curItem = old_item;
										} else
											Console.Write($"Floor must be an {Bright("cyan", "integer")}, or: {Bright("green", "<")} or {Bright("green", ">")}.\n");
									} else
										Console.Write($"{Bright("blue", "\nMove")} it where?\n\n");
								} else
									Console.Write($"{Bright("yellow", "Item")} must be an {Bright("cyan", "integer")}.\n");
							} else
								Console.Write($"{Bright("blue", "\nMove")} what, and where?\n\n");
							break;
						#endregion
						case "grab":
						case "select":
							#region
							if (cmds.Length > 1) {
								if (Regex.IsMatch(cmds[1], "[0-9]+")) {
									if (cmds.Length > 2 ) {
										if (Regex.IsMatch(cmds[2], "[0-9]+")) {
											switch (user.ChangeItemFocus(int.Parse(cmds[1]), int.Parse(cmds[2]))) {
												case 0:
													Console.Write($"\nThis {Bright("yellow", "Item")} selected: (of type {Bright("yellow", user.curItem.Type)})\n\n{user.curItem}\n\n");
													break;
												case 1:
													Console.Write($"Either {Bright("yellow", "Item ")}{Bright("cyan", cmds[1])} doesn't have any {Bright("yellow", "sub-Items")}, or the {Bright("cyan", "integer")} you entered is too high.\n");
													break;
												case 2:
													Console.Write($"\"{cmds[1]}\" is invalid, must be less than the floor {Bright("yellow", "Item")} size of: {Bright("cyan", user.FloorSize.ToString())}\n");
													break;
												default:
													Console.Write($"{Bright("red", "ERROR: get sub-item did not return 0, 1, or 2. Please report this!\n")}");
													break;
											}
										} else
											Console.Write($"\"{cmds[2]}\" is not a valid {Bright("cyan", "integer\n")}");
									} else if (user.ChangeItemFocus(int.Parse(cmds[1]))) Console.Write($"\nThis {Bright("yellow", "Item")} selected: (of type {Bright("yellow", user.curItem.Type)})\n\n{user.curItem}\n\n");
									else
										Console.Write($"\"{cmds[1]}\" is invalid, must be less than the floor {Bright("yellow", "Item")} size of: {Bright("cyan", user.FloorSize.ToString())}\n");
								} else
									Console.Write($"\"{cmds[1]}\" is not a valid {Bright("cyan", "integer\n")}");
							} else
								Console.Write($"{Bright("blue", "\nGrab")} what?\n\n");
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
														Console.Write($"\nThis {Bright("yellow", "Item")} is:\n{user.curItem}\n\n" +
															$"{Bright("red")}Are you sure you want to delete this? [Y/N] > ");
														user.curItem = tempItem;
														string yenu = Console.ReadLine().ToUpper();
														Console.WriteLine(Bright("reset"));
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
													Console.Write($"This {Bright("yellow", "Item")} either has no {Color("yellow", "sub-Items")} on it, or the {Bright("cyan", "integer")} is too high\n");
													break;
												case 2:
													Console.Write($"This floor only has {Bright("cyan", user.FloorSize.ToString())} items on it\n");
													break;
											}
										} else
											Console.Write($"\"{cmds[2]}\" is not a valid {Bright("cyan", "integer\n")}");
									} else if (user.ChangeItemFocus(int.Parse(cmds[1]))) {
										while (!validAnswer) {
											Console.Write($"\nThis {Bright("yellow", "Item")} is:\n{user.curItem}\n\n" +
												$"{Bright("red")}Are you sure you want to delete this? [Y/N] > ");
											user.curItem = tempItem;
											string yenu = Console.ReadLine().ToUpper();
											Console.WriteLine(Bright("reset"));
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
										Console.Write($"This floor only has {Bright("cyan", user.FloorSize.ToString())} items on it\n");
								} else
									Console.Write($"\"{cmds[1]}\" is not a valid {Bright("cyan", "integer\n")}");
							} else
								Console.Write($"{Bright("blue", "\nRemove")} what?\n\n");
							break;
						#endregion
						case "list":
						case "look":
							#region
							if (cmds.Length > 1) {
								if (EqualsIgnoreCaseOr(cmds[1], new string[] { "--hand", "--focus", "-h", "-f" }))
									Console.Write("\n" + user.ViewCurItem + "\n\n");
								else if (EqualsIgnoreCaseOr(cmds[1], new string[] { "-i", "--item" })) {
									if (cmds.Length > 2)
										Console.WriteLine(user.List(cmds[2]));
									else
										Console.Write($"No {Bright("red", "Item")} type specified.\n");
								} else if (EqualsIgnoreCaseOr(cmds[1], new string[] { "-p", "--page" })) {
									for (int i = 0; i < user.FloorSize / 20 + (user.FloorSize % 20 == 0 ? 0 : 1); i++) {
										Console.Write($"\n\tFloor {Color("blue", "Listing")} - Page {(i + 1)}");
										bool end_test = 20 * (i + 1) < user.FloorSize;
										Console.Write(user.List(20 * i, end_test ? 20 * (i + 1) : user.FloorSize) + "\n\n");
										if (end_test) {
											Console.Write("Press enter to continue > ");
											Console.ReadLine();
										}
									}
								} else if (EqualsIgnoreCaseOr(cmds[1], new string[] { "-r", "--range" })) {
									if (cmds.Length > 3 && MatchesAnd(new string[] { cmds[2], cmds[3] }, "[0-9]+"))
										Console.WriteLine(user.List(int.Parse(cmds[2]), int.Parse(cmds[3]) + 1));
									else
										Console.Write($"{Bright("blue", "range")} requires {Bright("cyan", "2 integers\n")}");
								} else if (Regex.IsMatch(cmds[1], "[0-9]+")) {
									if (int.Parse(cmds[1]) < user.FloorSize) {
										IItem temp_item = user.curItem;
										user.ChangeItemFocus(int.Parse(cmds[1]));
										switch (user.curItem.Type) {
											case "Bookshelf":
												Console.Write($"This {Bright("yellow", "Item")} is a {Bright("yellow", "Bookshelf")}, would you like to see:\n" +
													$"(Y) A specific {Bright("yellow", "Book")}\n(N) Just the {Bright("yellow", "Bookshelf")}\n\n");
												while (true) {
													Console.Write("[Y/N] > ");
													string temp = Console.ReadLine().ToUpper();
													int bC = ((Bookshelf)user.curItem).BookCount;
													if (temp.Equals("Y") && bC > 0) {
														Console.Write($"\nWhich {Bright("yellow", "Book:\n\n")}");
														while (true) {
															Console.Write($"[{Bright("cyan", "0")}-{Bright("cyan", (bC - 1).ToString())}] > ");
															int bk = int.Parse(Console.ReadLine());
															if (bk < bC) {
																Console.Write($"\n{((Bookshelf)user.curItem).GetBook(bk)}");
																break;
															}
														}
													}
													if (temp.Equals("N") || ((Bookshelf)user.curItem).BookCount == 0) Console.Write($"\n{user.ViewCurItem}");
													Console.WriteLine();
													if (EqualsIgnoreCaseOr(temp, new string[]{"Y", "N"})) break;
												}
												Console.WriteLine();
												break;
											case "Display":
												Console.Write($"This {Bright("yellow", "Item")} is a {Bright("yellow", "Display")}, would you like to see:\n" +
													$"(Y) A specific device\n(N) Just the {Bright("yellow", "Display\n\n")}");
												while (true) {
													Console.Write("[Y/N] > ");
													string temp = Console.ReadLine().ToUpper();
													if (temp.Equals("Y") && ((Display)user.curItem).DeviceCount > 0) {
														Console.Write("\nWhich device:\n\n");
														while (true) {
															Console.Write($"[{Bright("cyan", "0")}-{Bright("cyan", (((Display)user.curItem).DeviceCount - 1).ToString())}] > ");
															int dv = int.Parse(Console.ReadLine());
															if (dv < ((Display)user.curItem).DeviceCount) {
																Console.Write($"\n{((Display)user.curItem).GetDevice(dv)}");
																break;
															}
														}
													}
													if (temp.Equals("N") || ((Display)user.curItem).DeviceCount == 0) Console.Write($"\n{user.ViewCurItem}");
													Console.WriteLine();
													if (EqualsIgnoreCaseOr(temp, new string[]{"Y", "N"})) break;
												}
												Console.WriteLine();
												break;
											case "Book":
											case "Computer":
											case "Console":
											case "Bed":
												Console.Write("\n" + user.ViewCurItem + "\n\n");
												break;
										}
										user.curItem = temp_item;
									} else
										Console.Write($"This floor only has {user.FloorSize}{Color("yellow", " Items")} on it\n");
								} else
									Console.Write($"\"{cmds[1]}\" is not a valid {Bright("cyan", "integer\n")}");
							} else
								Console.WriteLine(user.List());
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
												Console.Write($"\nHow many {Color("yellow", "books")} will be on this {Color("yellow", "shelf")}? > ");
												int length = int.Parse(Console.ReadLine());
												Console.WriteLine();
												for (int i = 0; i < length; i++) {
													Console.Write($"{Bright("yellow", "Book ")}{Bright("cyan", i.ToString())}\n");
													Console.Write($"\nEnter {Bright("yellow", "Book")} Title > ");
													string title = Console.ReadLine();
													Console.Write($"\nEnter {Bright("yellow", "Book")} Author > ");
													string author = Console.ReadLine();
													Console.Write("\nEnter Publishing Year > ");
													int year = int.Parse(Console.ReadLine());
													Console.WriteLine();
													tempShelf.AddBook(new Book(title, author, year));
												}
												Console.Write($"\nThis {Bright("yellow", "Bookshelf")} created:\n{tempShelf}\n\n");
											} else
												Console.Write($"\nInvalid 2nd argument, did you mean {Bright("green", "arg")}?\n\n");
										} else
											Console.Write($"\nNew {Bright("yellow", "Bookshelf")} added to floor {Bright("cyan", user.CurFloor.ToString())}.\n\n");
										user.AddItem(tempShelf);
										break;
									case "book":
										Book tempBook = new Book();
										if (cmds.Length > 2) {
											if (string.Equals(cmds[2], "arg", StringComparison.OrdinalIgnoreCase)) {
												Console.Write($"\nEnter {Bright("yellow", "Book")} Title > ");
												string title = Console.ReadLine();
												Console.Write($"\nEnter {Bright("yellow", "Book")} Author > ");
												string author = Console.ReadLine();
												Console.Write("\nEnter Publishing Year > ");
												int year = int.Parse(Console.ReadLine());
												tempBook.Reset(title, author, year);
												Console.Write($"\nThis {Bright("yellow", "Book")} added:\n{tempBook}\n\n");
											} else
												Console.Write($"\nInvalid 2nd argument, did you mean {Bright("green", "arg")}?\n\n");
										} else
											Console.Write($"\nNew {Bright("yellow", "Book")} added to floor {Bright("cyan", user.CurFloor.ToString())}.\n\n");
										user.AddItem(tempBook);
										break;
									case "computer":
										Computer temp_comp = new Computer();
										if (cmds.Length > 2) {
											if (string.Equals(cmds[2], "arg", StringComparison.OrdinalIgnoreCase)) {
												Console.Write($"\nWhat kind of {Bright("yellow", "Computer")} is it? (Desktop, Laptop, etc) > ");
												string type = Console.ReadLine();
												Console.Write($"\n{Bright("yellow", "Computer")} Brand (ie: HP, Microsoft) > ");
												string brand = Console.ReadLine();
												Console.Write($"{Bright("yellow", "Computer")} Family (ie: Pavilion, Surface) > ");
												string family = Console.ReadLine();
												Console.Write($"{Bright("yellow", "Computer")} Model (ie: dv6, Pro 3) > ");
												string model = Console.ReadLine();
												Console.Write("\nIs it on? (Invalid input will default to no)\nYes or no? [Y/N] > ");
												string is_on = Console.ReadLine().ToUpper();
												temp_comp.Reset(brand, family, model, is_on.Equals("Y"), type);
												Console.Write($"\nThis {Bright("yellow", "Computer")} added:\n{temp_comp}\n\n");
											} else
												Console.Write($"\nInvalid 2nd argument, did you mean {Bright("green", "arg")}?\n\n");
										} else
											Console.Write($"\nNew {Bright("yellow", "Computer")} added to floor {Bright("cyan", user.CurFloor.ToString())}.\n\n");
										user.AddItem(temp_comp);
										break;
									case "console":
										GameConsole tempConsole = new GameConsole();
										if (cmds.Length > 2) {
											if (string.Equals(cmds[2], "arg", StringComparison.OrdinalIgnoreCase)) {
												for (int i = 0; i < GameConsole.types.Length; i++) Console.Write($"{Bright("cyan", i.ToString())}: {GameConsole.types[i]} ");
												Console.WriteLine();
												Console.Write($"\nEnter {Bright("yellow", "Console")} Type > ");
												int tempType = int.Parse(Console.ReadLine());
												Console.Write($"\nEnter {Bright("yellow", "Console")} Manufacturer (ie Nintendo) > ");
												string com = Console.ReadLine();
												Console.Write($"\nEnter {Bright("yellow", "Console")} Name (ie GameCube) > ");
												string sys = Console.ReadLine();
												tempConsole = new GameConsole(tempType, com, sys);
												Console.Write($"\nThis {Bright("yellow", "Console")} added:\n{tempConsole}\n\n");
											} else
												Console.Write($"\nInvalid 2nd argument, did you mean {Bright("green", "arg")}?\n\n");
										} else
											Console.Write($"\nNew {Bright("yellow", "Console")} added to floor {Bright("cyan", user.CurFloor.ToString())}.\n\n");
										user.AddItem(tempConsole);
										break;
									case "display":
										Display tempDisp = new Display();
										if (cmds.Length > 2) {
											if (string.Equals(cmds[2], "arg", StringComparison.OrdinalIgnoreCase)) {
												Console.Write("\nIs it a Monitor (Y) or a TV (N)?\nWill default to (Y)es if next input is invalid.\n[Y/N] > ");
												string is_mon = Console.ReadLine().ToUpper();
												Console.Write($"\nType the number for each device connected to this {Bright("yellow", "Display")} seperated by a space.\n(Optional)\n> ");
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
												Console.Write($"\n\nEnter the {Color("yellow", "Display's")} size in inches (decimals allowed) > ");
												double size = double.Parse(Console.ReadLine());
												List<IItem> new_items = new List<IItem>();
												foreach (int id in added)
													new_items.Add(user.GetItem(id));
												tempDisp = new Display(is_mon.Equals("N"), new_items, size);
												Console.Write($"\nThis {Bright("yellow", "Display")} added:\n{tempDisp}\n\n");
											} else
												Console.Write($"\nInvalid 2nd argument, did you mean {Bright("green", "arg")}?\n\n");
										} else
											Console.Write($"\nNew {Bright("yellow", "Display")} added to floor {Bright("cyan", user.CurFloor.ToString())}.\n\n");
										user.AddItem(tempDisp);
										break;
									case "bed":
										Bed tempBed = new Bed();
										if (cmds.Length > 2) {
											if (cmds[2].Equals("arg", StringComparison.OrdinalIgnoreCase)) {
												Console.Write($"\nIs this {Bright("yellow", "Bed")} adjustable? (Invalid input will default to N)\n[Y/N] > ");
												bool canMove = Console.ReadLine().ToUpper().Equals("Y");
												Console.WriteLine();
												for (int i = 0; i < Bed.types.Length; i++) Console.Write($"[{Bright("cyan", i.ToString())}] {Bed.types[i]} ");
												Console.Write($"\nInvalid input defaults to {Bright("cyan", "2")}");
												Console.Write($"\n[{Bright("cyan", "0")}-{Bright("cyan", (Bed.types.Length - 1).ToString())}] > ");
												string typeInput = Console.ReadLine();
												int bedType = Regex.IsMatch(typeInput, "[0-9]+") && int.Parse(typeInput) < Bed.types.Length ? int.Parse(typeInput) : 2;
												tempBed = new Bed(canMove, bedType);
												Console.Write($"\nThis {Bright("yellow", "Bed")} added:\n{tempBed}\n\n");
											} else Console.Write($"\nInvalid 2nd argument, did you mean {Bright("green", "arg")}?\n\n");
										} else Console.Write($"\nNew {Bright("yellow", "Bed")} added to floor {Bright("cyan", user.CurFloor.ToString())}.\n\n");
										user.AddItem(tempBed);
										break;
									default:
										Console.Write($"\"{cmds[1]}\" is not a valid {Bright("yellow", "Item")} type:\n");
										for (int i = 0; i < cmds.Length; i++)
											Console.Write(cmds[i] + " ");
										Console.Write("\n" + Help("add"));
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
										Console.Write(Help("add"));
										break;
									case "attach":
										Console.Write(Help("attach"));
										break;
									case "clear":
									case "cls":
										Console.Write(Help("clear"));
										break;
									case "down":
										Console.Write(Help("down"));
										break;
									case "exit":
									case "quit":
										Console.Write(Help("exit"));
										break;
									case "grab":
									case "select":
										Console.Write(Help("grab"));
										break;
									case "help":
										Console.Write(Help("help"));
										break;
									case "info":
									case "status":
										Console.Write(Help("info"));
										break;
									case "list":
									case "look":
										Console.Write(Help("list"));
										break;
									case "move":
										Console.Write(Help("move"));
										break;
									case "remove":
										Console.Write(Help("remove"));
										break;
									case "up":
										Console.Write(Help("up"));
										break;
									case "ver":
									case "version":
										Console.Write(Help("ver"));
										break;
									default:
										Console.Write("\nNo help was found for this command.\n\n");
										break;
								}
							} else {
								Console.WriteLine();
            		Console.Write($"          {Bright("blue")}add{Bright("reset")} - adds an {Bright("yellow", "Item")} to the current floor\n");
            		Console.Write($"       {Bright("blue")}attach{Bright("reset")} - attaches (or detaches) one {Bright("yellow")}Item{Bright("reset")} to (from) another\n");
            		Console.Write($"  {Bright("blue")}clear / cls{Bright("reset")} - clears the screen\n");
            		Console.Write($"         {Bright("blue")}down{Bright("reset")} - goes down 1 floor\n");
            		Console.Write($"  {Bright("blue")}exit / quit{Bright("reset")} - stops the program\n");
            		Console.Write($"{Bright("blue")}grab / select{Bright("reset")} - sets which {Bright("yellow")}Item{Bright("reset")} you are currently focused on\n");
            		Console.Write($"         {Bright("blue")}help{Bright("reset")} - displays this screen, and others\n");
            		Console.Write($"{Bright("blue")}info / status{Bright("reset")} - shows information about you and the {Bright("yellow")}House{Bright("reset")} you are currently in\n");
            		Console.Write($"  {Bright("blue")}list / look{Bright("reset")} - shows the {Bright("yellow")}Item{Bright("reset")}s on the current floor, or shows info about a\n" +
									$"                specific {Bright("yellow")}Item\n");
            		Console.Write($"         {Bright("blue")}move{Bright("reset")} - moves an {Bright("yellow")}Item{Bright("reset")} to another floor\n");
            		Console.Write($"       {Bright("blue")}remove{Bright("reset")} - removes an {Bright("yellow")}Item{Bright("reset")} from the current floor\n");
            		Console.Write($"           {Bright("blue")}up{Bright("reset")} - goes up 1 floor\n");
            		Console.Write($"{Bright("blue")}ver / version{Bright("reset")} - displays information about this command interpretter\n");
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
							Console.Write($"\n{Bright("red", "Ch")}{Bright("green", "ec")}{Bright("blue", "k")} Command Interpretter\n\tVersion {CurVer}\n\n");
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
			Class1 c1 = new Class1();
			Console.WriteLine($"Hello World! {c1.ReturnMessage()}");
		}
	}
}
