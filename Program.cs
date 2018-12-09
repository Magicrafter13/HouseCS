using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HouseCS.Items;

namespace HouseCS {
	internal class Program {
		private static readonly int verMajor = 1;
		private static readonly int verMinor = 0;
		private static readonly int verFix = 0;
		private static string CurVer => $"{verMajor}.{verMinor}.{verFix}";
		private static string Help(string cmd) {
			switch (cmd) {
				case "add":
					return "\nSyntax is: add item [arg]\n\n" +
						"\titem - must be a valid type\n" +
						"\t arg - causes you to be prompted for the requried info to create a new" +
						"\t       item of this type (without arg, a default item is created)\n\n" +
						"Adds item to the current floor\n\n";
				case "attach":
					return "\nSyntax is: attach src dst [-d]\n\n" +
						"\tsrc - must be a valid integer of an item on the current floor\n" +
						"\t      (when used with -d, src must be the integer of the item that is\n" +
						"\t      attached)\n" +
						"\tdst - must be a valid integer of an item on the current floor\n" +
						"\t -d - detaches source from destination\n\n" +
						"[De/A]ttaches src [from/to] dst.\n\n";
				case "clear":
					return "\nSyntax is: clear\n\n" +
						"Clears the console, and places cursor at home position\n\n";
				case "down":
					return "\nSyntax is: down\n\n" +
						"Moves to the next floor down, unless you are at the bottom\n\n";
				case "exit":
					return "\nSyntax is: exit\n\n" +
						"Stops the program, and returns to your command line/operating environment\n\n";
				case "grab":
					return "\nSyntax is: grab item\n\n" +
						"\titem - integer of item (see list)\n\n" +
						"Changes the \"Viewer\"'s current item\n\n";
				case "info":
					return "\nSyntax is: info\n\n" +
						"Returns info about the current 'Viewer'\n\n";
				case "list":
					return "\nSyntax is: list [item] [(-h / -f)] [-r start end] [-p] [-i item]\n\n" +
						"\t   item - integer of item (see list)\n" +
						"\t-h / -f - will show the the \"Viewer\"'s current item\n" +
						"\t          Long version is --hand or --focus\n" +
						"\t     -r - will list items between [start] and [end] (start and end are\n" +
						"\t          both positive integers)\n" +
						"\t          Long version is --range\n" +
						"\t     -p - lists all items on the floor one page at a time (page is\n" +
						"\t          defined as 20 lines)\n" +
						"\t          Long version is --page\n" +
						"\t-i item - lists all items of type 'item' (item string)\n" +
						"\t          Long version is --item\n\n" +
						"Used for getting info about an item, or multiple items.\n\n";
				case "move":
					return "\nSyntax is: move item floor\n\n" +
						"\t item - integer of item (see list)\n" +
						"\tfloor - integer of floor (or: < for next floor down or > for next floor\n" +
						"\t        down)\n\n" +
						"Moves an item from your current floor to the specified floor.\n\n";
				case "remove":
					return "\nSyntax is: remove item\n\n" +
						"\titem - integer of item (see list)\n\n" +
						"Removes specified item from\n\n";
				case "up":
					return "\nSyntax is: up\n\n" +
						"Moves to the next floor up, unless you are at the top\n\n";
				case "ver":
					return "\nSyntax is: ver\n\n" +
						"Tells you the current version of the Heck Command Interpretter\n\n";
				default:
					return "Code error!!! (Please report, as this message shouldn't be possible to see.)";
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
															Console.Write("Item cannot have things detached from it.\n");
															break;
													}
												} else
													Console.Write("Invalid argument, did you mean -d?\n");
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
															Console.Write("Item " + src + " cannot connect to a display.\n");
														break;
													default:
														Console.Write("Item cannot have things attached to it.\n");
														break;
												}
											} else
												Console.Write("The floor only has " + user.FloorSize + " items.\n");
										} else
											Console.Write("Item must be an integer.\n");
									} else
										Console.Write("\nAttach it to what?\n\n");
								} else
									Console.Write("Item must be an integer.\n");
							} else
								Console.Write("\nAttach what to what?\n\n");
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
													Console.Write("\nThis item moved to floor " + destination + "\n" + user.curItem + "\n\n");
												} else
													Console.Write("Floor does not exist.\n");
											} else
												Console.Write("Item does not exist.\n");
											user.curItem = old_item;
										} else
											Console.Write("Floor must be an integer, or: < or >.\n");
									} else
										Console.Write("\nMove it where?\n\n");
								} else
									Console.Write("Item must be an integer.\n");
							} else
								Console.Write("\nMove what, and where?\n\n");
							break;
						#endregion
						case "grab":
						case "select":
							#region
							if (cmds.Length > 1) {
								if (Regex.IsMatch(cmds[1], "[0-9]+")) {
									if (user.ChangeItemFocus(Math.Abs(int.Parse(cmds[1])))) {
										Console.Write("\nThis item selected: (of type " + user.curItem.Type + ")\n\n");
										Console.Write(user.curItem + "\n\n");
									} else
										Console.Write("\"" + cmds[1] + "\" is invalid, must be less than the floor item size of: " + user.FloorSize + "\n");
								} else
									Console.Write("\"" + cmds[1] + "\" is not a valid integer\n");
							} else
								Console.Write("\nGrab what?\n\n");
							break;
						#endregion
						case "remove":
							#region
							if (cmds.Length > 1) {
								if (Regex.IsMatch(cmds[1], "[0-9]+")) {
									IItem temp_item = user.curItem;
									if (user.ChangeItemFocus(Math.Abs(int.Parse(cmds[1])))) {
										if (user.curItem == temp_item)
											temp_item = new Empty();
										Console.Write("\nThis Item is:\n" + user.curItem + "\n\n" +
													  "Are you sure you want to delete this? [Y/N] > ");
										string yenu = Console.ReadLine().ToUpper();
										Console.WriteLine();
										bool valid_answer = false;
										while (!valid_answer) {
											switch (yenu) {
												case "Y":
													user.RemoveItem(int.Parse(cmds[1]));
													goto case "N";
												case "N":
													valid_answer = true;
													break;
											}
										}
										user.curItem = temp_item;
									} else
										Console.Write("This floor only has " + user.FloorSize + " items on it\n");
								} else
									Console.Write("\"" + cmds[1] + "\" is not a valid integer\n");
							} else
								Console.Write("\nRemove what?\n\n");
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
										Console.Write("No item type specified.\n");
								} else if (EqualsIgnoreCaseOr(cmds[1], new string[] { "-p", "--page" })) {
									for (int i = 0; i < user.FloorSize / 20 + (user.FloorSize % 20 == 0 ? 0 : 1); i++) {
										Console.Write("\n\tFloor Listing - Page " + (i + 1) + "\n\n");
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
										Console.Write("Range requires 2 integers\n");
								} else if (Regex.IsMatch(cmds[1], "[0-9]+")) {
									if (int.Parse(cmds[1]) < user.FloorSize) {
										IItem temp_item = user.curItem;
										user.ChangeItemFocus(int.Parse(cmds[1]));
										switch (user.curItem.Type) {
											case "Bookshelf":
												Console.Write("This item is a bookshelf, would you like to see:\n" +
															  "(Y) A specific book\n(N) Just the bookshelf\n\n");
												bool valid = false;
												while (!valid) {
													Console.Write("[Y/N] > ");
													string temp = Console.ReadLine().ToUpper();
													int b_c = ((Bookshelf)user.curItem).BookCount;
													if (temp.Equals("Y") && b_c > 0) {
														Console.Write("\nWhich book:\n\n");
														bool valid2 = false;
														while (!valid2) {
															Console.Write("[0-" + (b_c - 1) + "] > ");
															int bk = Math.Abs(int.Parse(Console.ReadLine()));
															if (bk < b_c) {
																Console.Write("\n" + ((Bookshelf)user.curItem).GetBook(bk));
																valid2 = true;
															}
														}
													}
													if (temp.Equals("N") || ((Bookshelf)user.curItem).BookCount == 0)
														Console.Write("\n" + user.ViewCurItem);
													if (EqualsIgnoreCaseOr(temp, new string[] { "Y", "N" }))
														valid = true;
													Console.WriteLine();
												}
												Console.WriteLine();
												break;
											case "Display":
												Console.Write("This item is a display, would you like to see:\n" +
															  "(Y) A specific device\n(N) Just the display\n\n");
												bool valid_letter = false;
												while (!valid_letter) {
													Console.Write("[Y/N] > ");
													string temp = Console.ReadLine().ToUpper();
													if (temp.Equals("Y") && ((Display)user.curItem).DeviceCount > 0) {
														Console.Write("\nWhich device:\n\n");
														bool valid_num = false;
														while (valid_num) {
															Console.Write("[0-" + (((Display)user.curItem).DeviceCount - 1) + "] > ");
															int dv = Math.Abs(int.Parse(Console.ReadLine()));
															if (dv < ((Display)user.curItem).DeviceCount) {
																Console.Write("\n" + ((Display)user.curItem).GetDevice(dv));
																valid_num = true;
															}
														}
													}
													if (temp.Equals("N") || ((Display)user.curItem).DeviceCount == 0)
														Console.Write("\n" + user.ViewCurItem);
													if (EqualsIgnoreCaseOr(temp, new string[] { "Y", "N" }))
														valid_letter = true;
													Console.WriteLine();
												}
												Console.WriteLine();
												break;
											case "Book":
											case "Computer":
											case "Console":
												Console.Write("\n" + user.ViewCurItem + "\n\n");
												break;
										}
										user.curItem = temp_item;
									} else
										Console.Write("This floor only has " + user.FloorSize + " items on it\n");
								} else
									Console.Write("\"" + cmds[1] + "\" is not a valid integer\n");
							} else
								Console.Write("\n" + user.List() + "\n\n");
							break;
						#endregion
						case "add":
							#region
							if (cmds.Length > 1) {
								switch (cmds[1]) {
									case "bookshelf":
										Bookshelf temp_shelf = new Bookshelf();
										if (cmds.Length > 2) {
											if (string.Equals(cmds[2], "arg", StringComparison.OrdinalIgnoreCase)) {
												Console.Write("\nHow many books will beon this shelf? > ");
												int length = int.Parse(Console.ReadLine());
												Console.WriteLine();
												for (int i = 0; i < length; i++) {
													Console.Write("Book " + i + "\n");
													Console.Write("\nEnter Book Title > ");
													string title = Console.ReadLine();
													Console.Write("\nEnter Book Author > ");
													string author = Console.ReadLine();
													Console.Write("\nEnter Publishing Year > ");
													int year = int.Parse(Console.ReadLine());
													Console.WriteLine();
													temp_shelf.AddBook(new Book(title, author, year));
												}
												Console.Write("\nThis bookshelf created:\n" + temp_shelf + "\n\n");
											} else
												Console.Write("\nInvalid 2nd argument, did you mean arg?\n\n");
										} else
											Console.Write("\nNew bookshelf added to floor " + user.CurFloor + ".\n\n");
										user.AddItem(temp_shelf);
										break;
									case "book":
										Book temp_book = new Book();
										if (cmds.Length > 2) {
											if (string.Equals(cmds[2], "arg", StringComparison.OrdinalIgnoreCase)) {
												Console.Write("\nEnter Book Title > ");
												string title = Console.ReadLine();
												Console.Write("\nEnter Book Author > ");
												string author = Console.ReadLine();
												Console.Write("\nEnter Publishing Year > ");
												int year = int.Parse(Console.ReadLine());
												temp_book.Reset(title, author, year);
												Console.Write("\nThis book added:\n" + temp_book + "\n\n");
											} else
												Console.Write("\nInvalid 2nd argument, did you mean arg?\n\n");
										} else
											Console.Write("\nNew book added to floor " + user.CurFloor + ".\n\n");
										user.AddItem(temp_book);
										break;
									case "computer":
										Computer temp_comp = new Computer();
										if (cmds.Length > 2) {
											if (string.Equals(cmds[2], "arg", StringComparison.OrdinalIgnoreCase)) {
												Console.Write("\nWhat kind of computer is it? (Desktop, Laptop, etc) > ");
												string type = Console.ReadLine();
												Console.Write("\nComputer Brand (ie: HP, Microsoft) > ");
												string brand = Console.ReadLine();
												Console.Write("Computer Family (ie: Pavilion, Surface) > ");
												string family = Console.ReadLine();
												Console.Write("Computer Model (ie: dv6, Pro 3) > ");
												string model = Console.ReadLine();
												Console.Write("\nIs it on? (Invalid input will default to no)\n Yes or no? [Y/N] > ");
												string is_on = Console.ReadLine().ToUpper();
												temp_comp.Reset(brand, family, model, is_on.Equals("Y"), type);
												Console.Write("\nThis computer added:\n" + temp_comp + "\n\n");
											} else
												Console.Write("\nInvalid 2nd argument, did you mean arg?\n\n");
										} else
											Console.Write("\nNew computer added to floor " + user.CurFloor + ".\n\n");
										user.AddItem(temp_comp);
										break;
									case "console":
										GameConsole temp_console = new GameConsole();
										if (cmds.Length > 2) {
											if (string.Equals(cmds[2], "arg", StringComparison.OrdinalIgnoreCase)) {
												Console.Write("0: " + GameConsole.types[0]);
												for (int i = 1; i < GameConsole.types.Length; i++)
													Console.Write(" " + i + ": " + GameConsole.types[i]);
												Console.WriteLine();
												Console.Write("\nEnter Console Type > ");
												int temp_type = int.Parse(Console.ReadLine());
												Console.Write("\nEnter Console Manufacturer (ie Nintendo) > ");
												string com = Console.ReadLine();
												Console.Write("\nEnter Console Name (ie GameCube) > ");
												string sys = Console.ReadLine();
												temp_console = new GameConsole(temp_type, com, sys);
												Console.Write("\nThis Console added:\n" + temp_console + "\n\n");
											} else
												Console.Write("\nInvalid 2nd argument, did you mean arg?\n\n");
										} else
											Console.Write("\nNew console added to floor " + user.CurFloor + ".\n\n");
										user.AddItem(temp_console);
										break;
									case "display":
										Display temp_disp = new Display();
										if (cmds.Length > 2) {
											if (string.Equals(cmds[2], "arg", StringComparison.OrdinalIgnoreCase)) {
												Console.Write("\nIs it a Monitor (Y) or a TV (N)?\nWill default to (Y)es if next input is invalid.\n[Y/N] > ");
												string is_mon = Console.ReadLine().ToUpper();
												Console.Write("\nType the number for each device connected to this Display seperated by a space.\n(Optional)\n> ");
												string[] con_devs = Regex.Split(Console.ReadLine(), " +");
												List<IItem> valid_devs = new List<IItem>();
												List<int> added = new List<int>();
												List<int> not_added = new List<int>();
												List<string> not_number = new List<string>();
												foreach (string dev in con_devs) {
													if (Regex.IsMatch(dev, "[0-9]+")) {
														int devID = int.Parse(dev);
														if (devID >= 0 && devID < user.FloorSize)
															added.Add(devID);
														else
															not_added.Add(devID);
													} else
														not_number.Add(dev);
												}
												Console.Write("\nAdded: ");
												foreach (int num in added)
													Console.Write(num + " ");
												Console.Write("\n\nNot added: ");
												foreach (int num in not_added)
													Console.Write(num + " ");
												Console.Write("\n\nNot a number: ");
												foreach (string str_num in not_number)
													Console.Write(str_num + " ");
												Console.Write("\n\nEnter the displays size in inches (decimals allowed) > ");
												double size = double.Parse(Console.ReadLine());
												List<IItem> new_items = new List<IItem>();
												foreach (int id in added)
													new_items.Add(user.GetItem(id));
												temp_disp = new Display(is_mon.Equals("N"), new_items, size);
												Console.Write("\nThis display added:\n" + temp_disp + "\n\n");
											} else
												Console.Write("\nInvalid 2nd argument, did you mean arg?\n\n");
										} else
											Console.Write("\nNew display added to floor " + user.CurFloor + ".\n\n");
										user.AddItem(temp_disp);
										break;
									default:
										Console.Write("\"" + cmds[1] + "\" is not a valid Item type:\n");
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
								Console.Write("\nadd - adds item to the current floor\n");
								Console.Write("attach - attaches (or detaches) one item to (from) another\n");
								Console.Write("clear / cls - clears the screen\n");
								Console.Write("down - goes down 1 floor\n");
								Console.Write("exit / quit - stops the program\n");
								Console.Write("grab / select - sets which item you are currently focused on\n");
								Console.Write("help - displays this screen\n");
								Console.Write("info / status - shows information about you and the house you are currently in\n");
								Console.Write("list / look - shows the items on the current floor, or shows info about a\n" +
											  "              specific item\n");
								Console.Write("move - moves an item to another floor\n");
								Console.Write("remove - removes an object from the current floor\n");
								Console.Write("up - goes up 1 floor\n");
								Console.Write("ver / version - displays information about this command interpretter\n");
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
							Console.Write("\nCheck Command Interpretter\n\tVersion " + CurVer + "\n\n");
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
