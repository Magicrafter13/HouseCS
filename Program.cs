﻿using HouseCS.ConsoleUtils;
using HouseCS.Exceptions;
using HouseCS.Items;
using HouseCS.Items.Clothes;
using HouseCS.Items.Containers;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HouseCS
{
	internal class Program
	{
		public const int verMajor = 3, verMinor = 0, verFix = 2;

		public const int maxStreets = 300, maxAvenues = 300, maxHouses = 20; // 300 st x 300 ave, up to 20 houses per road

		public static readonly string[] topTypes = { "Bed", "Book", "Computer", "Console", "Display", "Clothing", "Container", "Printer" };

		public static readonly string[] floorInteracts = { "light (or lights)" };

		public static readonly string[] errorMeaning =
		{
			"success",
			"basic error",
			"bad argument",
			"non-script syntax",
			"code error"
		};

		public static string[,] enviVar = {
			{"interactive", "false", "bool",   "user"  }, // tells the environment whether or not to do certain things
			{"temperature", "70.0",  "double", "system"}, // the ambient house temperature
			{"house",       "0",     "int",    "system"}, // integer representation of current viewer
			{"use_rooms",   "false", "bool",   "user"  }, // off by default so previous users aren't confused as to where their items are
			{"cur_room",    "-1",    "int",    "system"}, // current room to get items from
			{"errorlevel",  "0",     "int",    "system"}, // is updated when ErrorLevel is set
		};


		private static void Main(string[] args)
		{
			if (args == null)
				throw new ArgumentNullException(nameof(args));
			List<List<string>> cmds = new List<List<string>>()
			{
				new List<string>()
			};

			//This is to keep the contents of my actual house a little more private.
			//Just make your own .cs file that returns Items. (See example)
			ItemImport.InitializeItems();
			string[][] Scripts = ItemImport.LoadScripts();
			List<House> houseData = ItemImport.houses;
			List<Viewer> viewers = new List<Viewer>();
			foreach (House h in houseData)
				viewers.Add(new Viewer(h));
			List<House>[,,] localMap = new List<House>[4, maxStreets, maxAvenues];
			for (int q = 0; q < 4; q++) {
				for (int s = 0; s < maxStreets; s++) {
					for (int a = 0; a < maxAvenues; a++) {
						localMap[q, s, a] = new List<House>(maxHouses); //add roads
					}
				}
			}

			Viewer user = viewers[0];
			bool here = true;

			for (int h = 0; h < houseData.Count; h++)
				localMap[
					houseData[h].Quadrant,
					(houseData[h].Street ? houseData[h].ConRoad : houseData[h].AdjRoad) - 1,
					(houseData[h].Street ? houseData[h].AdjRoad : houseData[h].ConRoad) - 1
				].Add(houseData[h]);

			int timesExpanded = 0;

			while (here) {
				if (cmds.Count == 0 || cmds[0].Count == 0) {
					Console.Write("> ");
					List<string> input = new List<string>(Regex.Split(Console.ReadLine(), " +"));
					if (cmds.Count == 0)
						cmds.Add(input);
					else
						cmds[0] = input;
				}
				else {
					if (!cmds[0][0].Equals("echo", StringComparison.OrdinalIgnoreCase)) {
						Console.Write("> ");
						foreach (string t in cmds[0])
							Console.Write($"{t} ");
						Console.WriteLine();
					}
				}
				if (string.IsNullOrWhiteSpace(cmds[0][^1]))
					cmds[0] = new List<string>(cmds[0].GetRange(0, cmds[0].Count - 1));
				int lineCheck = 0;
				timesExpanded = 0;
				for (int i = 0; i < cmds[lineCheck].Count; i++) {
					if (cmds[lineCheck][i].Equals("&&")) {
						cmds.Insert(lineCheck + 1, new List<string>(cmds[lineCheck].GetRange(i + 1, cmds[lineCheck].Count - i - 1)));
						cmds[lineCheck].RemoveRange(i, cmds[lineCheck].Count - i);
						timesExpanded++;
					}
				}
				if (cmds[0].Count > 0) {
					switch (cmds[0][0].ToLower()) {
						case "": {
							break;
						}
						// ^ Keep this on top, might not affect performance, but if it does, just keep it here ^
						case "echo": {
							for (int t = 1; t < cmds[0].Count; t++)
								WriteColor(new ColorText($"{cmds[0][t]} ", ConsoleColor.Green));
							Console.WriteLine();
							SetErrorLevel(0);
							break;
						}
						case "script": {
							SetErrorLevel(2);
							if (cmds[0].Count > 1) {
								if (Regex.IsMatch(cmds[0][1], @"^[0-9]+$")) {
									int script = int.Parse(cmds[0][1]);
									if (script < Scripts.Length) {
										foreach (string c in Scripts[script])
											cmds.Add(new List<string>(Regex.Split(c, " +")));
										cmds.Add(new List<string>() { "echo" }); // By adding a useless command (with basically no output), we make sure the final command can still return an errorcode.
										SetErrorLevel(0);
									}
								}
							}
							else SetErrorLevel(1);
							break;
						}
						case "ren":
						case "rename": {
							SetErrorLevel(1);
							if (cmds[0].Count > 1) {
								if (Regex.IsMatch(cmds[0][1], @"^\d+$")) {
									IItem i = user.GetItem(int.Parse(cmds[0][1]));
									if (!(i is Empty)) {
										if (cmds[0].Count > 2) {
											string name = cmds[0][2];
											for (int s = 3; s < cmds[0].Count; s++)
												name += $" {cmds[0][s]}";
											i.Rename(name);
											WriteColor(new string[] { "Item", " renamed\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.Blue });
											SetErrorLevel(0);
										}
										else WriteColor(new string[] { "Rename", " it to what?\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
									}
									else WriteColor(new string[] { "This floor only has ", $"{user.FloorSize.ToString()}{SetErrorLevel(2)}", " items on it\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
								}
								else WriteColor(new string[] { "item", " must be a positive ", $"integer\n{SetErrorLevel(2)}" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan });
							}
							else WriteColor(new string[] { "Rename", " what to what?\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
							break;
						}
						case "map": {
							Console.WriteLine("Soon...");
							break;
						}
						case "search":
						case "find": {
							SetErrorLevel(0);
							string searchItem = string.Empty;
							List<string> keywords = new List<string>();
							int searchFloor = -1, searchRoom = -2;
							List<string> removedTokens = new List<string>();
							for (int arg = 1; arg < cmds[0].Count; arg++) {
								if (string.IsNullOrEmpty(searchItem) && EqualsIgnoreCaseOr(cmds[0][arg], new string[] { "-t", "--type" }) && arg < cmds[0].Count - 1) {
									searchItem = cmds[0][arg + 1];
									removedTokens.Add(cmds[0][arg]);
									cmds[0].RemoveAt(arg);
									removedTokens.Add(cmds[0][arg]);
									cmds[0].RemoveAt(arg);
									arg--;
								}
								else if (searchFloor == -1 && EqualsIgnoreCaseOr(cmds[0][arg], new string[] { "-f", "--floor" }) && arg < cmds[0].Count - 1) {
									if (Regex.IsMatch(cmds[0][arg + 1], @"^\d+$")) {
										searchFloor = int.Parse(cmds[0][arg + 1]);
										removedTokens.Add(cmds[0][arg]);
										cmds[0].RemoveAt(arg);
										removedTokens.Add(cmds[0][arg]);
										cmds[0].RemoveAt(arg);
										arg--;
									}
									else {
										WriteColor(new string[] { "floor", " must be a positive ", "integer", ".\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
										SetErrorLevel(2);
									}
								}
								else if (searchRoom == -2 && EqualsIgnoreCaseOr(cmds[0][arg], new string[] { "-r", "--room" }) && arg < cmds[0].Count - 1) {
									if (Regex.IsMatch(cmds[0][arg + 1], @"^(-1|\d+)$")) {
										searchRoom = int.Parse(cmds[0][arg + 1]);
										removedTokens.Add(cmds[0][arg]);
										cmds[0].RemoveAt(arg);
										removedTokens.Add(cmds[0][arg]);
										cmds[0].RemoveAt(arg);
										arg--;
									}
									else {
										WriteColor(new string[] { "room", " must be a positive ", "integer", " or ", "-1", ".\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
										SetErrorLevel(2);
									}
									continue;
								}
								else {
									if (arg + 1 < cmds[0].Count && Regex.IsMatch(cmds[0][arg], "^\"")) {
										keywords.Add(string.Empty);
										bool start = true;
										SetErrorLevel(2);
										while (arg < cmds[0].Count) {
											keywords[keywords.Count - 1] += $"{(start ? string.Empty : " ")}{cmds[0][arg].Substring(start ? 1 : 0)}";
											start = false;
											removedTokens.Add(cmds[0][arg]);
											cmds[0].RemoveAt(arg);
											if (removedTokens[^1][^1].Equals('"') && !removedTokens[^1].Substring(removedTokens[^1].Length - 2, 2).Equals("\\\"")) {
												keywords[keywords.Count - 1] = keywords[keywords.Count - 1][0..^1];
												SetErrorLevel(0);
												break;
											}
										}
										if (ErrorLevel == 0)
											keywords[keywords.Count - 1] = keywords[keywords.Count - 1].Replace("\\\"", "\"");
									}
									else {
										keywords.Add(cmds[0][arg]);
										removedTokens.Add(keywords[^1]);
										cmds[0].RemoveAt(arg);
									}
									arg--;
								}
								if (ErrorLevel != 0) break;
							}
							cmds[0].AddRange(removedTokens);
							if (ErrorLevel != 0) break;
							Console.WriteLine($"{searchFloor} {searchRoom}");
							if (keywords.Count == 0) {
								Console.WriteLine("Please enter 1 - 3 keywords: (2 and 3 optional)");
								string[] keys = { "", "", "" };
								do {
									Console.Write("Keyword 1 > ");
									keys[0] = Console.ReadLine();
								} while (string.IsNullOrEmpty(keys[0]));
								Console.Write("Keyword 2 > ");
								keys[1] = Console.ReadLine();
								if (!string.IsNullOrEmpty(keys[1])) {
									Console.Write("Keyword 3 > ");
									keys[2] = Console.ReadLine();
								}
								keywords.Add(keys[0]);
								if (!string.IsNullOrEmpty(keys[1])) keywords.Add(keys[1]);
								if (!string.IsNullOrEmpty(keys[2])) keywords.Add(keys[2]);
							}
							Console.WriteLine("\nSearching for:");
							foreach (string key in keywords)
								WriteColor(new ColorText(new string[] { "\"", key, "\"", "," }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White }));
							Console.WriteLine();
							WriteColor(new string[] { "In: ", (string.IsNullOrEmpty(searchItem) ? "All" : searchItem), " items", ". On floor: ", (searchFloor == -1 ? "all" : searchFloor.ToString()), ". In room: ", (searchRoom == -2 ? "all" : searchRoom == -1 ? "No room" : searchRoom.ToString()), ".\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.DarkYellow, ConsoleColor.White, (searchFloor == -1 ? ConsoleColor.White : ConsoleColor.Cyan), ConsoleColor.White, (searchRoom == -2 || searchRoom == -1 ? ConsoleColor.White : ConsoleColor.Cyan), ConsoleColor.White });
							List<ColorText> output = user.Search(searchFloor, searchRoom, searchItem, keywords);
							if (output.Count > 0)
								WriteColor(output.ToArray());
							else
								Console.WriteLine("No matches found.\n");
							break;
						}
						case "save":
						case "export": {
							if (cmds[0].Count > 1) {
								if (cmds[0].Count > 2) {
									SetErrorLevel(2);
									switch (cmds[0][1].ToLower()) {
										case "-h":
											if (Regex.IsMatch(cmds[0][2], @"^\d+$")) {
												if (int.Parse(cmds[0][2]) >= 0 && int.Parse(cmds[0][2]) < houseData.Count) {
													System.IO.File.WriteAllText("exportedHouse.txt", $"{houseData[int.Parse(cmds[0][2])].Export(int.Parse(cmds[0][2]))}\n");
													Console.WriteLine($"\nHouse {cmds[0][2]} exported.\n");
													SetErrorLevel(0);
												}
												else WriteColor(new string[] { "house", " must be greater than or equal to ", "0", " and less than ", houseData.Count.ToString(), ".\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
											}
											else WriteColor(new string[] { $"{cmds[0][2]} is not a valid ", "integer", ".\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
											break;
										case "-f":
											if (Regex.IsMatch(cmds[0][2], @"^\d+$")) {
												if (user.HasFloor(int.Parse(cmds[0][2]))) {
													System.IO.File.WriteAllText("exportedFloor.txt", $"{user.CurHouse.GetFloor(int.Parse(cmds[0][2])).Export(int.Parse(cmds[0][2]))}\n");
													Console.WriteLine($"\nFloor {cmds[0][2]} exported.\n");
													SetErrorLevel(0);
												}
												else WriteColor(new string[] { "floor", " must be greater than or equal to ", "0", " and less than ", user.CurHouse.Floors.Count.ToString(), ".\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
											}
											else WriteColor(new string[] { $"{cmds[0][2]} is not a valid ", "integer", ".\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
											break;
										case "-r":
											if (Regex.IsMatch(cmds[0][2], @"^(-1|\d+)$")) {
												if (int.Parse(cmds[0][2]) >= -1 && int.Parse(cmds[0][2]) < user.GetFloor().RoomNames.Count) {
													System.IO.File.WriteAllText("exportedRoom.txt", $"{user.GetFloor().Export(user.CurFloor, user.CurRoom)}\n");
													Console.WriteLine($"\nRoom {cmds[0][2]} exported.\n");
													SetErrorLevel(0);
												}
												else WriteColor(new string[] { "room", " must be greater than or equal to ", "-1", " and less than ", user.GetFloor().RoomNames.Count.ToString(), ".\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
											}
											else WriteColor(new string[] { $"{cmds[0][2]} is not a valid ", "integer", ".\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
											break;
										default:
											Console.Write($"{cmds[0][1]} is not a valid argument.\n");
											break;
									}
								}
								else Console.Write($"accepts 0 or 2 arguments.\n{SetErrorLevel(1)}");
							}
							else {
								string exportData = string.Empty;
								for (int i = 0; i < houseData.Count; i++)
									exportData += $"{houseData[i].Export(i)}\n";
								System.IO.File.WriteAllText(@"exportedItems.txt", exportData);
								Console.Write($"\nAll House Data {cmds[0][0].ToUpper()[0]}{cmds[0][0].Substring(1).ToLower()}{(cmds[0][0].Equals("export", StringComparison.OrdinalIgnoreCase) ? "e" : "")}d\n\n");
							}
							break;
						}
						case "goto": {
							if (cmds[0].Count > 1) {
								SetErrorLevel(2);
								if (Regex.IsMatch(cmds[0][1], @"^(-1|\d+|>|<)$")) {
									int room = cmds[0][1].Contains('<') ? user.CurRoom - 1 : cmds[0][1].Contains('>') ? user.CurRoom + 1 : int.Parse(cmds[0][1]);
									switch (user.GoRoom(room)) {
										case 1:
											Console.Write("Already in hallway (room -1), can't go lower. Room unchanged.\n");
											break;
										case 2:
											Console.Write("There aren't that many rooms on this floor.\n");
											break;
										case 3:
											Console.Write("Left the room.\n");
											enviVar[4, 1] = "-1";
											SetErrorLevel(0);
											break;
										case 0:
											Console.WriteLine($"Welcome to room {room}, \"{user.GetFloor().RoomNames[room]}\".");
											enviVar[4, 1] = room.ToString();
											SetErrorLevel(0);
											break;
										default:
											WriteColor(new ColorText("unexpected value returned by user.GoRoom, please report this.\n", ConsoleColor.Red));
											SetErrorLevel(4);
											break;
									}
								}
								else WriteColorLine(new string[] { "room", " must be a positive ", "integer", ", ", "-1", ", ", "<", ", or ", ">", "." }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White });
							}
							else {
								if (cmds.Count - 1 > timesExpanded) {
									Console.WriteLine("Please use the command's full syntax when in a script.");
									SetErrorLevel(3);
									break;
								}
								Console.WriteLine("Rooms on this floor:");
								for (int i = 0; i < user.RoomNames.Count; i++)
									Console.WriteLine($"{i.ToString()}: {user.RoomNames[i]}");
								int newRoom = GetInput(-1, user.RoomNames.Count);
								switch (user.GoRoom(newRoom)) {
									case 3:
										Console.WriteLine("Left the room(s).");
										break;
									case 0:
										Console.WriteLine($"Welcome to room {newRoom}, \"{user.GetFloor().RoomNames[newRoom]}\".");
										break;
								}
								enviVar[4, 1] = newRoom.ToString();
								SetErrorLevel(0);
							}
							break;
						}
						case "visit": {
							SetErrorLevel(1);
							if (cmds[0].Count > 1) {
								SetErrorLevel(2);
								if (Regex.IsMatch(cmds[0][1], @"^\d+$")) {
									int dst = int.Parse(cmds[0][1]);
									if (dst < houseData.Count) {
										user = viewers[dst];
										Console.WriteLine($"Welcome to House {dst}.");
										SetErrorLevel(0);
									}
									else Console.Write("There aren't that many Houses! (Remember: the first House is #0)\n");
								}
								else WriteColor(new string[] { "House number must be a positive ", "Integer", $", that is less than {houseData.Count}.\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
							}
							else Console.Write($"Visit which house? (There are {houseData.Count})\n");
							break;
						}
						case "use": {
							SetErrorLevel(1);
							if (cmds[0].Count > 1) {
								switch (cmds[0][1]) {
									case "light":
									case "lights":
										if (cmds[0].Count == 2) {
											WriteColor(new ColorText[] { user.GetFloor().ToggleLights(), new ColorText("\n") });
											SetErrorLevel(0);
										}
										else
											WriteColor(new string[] { $"{cmds[0][1]}, doesn't have extra arguments.\n" }, new ConsoleColor[] { ConsoleColor.White });
										break;
									default:
										WriteColorLine(new string[] { $"{cmds[0][1]} cannot be '", "used", "'." }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White });
										break;
								}
							}
							else WriteColor(new string[] { "Use", " what?\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
							break;
						}
						case "set": {
							bool validVar;
							switch (cmds[0].Count) {
								case 1:
									Console.Write("\n\tEnvironment Variables:\n\n");
									for (int v = 0; v < enviVar.GetLength(0); v++)
										Console.WriteLine($"{enviVar[v, 0]} = {enviVar[v, 1]}");
									Console.WriteLine();
									SetErrorLevel(3);
									break;
								case 2:
									validVar = false;
									for (int v = 0; v < enviVar.GetLength(0); v++) {
										if (enviVar[v, 0].Equals(cmds[0][1], StringComparison.OrdinalIgnoreCase)) {
											Console.Write($"\n{enviVar[v, 0]} = {enviVar[v, 1]}\n\n");
											validVar = true;
										}
									}
									if (!validVar)
										Console.Write($"{cmds[0][1]} is not a valid variable.\n");
									SetErrorLevel(validVar ? 3 : 2);
									break;
								case 3:
									validVar = false;
									int i;
									for (i = 0; i < enviVar.GetLength(0); i++) {
										if (enviVar[i, 0].Equals(cmds[0][1], StringComparison.OrdinalIgnoreCase)) {
											validVar = true;
											break;
										}
									}
									SetErrorLevel(2);
									if (validVar) {
										if (enviVar[i, 3].Equals("user")) {
											switch (enviVar[i, 2]) {
												case "bool":
													if (EqualsIgnoreCaseOr(cmds[0][2], new string[] { "false", "true", "0", "1" })) {
														enviVar[i, 1] = EqualsIgnoreCaseOr(cmds[0][2], new string[] { "true", "1" }) ? "true" : "false";
														SetErrorLevel(0);
													}
													else Console.Write($"{enviVar[i, 0]} stores a boolean value, must be true or false. (0 and 1 are acceptible)\n");
													break;
												case "int":
												case "double":
													if (Regex.IsMatch(cmds[0][2], @"^-?\d+(.\d+)?$")) {
														double newVal = double.Parse(cmds[0][2]);
														enviVar[i, 1] = (enviVar[i, 2].Equals("int") ? (int)newVal : newVal).ToString();
														SetErrorLevel(0);
													}
													else Console.Write($"{enviVar[i, 0]} stores a numeric value, must only contain a number, - is optional, if variable stores a double, you may provide a decimal value\n");
													break;
												default:
													Console.Write($"{enviVar[i, 0]} did not have a recognized value type, be cautious, and report this bug.\n");
													SetErrorLevel(4);
													goto case "string";
												case "string":
													enviVar[i, 1] = cmds[0][2];
													if (ErrorLevel != 4) SetErrorLevel(0);
													break;
											}
											Console.Write($"{enviVar[i, 0]} = {enviVar[i, 1]}\n");
										}
										else Console.Write($"{enviVar[i, 0]} is a system variable, and cannot be changed by the user.\n");
									}
									else Console.Write($"{cmds[0][1]} is not a valid variable.\n");
									break;
								default:
									Console.Write("Invalid amount of arguments.\n");
									SetErrorLevel(1);
									break;
							}
							break;
						}
						case "attach": {
							SetErrorLevel(1);
							if (cmds[0].Count > 1) {
								SetErrorLevel(2);
								if (Regex.IsMatch(cmds[0][1], @"^\d+$")) {
									SetErrorLevel(1);
									if (cmds[0].Count > 2) {
										SetErrorLevel(2);
										if (Regex.IsMatch(cmds[0][2], @"^\d+$")) {
											int src = Math.Abs(int.Parse(cmds[0][1]));
											int dst = Math.Abs(int.Parse(cmds[0][2]));
											if (cmds[0].Count > 3) {
												if (string.Equals(cmds[0][3], "-d", StringComparison.OrdinalIgnoreCase)) {
													IItem dst_i = user.GetItem(dst);
													switch (dst_i.Type) {
														case "Container":
															IItem tempItem = ((Container)dst_i).GetItem(src);
															if (!(tempItem is Empty)) {
																user.AddItem(tempItem);
																WriteColor(((Container)dst_i).RemoveItem(src));
																SetErrorLevel(0);
															}
															else WriteColor(new string[] { "The ", dst_i.SubType, " doesn't have that many ", "Items", " in it.\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White });
															break;
														case "Display":
															Console.WriteLine(((Display)dst_i).Disconnect(src));
															SetErrorLevel(0);
															break;
														default:
															WriteColor(new string[] { "That ", "Item", " cannot have things ", "detached", " from it.\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White });
															break;
													}
												}
												else WriteColor(new string[] { "Invalid argument, did you mean ", "-d", "?\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White });
											}
											else if (user.HasItem(src) && user.HasItem(dst)) {
												IItem src_i = user.GetItem(src);
												IItem dst_i = user.GetItem(dst);
												switch (dst_i.Type) {
													case "Container":
														if (CanGoInside(src_i is Clothing ? "Clothing" : src_i.SubType, dst_i.SubType)) {
															user.RemoveItem(src);
															WriteColor(new ColorText[] { ((Container)dst_i).AddItem(src_i), new ColorText("\n") });
															SetErrorLevel(0);
														}
														else Console.Write($"A {src_i.SubType}, cannot be put-in/attached-to a {dst_i.SubType}\n");
														break;
													case "Display":
														if (src_i is Computer || src_i is GameConsole) {
															WriteColor(new ColorText[] { new ColorText("\n"), ((Display)dst_i).Connect(src_i), new ColorText("\n") });
															SetErrorLevel(0);
														}
														else
															WriteColor(new string[] { "Item ", src.ToString(), " cannot connect to a ", "Display", ".\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
														break;
													default:
														WriteColor(new string[] { "Item", " cannot have things attached to it.\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White });
														break;
												}
											}
											else WriteColor(new string[] { "The floor only has ", user.FloorSize.ToString(), " items.\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
										}
										else WriteColor(new string[] { "Item", " must be an ", "integer", ".\n" }, new ConsoleColor[] { ConsoleColor.DarkRed, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
									}
									else WriteColor(new string[] { "Attach", " it to what?\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
								}
								else WriteColor(new string[] { "Item", " must be an ", "integer", ".\n" }, new ConsoleColor[] { ConsoleColor.DarkRed, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
							}
							else WriteColor(new string[] { "Attach", " what to what?\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
							break;
						}
						case "move": {
							SetErrorLevel(1);
							if (cmds[0].Count > 1) {
								SetErrorLevel(2);
								if (Regex.IsMatch(cmds[0][1], @"^\d+$")) {
									SetErrorLevel(1);
									if (cmds[0].Count > 2) {
										SetErrorLevel(2);
										if (Regex.IsMatch(cmds[0][2], @"^(\d+|<|>)(:(-(1|2)|\d+))?$")) {
											IItem old_item = user.curItem;
											int item = int.Parse(cmds[0][1]);
											int destinationFloor = cmds[0][2].Contains('<') ? user.CurFloor - 1 : cmds[0][2].Contains('>') ? user.CurFloor + 1 : cmds[0][2].Contains(':') ? int.Parse(cmds[0][2].Substring(0, cmds[0][2].IndexOf(':'))) : int.Parse(cmds[0][2]);
											int destinationRoom = cmds[0][2].Contains(':') ? int.Parse(cmds[0][2].Substring(cmds[0][2].IndexOf(':') + 1)) : -1;
											int oldFloor = user.CurFloor;
											int oldRoom = user.CurRoom;
											if (user.ChangeItemFocus(item)) {
												if (user.GoFloor(destinationFloor)) {
													int check = user.GoRoom(destinationRoom);
													if (check == 2 || check == 1) {
														Console.Write($"Room {destinationRoom} does not exist.\n");
														user.GoFloor(oldFloor);
														user.GoRoom(oldRoom);
														break;
													}
													IItem tempItem = user.curItem;
													user.AddItem(tempItem);
													user.GoFloor(oldFloor);
													user.GoRoom(oldRoom);
													user.RemoveItem(item);
													user.curItem = tempItem;
													WriteColor(new ColorText[] { new ColorText(new string[] { "This ", "Item ", "moved ", $"to floor {destinationFloor}, room {destinationRoom}\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.DarkBlue, ConsoleColor.White }), user.curItem.ToText(), new ColorText("\n\n") });
													SetErrorLevel(0);
												}
												else Console.Write("Floor does not exist.\n");
											}
											else WriteColor(new string[] { "Item", " does not exist.\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White });
											user.curItem = old_item;
										}
										else WriteColor(new string[] { "Floor must be an ", "integer", ", or: ", "<", " or ", ">", ", to change room, floor must be followed by ", ":", "room", ".\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.Cyan, ConsoleColor.White });
									}
									else WriteColor(new string[] { "Move", " it where?\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
								}
								else WriteColor(new string[] { "Item", " must be an ", "integer", ".\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
							}
							else WriteColor(new string[] { "Move", " what, and where?\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
							break;
						}
						case "grab":
						case "select": {
							SetErrorLevel(1);
							if (cmds[0].Count > 1) {
								SetErrorLevel(2);
								if (Regex.IsMatch(cmds[0][1], @"^\d+$")) {
									if (cmds[0].Count > 2) {
										if (Regex.IsMatch(cmds[0][2], @"^\d+$")) {
											switch (user.ChangeItemFocus(int.Parse(cmds[0][1]), int.Parse(cmds[0][2]))) {
												case 0:
													WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", "Item", " selected: (of type ", user.curItem.Type, ")\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }), user.curItem.ToText(), new ColorText("\n\n") });
													SetErrorLevel(0);
													break;
												case 1:
													WriteColor(new string[] { "Either ", "Item ", cmds[0][1], " doesn't have any ", "sub-Items", ", or the ", "integer", " you entered is too high.\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
													break;
												case 2:
													WriteColor(new string[] { $"\"{cmds[0][1]}\" is invalid, must be less than the floor ", "Item", " size of: ", $"{user.FloorSize}\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan });
													break;
												default:
													WriteColor(new ColorText("unexpected value returned by get sub-item, please report this.\n", ConsoleColor.Red));
													SetErrorLevel(4);
													break;
											}
										}
										else WriteColor(new string[] { $"\"{cmds[0][2]}\" is not a valid ", "integer\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan });
									}
									else if (user.ChangeItemFocus(int.Parse(cmds[0][1])))
										WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", "Item", " selected: (of type ", user.curItem.Type, $")\n\n{SetErrorLevel(0)}" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }), user.curItem.ToText(), new ColorText("\n\n") });
									else
										WriteColor(new string[] { $"\"{cmds[0][1]}\" is invalid, must be less than the floor ", "Item", " size of: ", $"{user.FloorSize}\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan });
								}
								else WriteColor(new string[] { $"\"{cmds[0][1]}\" is not a valid ", "integer\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan });
							}
							else WriteColor(new string[] { "Grab", " what?\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
							break;
						}
						case "remove": {
							SetErrorLevel(1);
							if (cmds[0].Count > 1) {
								SetErrorLevel(2);
								if (Regex.IsMatch(cmds[0][1], @"^\d+$")) {
									IItem tempItem = user.curItem;
									bool validAnswer = false;
									bool validItem = false;
									bool subItem = false;
									SetErrorLevel(1);
									if (cmds[0].Count > 2) {
										subItem = true;
										SetErrorLevel(2);
										if (EqualsIgnoreCaseOr(cmds[0][2], new string[] { "-h", "--house" })) {
											if (int.Parse(cmds[0][1]) < houseData.Count) {
												bool delete = false;
												if (cmds.Count - 1 > timesExpanded)
													delete = true;
												else {
													try {
														delete = GetInput(new ColorText("Are you sure you want to remove this House, and all of its contents? [Y/N] ", ConsoleColor.Red), new string[] { "y", "n" }, true).ToLower().Equals("y");
													}
													catch (ArrayTooSmall e) { Console.Write(e.StackTrace); }
												}
												if (delete) {
													houseData.RemoveAt(int.Parse(cmds[0][1]));
													viewers.RemoveAt(int.Parse(cmds[0][1]));
													Console.Write($" House {cmds[0][1]} removed.\n");
													SetErrorLevel(3);
													if (cmds[0].Count > 3) {
														SetErrorLevel(2);
														if (Regex.IsMatch(cmds[0][1], @"^\d+$")) {
															user = viewers[int.Parse(cmds[0][3])];
															SetErrorLevel(0);
														}
													}
													else {
														if (!houseData.Contains(user.CurHouse)) {
															Console.Write("\nPlease select a pre-existing House to go to now.\n");
															user = viewers[GetInput(0, houseData.Count)];
														}
													}
												}
												else Console.Write("Action aborted.\n");
											}
											else Console.WriteLine("There aren't that many Houses! (Remember: the first House is #0)");
										}
										else if (EqualsIgnoreCaseOr(cmds[0][2], new string[] { "-f", "--floor" })) {
											if (int.Parse(cmds[0][1]) < user.CurHouse.Size) {
												bool delete = false;
												if (cmds.Count - 1 > timesExpanded)
													delete = true;
												else {
													try {
														delete = GetInput(new ColorText("Are you sure you want to remove this Floor, and all of its contents? [Y/N] ", ConsoleColor.Red), new string[] { "y", "n" }, true).ToLower().Equals("y");
													}
													catch (ArrayTooSmall e) { Console.Write(e.StackTrace); }
												}
												if (delete) {
													user.CurHouse.RemoveFloor(int.Parse(cmds[0][1]));
													Console.Write($" Floor {cmds[0][1]} removed.\n");
													if (user.CurFloor >= int.Parse(cmds[0][1]))
														user.GoDown();
													if (user.CurHouse.Size == 0) {
														SetErrorLevel(3);
														Console.Write("\nHouse now has 0 floors, [R]emove House, or [A]dd new floor?\n");
														string action = string.Empty;
														if (cmds[0].Count > 3) {
															SetErrorLevel(0);
															action = cmds[0][3] switch
															{
																"-r" => "r",
																"-a" => "a",
																_ => SetErrorLevel(2)
															};
															if (ErrorLevel == 2)
																//WriteColor(new string[] { "remove: ", $" {cmds[0][3]} is not a valid argument.\n" });
																Console.Write($"{cmds[0][3]} is not a valid argument.\n");
														}
														else {
															try {
																action = GetInput(new ColorText("[R/A] "), new string[] { "r", "a" }, true).ToLower();
															}
															catch (ArrayTooSmall e) { Console.Write(e.StackTrace); }
														}
														if (ErrorLevel != 0)
															break;
														if (action.Equals("r")) {
															viewers.RemoveAt(houseData.IndexOf(user.CurHouse));
															houseData.Remove(user.CurHouse);
															WriteColor(new string[] { "remove:", " House removed.\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
															if (cmds.Count - 1 > timesExpanded) {
																user = viewers[0];
															}
															else {
																Console.WriteLine("\nPlease select a pre-existing House to go to now.\n");
																user = viewers[GetInput(0, houseData.Count)];
															}
														}
														else user.CurHouse.AddFloor();
													}
												}
												else Console.Write("Action aborted.\n");
											}
											else Console.WriteLine("There aren't that many Floors! (Remember: the first Floor is #0)");
										}
										else if (EqualsIgnoreCaseOr(cmds[0][2], new string[] { "-r", "--room" })) {
											if (int.Parse(cmds[0][1]) < user.GetFloor().RoomNames.Count) {
												bool delete = false;
												try {
													delete = GetInput(new ColorText("[Y] Remove all contents in room. [N] Move all contents in room to hall. ", ConsoleColor.Red), new string[] { "y", "n" }, true).ToLower().Equals("y");
												}
												catch (ArrayTooSmall e) { Console.Write(e.StackTrace); }
												Console.Write(user.GetFloor().RemoveRoom(int.Parse(cmds[0][1]), delete));
												if (cmds[0][1].Equals(enviVar[4, 1])) {
													enviVar[4, 1] = "-1";
													user.GoRoom(-1);
												}
											}
											else Console.WriteLine("There aren't that many Rooms! (Remember: the first Room is #0)");
										}
										else if (Regex.IsMatch(cmds[0][2], @"^\d+$")) {
											switch (user.ChangeItemFocus(int.Parse(cmds[0][1]), int.Parse(cmds[0][2]))) {
												case 0:
													validItem = true;
													break;
												case 1:
													WriteColor(new string[] { "This ", "Item", " either has no ", "sub-Items", " on it, or the ", "integer", " is too high\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
													break;
												case 2:
													WriteColor(new string[] { "This floor only has ", user.FloorSize.ToString(), " items on it\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
													break;
											}
										}
										else WriteColor(new string[] { $"\"{cmds[0][2]}\" is not a valid ", "integer\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan });
									}
									else validItem = user.ChangeItemFocus(int.Parse(cmds[0][1]));
									if (validItem) {
										while (!validAnswer) {
											WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", "Item", " is:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }), user.curItem.ToText() });
											user.curItem = tempItem;
											string yenu = string.Empty;
											try {
												yenu = GetInput(new ColorText("Are you sure you want to delete this? [Y/N] ", ConsoleColor.Red), new string[] { "y", "n" }, true);
											}
											catch (ArrayTooSmall e) { Console.Write(e.StackTrace); }
											Console.WriteLine();
											switch (yenu.ToUpper()) {
												case "Y":
													if (subItem)
														user.RemoveItem(int.Parse(cmds[0][1]), int.Parse(cmds[0][2]));
													else
														user.RemoveItem(int.Parse(cmds[0][1]));
													goto case "N";
												case "N":
													validAnswer = true;
													break;
											}
										}
									}
									else if (!subItem)
										WriteColor(new string[] { "This floor only has ", user.FloorSize.ToString(), " items on it\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
								}
								else WriteColor(new string[] { $"\"{cmds[0][1]}\" is not a valid ", "integer\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan });
							}
							else WriteColor(new string[] { "\nRemove", " what?\n\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
							break;
						}
						case "list":
						case "look": {
							int searchRoom = enviVar[3, 1].Equals("true") ? int.Parse(enviVar[4, 1]) : -2;
							if (cmds[0].Count > 1) {
								if (EqualsIgnoreCaseOr(cmds[0][1], new string[] { "--house", "-h" })) {
									House infoHouse = user.CurHouse;
									if (cmds[0].Count > 2) {
										if (EqualsIgnoreCaseOr(cmds[0][2], new string[] { "--address", "-a" })) {
											Console.Write("What quadrant is the House in? (0:NE, 1:NW, 2:SW, 3:SE)");
											int quad = GetInput(0, 4);
											bool street = GetInput(new ColorText("Is this house on a Street, or Avenue [st/ave] "), new string[] { "st", "ave" }, true).Equals("st", StringComparison.OrdinalIgnoreCase);
											Console.Write($"What {(street ? "street" : "avenue")} is it on?\n");
											int conRoad = GetInput(1, 301) - 1;
											Console.Write("Enter the 3-5 digit House number:\n");
											int rawAdd = GetInput(100, 29920);
											int adjRoad = rawAdd / 100 - 1;
											bool found = false;
											for (int h = 0; h < maxHouses; h++) {
												infoHouse = localMap[quad, (street ? conRoad : adjRoad), (street ? adjRoad : conRoad)][h];
												if (infoHouse.HouseNumber == rawAdd % 100) {
													found = true;
													break;
												}
											}
											if (!found) {
												Console.Write("House not found at specified address.\n");
												break;
											}
										}
										else {
											if (Regex.IsMatch(cmds[0][2], @"^\d+$")) {
												int h = int.Parse(cmds[0][2]);
												if (h >= houseData.Count) {
													Console.WriteLine("There aren't that many Houses! (Remember: the first House is #0)");
													break;
												}
												infoHouse = houseData[int.Parse(cmds[0][2])];
											}
											else {
												Console.Write($"{cmds[0][2]} is not a valid argument.\n");
												break;
											}
										}
									}
									WriteColor(new string[] { "House ", $"{houseData.IndexOf(infoHouse)}", $":\n\tColor: {infoHouse.GetColor}\n\tSize: ", (infoHouse.Size).ToString(), $" floors\n\tAddress: {infoHouse.Address}\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
								}
								else if (EqualsIgnoreCaseOr(cmds[0][1], new string[] { "--hand", "--focus", "-h", "-f" }))
									WriteColor(new ColorText[] { new ColorText("\n"), user.ViewCurItem(), new ColorText("\n\n") });
								else if (cmds[0].Count > 2 || EqualsIgnoreCaseOr(cmds[0][1], new string[] { "-p", "--page" })) {
									bool page = false;
									int rangeStart = 0;
									int rangeEnd = user.FloorSize;
									string searchType = "*";
									int invalidArg = 0;
									for (int i = 1; i < cmds[0].Count; i++) {
										if (EqualsIgnoreCaseOr(cmds[0][i], new string[] { "-x", "--room" }) && (i + 1 < cmds[0].Count)) {
											if (Regex.IsMatch(cmds[0][i + 1], @"^(-(1|2)|\d+)$")) {
												searchRoom = int.Parse(cmds[0][i + 1]);
												i++;
												continue;
											}
											invalidArg = i + 1;
											continue;
										}
										if (EqualsIgnoreCaseOr(cmds[0][i], new string[] { "-i", "--item" }) && (i + 1 < cmds[0].Count)) {
											searchType = cmds[0][i + 1];
											i++;
											continue;
										}
										if (EqualsIgnoreCaseOr(cmds[0][i], new string[] { "-r", "--range" }) && (i + 2 < cmds[0].Count)) {
											if (Regex.IsMatch(cmds[0][i + 1], @"^\d+$")) {
												if (Regex.IsMatch(cmds[0][i + 2], @"^\d+$")) {
													rangeStart = int.Parse(cmds[0][i + 1]);
													rangeEnd = int.Parse(cmds[0][i + 2]);
													i += 2;
													continue;
												}
												invalidArg = i + 2;
												continue;
											}
											invalidArg = i + 1;
											continue;
										}
										if (EqualsIgnoreCaseOr(cmds[0][i], new string[] { "-p", "--page" })) {
											page = true;
											continue;
										}
										invalidArg = i;
									}
									if (invalidArg == 0) {
										if (enviVar[0, 1].Equals("false") || user.GetFloor().Lights) {
											if (page) {
												int pageCount = user.PageCount(rangeStart, rangeEnd, searchType, 20, searchRoom);
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
													case -5:
														Console.Write("Room does not exist.\n");
														break;
													case -6:
														Console.Write("Floor does not exist.\n");
														break;
													default:
														for (int i = 0; i < pageCount; i++) {
															WriteColorLine(new string[] { "\n\tFloor ", "Listing", " - Page ", (i + 1).ToString() }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.DarkBlue, ConsoleColor.Cyan });
															WriteColor(user.List(rangeStart, rangeEnd, searchType, 20, i, searchRoom));
															if (i + 1 < pageCount) {
																Console.Write("Press enter to continue > ");
																Console.ReadLine();
															}
															else Console.WriteLine();
														}
														break;
												}
											}
											else WriteColor(new ColorText[] { user.List(rangeStart, rangeEnd, searchType, user.FloorSize, 0, searchRoom), new ColorText("\n") });
										}
										else Console.Write("\nYou can't see anything, the floor is completely dark!\n\n");
									}
									else Console.Write($"{cmds[0][invalidArg]} is not a valid argument.\n");
								}
								else if (Regex.IsMatch(cmds[0][1], @"^\d+$")) {
									if (int.Parse(cmds[0][1]) < user.FloorSize) {
										IItem tempItem = user.curItem;
										user.ChangeItemFocus(int.Parse(cmds[0][1]));
										switch (user.curItem.Type) {
											case "Container": {
												WriteColor(new string[] { "This ", "Item", " is a ", user.curItem.SubType, ", would you like to see:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												WriteColor(new string[] { "(Y) A specific ", "Item", "\n(N) Just the overall contents\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												bool yenu = false;
												try {
													yenu = GetInput(new ColorText("[Y/N] "), new string[] { "y", "n" }, true).ToLower().Equals("y");
												}
												catch (ArrayTooSmall e) { Console.Write(e.StackTrace); }
												int iC = ((Container)user.curItem).Size;
												if (yenu && iC > 0) {
													WriteColor(new string[] { "\nWhich ", "Item", ":\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
													int im = GetInput(0, iC);
													WriteColor(new ColorText[] { new ColorText("\n"), ((Container)user.curItem).GetItem(im).ToText() });
												}
												else if (!yenu && iC > 0)
													WriteColor(new ColorText[] { new ColorText("\n"), user.ViewCurItem() });
												else
													WriteColor(new string[] { $"{user.curItem.SubType}{(string.IsNullOrEmpty(user.curItem.Name) ? string.Empty : $" {user.curItem.Name}")}", " is empty." }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White });
												Console.WriteLine();
												Console.WriteLine();
												break;
											}
											case "Display": {
												WriteColor(new string[] { "This ", "Item", " is a ", "Display", ", would you like to see:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												WriteColor(new string[] { "(Y) A specific device\n(N) Just the ", "Display\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow });
												bool yenu = false;
												try {
													yenu = GetInput(new ColorText("[Y/N] "), new string[] { "y", "n" }, true).ToLower().Equals("y");
												}
												catch (ArrayTooSmall e) { Console.Write(e.StackTrace); }
												if (yenu && ((Display)user.curItem).DeviceCount > 0) {
													Console.Write("\nWhich device:\n\n");
													while (true) {
														int dv = GetInput(0, ((Display)user.curItem).DeviceCount);
														if (dv < ((Display)user.curItem).DeviceCount) {
															WriteColor(new ColorText[] { new ColorText("\n"), ((Display)user.curItem).GetDevice(dv).ToText() });
															break;
														}
														else WriteColor(new string[] { user.curItem.SubType, " doesn't have that many ", "Items" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.Yellow, ConsoleColor.Yellow });
													}
												}
												else if (!yenu || ((Display)user.curItem).DeviceCount == 0)
													WriteColor(new ColorText[] { new ColorText("\n"), user.ViewCurItem() });
												Console.WriteLine();
												Console.WriteLine();
												break;
											}
											case "Book":
											case "Computer":
											case "Console":
											case "Bed":
											case "Clothing":
											case "Printer":
												WriteColor(new ColorText[] { new ColorText("\n"), user.ViewCurItem(), new ColorText("\n\n") });
												break;
										}
										user.curItem = tempItem;
									}
									else WriteColor(new string[] { "This floor only has ", user.FloorSize.ToString(), " Items", " on it\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.White });
								}
								else WriteColor(new string[] { $"\"{cmds[0][1]}\" is not a valid ", "integer\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan });
							}
							else if (enviVar[0, 1].Equals("false") || user.GetFloor().Lights)
								WriteColor(new ColorText[] { user.List(0, user.FloorSize, "*", user.FloorSize, 0, searchRoom), new ColorText("\n") });
							else
								Console.Write("\nYou can't see anything, the floor is completely dark!\n\n");
							break;
						}
						case "add": {
							if (cmds[0].Count > 1) {
								user.GoRoom(int.Parse(enviVar[4, 1]));
								switch (cmds[0][1].ToLower()) {
									case "house":
										if (cmds[0].Count > 2) {
											if (cmds[0][2].Equals("arg", StringComparison.OrdinalIgnoreCase)) {
												int color;
												Console.WriteLine("\nChoose from these colors:\n");
												for (int i = 0; i < House.colors.Length; i++)
													WriteColor(new string[] { i.ToString(), $": {House.colors[i]} " }, new ConsoleColor[] { ConsoleColor.Cyan, ConsoleColor.White });
												Console.WriteLine();
												color = GetInput(0, House.colors.Length);
												int floors;
												Console.Write("\nHow many floors will this house have?\n");
												floors = GetInput(1, 101); //Kinda just had to pick an arbitrary number here
												House newHouse = new House(color, floors, true, -1, -1, -1, -1);
												houseData.Add(newHouse);
												viewers.Add(new Viewer(newHouse));
												Console.WriteLine($"\n{floors} story, {House.colors[color]} House, number {(houseData.Count - 1).ToString()} added.\n");
											}
											else WriteColor(new string[] { "\nInvalid 2nd argument, did you mean ", "arg", "?\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White });
										}
										else {
											House newHouse = new House();
											houseData.Add(newHouse);
											viewers.Add(new Viewer(newHouse));
											Console.WriteLine($"\nHouse {houseData.Count - 1} added.\n");
										}
										break;
									case "floor":
										if (cmds[0].Count > 2) {
											if (cmds[0][2].Equals("arg", StringComparison.OrdinalIgnoreCase)) {
												int rooms = 0;
												Console.WriteLine("\nHow many Rooms will be on this floor (1 or more):\n(You will have to enter a name for EACH room, or type -c to abort the process.)\n");
												do {
													Console.Write("\n> ");
													string rm = Console.ReadLine();
													if (Regex.IsMatch(rm, @"^\d+$"))
														rooms = int.Parse(rm);
												} while (rooms < 1);
												List<string> roomNames = new List<string>();
												for (int i = 0; i < rooms; i++) {
													Console.Write("\n> ");
													string name = Console.ReadLine();
													if (name.Equals("-c")) {
														Console.WriteLine("\nProcess aborted, floor not added.\n");
														break;
													}
													roomNames.Add(name);
												}
												user.CurHouse.AddFloor(new Floor(new List<IItem>(), new List<int>(), false, roomNames));
											}
											else WriteColor(new string[] { "\nInvalid 2nd argument, did you mean ", "arg", "?\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White });
										}
										else {
											user.CurHouse.AddFloor();
											Console.WriteLine($"\nFloor {user.CurHouse.Size - 1} added.\n");
										}
										break;
									case "room":
										Console.WriteLine("\nEnter room name:\n");
										string room = Console.ReadLine();
										user.GetFloor().AddRoom(room);
										break;
									case "book":
										Book tempBook = new Book();
										if (cmds[0].Count > 2) {
											if (string.Equals(cmds[0][2], "arg", StringComparison.OrdinalIgnoreCase)) {
												WriteColor(new string[] { "\nEnter ", "Book", " Title > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												string title = Console.ReadLine();
												WriteColor(new string[] { "\nEnter ", "Book", " Author > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												string author = Console.ReadLine();
												Console.Write("\nEnter Publishing Year > ");
												string year = Console.ReadLine();
												tempBook.Reset(title, author, Regex.IsMatch(year, @"^\d+") ? int.Parse(year) : 0); // I could use ^(1([6-9]|\d{2,})|[2-9]\d{1,})\d{2,}$ but I'll just let the Reset method take care of it...
												WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", "Book", " added:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }), tempBook.ToText(), new ColorText("\n\n") });
											}
											else WriteColor(new string[] { "\nInvalid 2nd argument, did you mean ", "arg", "?\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White });
										}
										else WriteColor(new string[] { "\nNew ", "Book", " added to floor ", user.CurFloor.ToString(), ".\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
										user.AddItem(tempBook);
										break;
									case "computer":
										Computer tempComp = new Computer();
										if (cmds[0].Count > 2) {
											if (string.Equals(cmds[0][2], "arg", StringComparison.OrdinalIgnoreCase)) {
												WriteColor(new string[] { "\nWhat kind of ", "Computer", " is it? (Desktop, Laptop, etc) > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												string computer = Console.ReadLine();
												WriteColor(new string[] { "\nComputer", " Brand (ie: HP, Microsoft) > " }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White });
												string brand = Console.ReadLine();
												WriteColor(new string[] { "Computer", " Family (ie: Pavilion, Surface) > " }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White });
												string family = Console.ReadLine();
												WriteColor(new string[] { "Computer", " Model (ie: dv6, Pro 3) > " }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White });
												string model = Console.ReadLine();
												Console.Write("\nIs the computer turned on?");
												bool isOn = false;
												try {
													isOn = GetInput(new ColorText("[Y/N] "), new string[] { "y", "n" }, true).ToLower().Equals("y");
												}
												catch (ArrayTooSmall e) { Console.Write(e.StackTrace); }
												WriteColor(new string[] { "Computer", " name > " }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White });
												string name = Console.ReadLine();
												tempComp.Reset(brand, family, model, isOn, computer, name);
												WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", "Computer", " added:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }), tempComp.ToText(), new ColorText("\n\n") });
											}
											else WriteColor(new string[] { "\nInvalid 2nd argument, did you mean ", "arg", "?\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White });
										}
										else WriteColor(new string[] { "\nNew ", "Computer", " added to floor ", user.CurFloor.ToString(), ".\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
										user.AddItem(tempComp);
										break;
									case "console":
										GameConsole tempConsole = new GameConsole();
										if (cmds[0].Count > 2) {
											if (string.Equals(cmds[0][2], "arg", StringComparison.OrdinalIgnoreCase)) {
												for (int i = 0; i < GameConsole.types.Length; i++)
													WriteColor(new string[] { i.ToString(), $": {GameConsole.types[i]} " }, new ConsoleColor[] { ConsoleColor.Cyan, ConsoleColor.White });
												Console.WriteLine();
												WriteColor(new string[] { "\nEnter ", "Console", " Type > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												int tempType = GetInput(0, GameConsole.types.Length);
												WriteColor(new string[] { "\nEnter ", "Console", " Manufacturer (ie Nintendo) > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												string com = Console.ReadLine();
												WriteColor(new string[] { "\nEnter ", "Console", " Name (ie GameCube) > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												string sys = Console.ReadLine();
												WriteColor(new string[] { "\nEnter ", "Console", " name (personalized name for the ", "Item", ") > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												string name = Console.ReadLine();
												tempConsole = new GameConsole(tempType, com, sys, name);
												WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", "Console", " added:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }), tempConsole.ToText(), new ColorText("\n\n") });
											}
											else WriteColor(new string[] { "\nInvalid 2nd argument, did you mean ", "arg", "?\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White });
										}
										else WriteColor(new string[] { "\nNew ", "Console", " added to floor ", user.CurFloor.ToString(), ".\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
										user.AddItem(tempConsole);
										break;
									case "display":
										Display tempDisp = new Display();
										if (cmds[0].Count > 2) {
											if (string.Equals(cmds[0][2], "arg", StringComparison.OrdinalIgnoreCase)) {
												Console.Write("\nIs it a Monitor (Y) or a TV (N)?");
												bool isMon = false;
												try {
													isMon = GetInput(new ColorText("[Y/N] "), new string[] { "y", "n" }, true).ToLower().Equals("y");
												}
												catch (ArrayTooSmall e) { Console.Write(e.StackTrace); }
												WriteColor(new string[] { "\nType the number for each device connected to this ", "Display", " seperated by a space.\n(Optional)\n> " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												string[] conDevs = Regex.Split(Console.ReadLine(), " +");
												List<int> added = new List<int>();
												List<int> notAdded = new List<int>();
												List<string> notNumber = new List<string>();
												foreach (string dev in conDevs) {
													if (Regex.IsMatch(dev, @"^\d+$")) {
														int devID = int.Parse(dev);
														if (devID >= 0 && devID < user.FloorSize && CanGoInside(user.GetItem(devID).Type, "Display"))
															added.Add(devID);
														else
															notAdded.Add(devID);
													}
													else notNumber.Add(dev);
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
												WriteColor(new string[] { "\n\nEnter the ", "Display's", " size in inches (decimals allowed)" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White });
												double size = GetInput(0.0, 100.0);
												WriteColor(new string[] { "\nEnter the ", "Display's", " name > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White });
												string name = Console.ReadLine();
												tempDisp = new Display(isMon, new List<IItem>(), size, name);
												foreach (int id in added)
													tempDisp.Connect(user.GetItem(id));
												WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", "Display", " added:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }), tempDisp.ToText(), new ColorText("\n\n") });
											}
											else WriteColor(new string[] { "\nInvalid 2nd argument, did you mean ", "arg", "?\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White });
										}
										else WriteColor(new string[] { "\nNew ", "Display", " added to floor ", user.CurFloor.ToString(), ".\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
										user.AddItem(tempDisp);
										break;
									case "bed":
										Bed tempBed = new Bed();
										if (cmds[0].Count > 2) {
											if (cmds[0][2].Equals("arg", StringComparison.OrdinalIgnoreCase)) {
												WriteColor(new string[] { "\nIs this ", "Bed", " adjustable? (Invalid input will default to N)\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												bool canMove = false;
												try {
													canMove = GetInput(new ColorText("[Y/N] "), new string[] { "y", "n" }, true).ToLower().Equals("y");
												}
												catch (ArrayTooSmall e) { Console.Write(e.StackTrace); }
												WriteColor(new string[] { "\nEnter ", "Bed", $" type: {Bed.types[0]}" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												for (int i = 1; i < Bed.types.Length; i++)
													Console.Write($", {Bed.types[i]}");
												Console.WriteLine();
												int bedType = GetInput(0, Bed.types.Length);
												WriteColor(new string[] { "\nEnter the ", "Bed's", " name > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White });
												string name = Console.ReadLine();
												tempBed = new Bed(canMove, bedType, name);
												WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", "Bed", " added:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }), tempBed.ToText(), new ColorText("\n\n") });
											}
											else WriteColor(new string[] { "\nInvalid 2nd argument, did you mean ", "arg", "?\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White });
										}
										else WriteColor(new string[] { "\nNew ", "Bed", " added to floor ", user.CurFloor.ToString(), ".\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
										user.AddItem(tempBed);
										break;
									case "container":
										WriteColor(new string[] { "\nEnter the ", "Container", " sub-type:\n\tie: Container, Bookshelf, Fridge, etc. (Defaults to Container)\n\n> " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
										string type = Console.ReadLine();
										IItem tempCon = Container.Create(type);
										if (cmds[0].Count > 2) {
											if (cmds[0][2].Equals("arg", StringComparison.OrdinalIgnoreCase)) {
												WriteColor(new string[] { "\nType the number for each ", "Item", " to be put inside this ", tempCon.SubType, " seperated by a space.\n(Optional)\n> " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												string[] objs = Regex.Split(Console.ReadLine(), " +");
												Console.WriteLine();
												List<int> added = new List<int>();
												List<int> notAdded = new List<int>();
												List<string> notNumber = new List<string>();
												foreach (string itm in objs) {
													if (Regex.IsMatch(itm, @"^\d+$")) {
														int itmID = int.Parse(itm);
														if (itmID >= 0 && itmID < user.FloorSize)
															added.Add(itmID);
														else
															notAdded.Add(itmID);
													}
													else notNumber.Add(itm);
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
												WriteColor(new string[] { "\nEnter the ", "Container's", " name > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White });
												tempCon.Rename(Console.ReadLine());
												WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", tempCon.SubType, " created:\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.Yellow, ConsoleColor.White }), tempCon.ToText(), new ColorText("\n\n") });
											}
											else WriteColor(new string[] { "\nInvalid 2nd argument, did you mean ", "arg", "?\n\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.White });
										}
										else WriteColor(new string[] { "\nNew ", tempCon.SubType, " added to floor ", user.CurFloor.ToString(), ".\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
										user.AddItem(tempCon);
										break;
									case "clothing":
										WriteColor(new string[] { "\nEnter the ", "Clothing", " sub-type:\n\tie: Clothing, Shirt, Pants, etc. (Defaults to Clothing)\n\n> " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
										string clothType = Console.ReadLine();
										IItem tempCloth = Clothing.Create(clothType);
										if (cmds[0].Count > 2) {
											if (cmds[0][2].Equals("arg", StringComparison.OrdinalIgnoreCase)) {
												Console.Write("\nEnter Clothing color > ");
												((Clothing)tempCloth).Color = Console.ReadLine();
												Console.Write("\nEnter Clothing name > ");
												tempCloth.Rename(Console.ReadLine());
												WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", tempCloth.SubType, " created:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.Yellow }), tempCloth.ToText(), new ColorText("\n\n") });
											}
											else WriteColor(new string[] { "\nInvalid 2nd argument, did you mean ", "arg", "?\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White });
										}
										else WriteColor(new string[] { "\nNew ", tempCloth.SubType, " added to floor ", user.CurFloor.ToString(), ".\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
										user.AddItem(tempCloth);
										break;
									case "printer":
										Printer tempPrint = new Printer();
										if (cmds[0].Count > 2) {
											if (cmds[0][2].Equals("arg", StringComparison.OrdinalIgnoreCase)) {
												WriteColor(new string[] { "\nEnter the ", "Printer", " details.\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												WriteColor(new string[] { "\nDoes the ", "Printer", " print in color?\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												bool hasColor = GetInput(new ColorText("[Y/N] "), new string[] { "y", "n" }, true).Equals("Y", StringComparison.OrdinalIgnoreCase);
												WriteColor(new string[] { "\nCan the ", "Printer", " send and receive faxes?\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												bool canFax = GetInput(new ColorText("[Y/N] "), new string[] { "y", "n" }, true).Equals("Y", StringComparison.OrdinalIgnoreCase);
												WriteColor(new string[] { "\nCan the ", "Printer", " scan things?\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
												bool canScan = GetInput(new ColorText("[Y/N] "), new string[] { "y", "n" }, true).Equals("Y", StringComparison.OrdinalIgnoreCase);
												WriteColor(new string[] { "\nEnter the ", "Printer's", " name > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White });
												string name = Console.ReadLine();
												tempPrint = new Printer(canFax, canScan, hasColor, name);
												WriteColor(new ColorText[] { new ColorText(new string[] { "\nThis ", "Printer", " added:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }), tempPrint.ToText(), new ColorText("\n\n") });
											}
											else WriteColor(new string[] { "\nInvalid 2nd argument, did you mean ", "arg", "?\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White });
										}
										else WriteColor(new string[] { "\nNew ", "Printer", " added to floor ", user.CurFloor.ToString(), ".\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
										user.AddItem(tempPrint);
										break;
									default:
										WriteColor(new string[] { $"\"{cmds[0][1]}\" is not a valid ", "Item", " type:\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
										for (int i = 0; i < cmds[0].Count; i++)
											Console.Write(cmds[0][i] + " ");
										Console.WriteLine();
										Help("add");
										break;
								}
							}
							else Console.Write("\nInvalid syntax, requires at least one argument\n\n");
							break;
						}
						case "status":
						case "info": {
							user.GoRoom(int.Parse(enviVar[4, 1]));
							enviVar[2, 1] = houseData.IndexOf(user.CurHouse).ToString();
							Console.WriteLine($"\n{user}\n");
							break;
						}
						case ">":
						case "up": {
							switch (cmds[0].Count) {
								case 1:
									Console.WriteLine(user.GoUp());
									break;
								case 2:
									if (Regex.IsMatch(cmds[0][1], @"^\d+$")) {
										if (user.GoFloor(user.CurFloor + int.Parse(cmds[0][1]))) {
											WriteColor(new string[] { "\nWelcome to floor ", user.CurFloor.ToString(), ".\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
											enviVar[4, 1] = "-1";
										}
										else Console.WriteLine("\nYou are currently on the top floor, floor unchanged.\n");
									}
									else WriteColorLine(new string[] { "Argument must be a positive ", "integer", "." }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
									break;
								default:
									Console.WriteLine($"{cmds[0][0]} only accepts 1 argument.");
									break;
							}
							break;
						}
						case "<":
						case "down": {
							switch (cmds[0].Count) {
								case 1:
									Console.WriteLine(user.GoDown());
									break;
								case 2:
									if (Regex.IsMatch(cmds[0][1], @"^\d+$")) {
										if (user.GoFloor(user.CurFloor - int.Parse(cmds[0][1]))) {
											WriteColor(new string[] { "\nWelcome to floor ", user.CurFloor.ToString(), ".\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
											enviVar[4, 1] = "-1";
										}
										else Console.WriteLine("\nYou are currently on the bottom floor, floor unchanged.\n");
									}
									else WriteColorLine(new string[] { "Argument must be a positive ", "integer", "." }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
									break;
								default:
									Console.WriteLine($"{cmds[0][0]} only accepts 1 argument.");
									break;
							}
							break;
						}
						case "help": {
							if (cmds[0].Count > 1) {
								if (cmds[0].Count > 2) {
									switch (cmds[0][1].ToLower()) {
										case "add":
										case "list":
										case "look":
										case "find":
										case "search":
											if (cmds[0][2].Equals("item", StringComparison.OrdinalIgnoreCase))
												WriteColor(Help("top-item"));
											else
												WriteColor(Help("bad-sub"));
											break;
										case "use":
											if (cmds[0][2].Equals("object", StringComparison.OrdinalIgnoreCase))
												WriteColor(Help("floor-interact"));
											else
												WriteColor(Help("bad-sub"));
											break;
										default:
											WriteColor(Help("."));
											break;
									}
								}
								else WriteColor(Help(cmds[0][1].ToLower()));
							}
							else {
								Console.WriteLine();
								WriteColor(new string[] { "          add", " - adds an ", "Item", " to the current floor\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
								WriteColor(new string[] { "       attach", " - attaches (or detaches) one ", "Item", " to (from) another\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
								WriteColor(new string[] { "  clear / cls", " - clears the screen\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
								WriteColor(new string[] { "         down", " - goes down 1 floor\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
								WriteColor(new string[] { "  exit / quit", " - stops the program\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
								WriteColor(new string[] { "find / search", " - looks through House for ", "Items", " matching keywords\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
								WriteColor(new string[] { "grab / select", " - sets which ", "Item", " you are currently focused on\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
								WriteColor(new string[] { "         help", " - displays this screen, and others\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
								WriteColor(new string[] { "info / status", " - shows information about you and the ", "House", " you are currently in\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
								WriteColor(new string[] { "  list / look", " - shows the ", "Items", " on the current floor, or shows info about a\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
								WriteColor(new string[] { "                specific ", "Item\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow });
								WriteColor(new string[] { "         move", " - moves an ", "Item", " to another floor\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
								WriteColor(new string[] { "       remove", " - removes an ", "Item", " from the current floor\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
								WriteColor(new string[] { " ren / rename", " - renames an ", "Item" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Yellow });
								WriteColor(new string[] { "  save/export", " - saves program objects to a text file, so you can implement\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
								WriteColor(new string[] { "                them in your own code\n" }, new ConsoleColor[] { ConsoleColor.White });
								WriteColor(new string[] { "          set", " - used to set variables in the command interpretter\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
								WriteColor(new string[] { "           up", " - goes up 1 floor\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
								WriteColor(new string[] { "          use", " - interacts with certain items (for interactive mode)\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
								WriteColor(new string[] { "ver / version", " - displays information about the command interpretter\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
								WriteColor(new string[] { "        visit", " - changes which House (and which viewer) you are in\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White });
								Console.Write("\ntype help (command) for more detailed information about a specific command\n\n");
							}
							break;
						}
						case "clear":
						case "cls": {
							Console.Clear();
							break;
						}
						case "exit":
						case "quit": {
							here = false;
							break;
						}
						case "ver":
						case "version": {
							WriteColor(new string[] { "\nCh", "ec", "k", $" Command Interpretter\n\tVersion {CurVer}\n\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.White });
							break;
						}
						default: {
							Console.Write($"\"{cmds[0][0]}\" is not a valid command:\n");
							for (int i = 0; i < cmds[0].Count; i++)
								Console.Write(cmds[0][i] + " ");
							Console.WriteLine();
							break;
						}
					}
					Console.WriteLine();
					bool keepRunning = true;
					if (cmds.Count > 1) {
						string[] message = { "`", cmds[0][0], "`", " returned ", ErrorLevel.ToString(), $": {(ErrorLevel >= 0 && ErrorLevel < errorMeaning.Length ? errorMeaning[ErrorLevel] : string.Empty)}" };
						for (int i = 1; i < cmds[0].Count; i++)
							message[1] += $" {cmds[0][i]}";
						switch (ErrorLevel) {
							case 0: break;
							case 1:
							case 2: goto recoverable;
							case 3:
							case 4: goto unrecoverable;
							recoverable:
								WriteColor(message, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.DarkGray, ConsoleColor.Cyan, ConsoleColor.White });
								keepRunning = GetInput(new ColorText(new string[] { "[", "C", "]ontinue running/[", "A", "]bort? " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White }), new string[] { "c", "a" }, true).Equals("c");
								break;
							unrecoverable:
								WriteColor(message, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.DarkGray, ConsoleColor.Cyan, ConsoleColor.White });
								keepRunning = false;
								break;
							default: {
								Console.WriteLine(message);
								WriteColor(new ColorText("Unknown return code, unrecoverable!\n", ConsoleColor.Red));
								keepRunning = false;
								break;
							}
						}
					}
					cmds.RemoveAt(0);
					if (!keepRunning) {
						WriteColor(new ColorText("Aborted.\n", ConsoleColor.Red));
						cmds = new List<List<string>>()
						{
							new List<string>()
						};
					}
				}
			}
		}

		private static string CurVer => $"{verMajor}.{verMinor}.{verFix}";

		public static int ErrorLevel { get; private set; }

		private static string SetErrorLevel(int err)
		{
			ErrorLevel = err;
			enviVar[5, 1] = err.ToString();
			return string.Empty;
		}

		public static void WriteColor(string[] lines, ConsoleColor[] colors)
		{
			if (lines.Length != colors.Length)
				return;
			for (int i = 0; i < colors.Length; i++) {
				Console.ForegroundColor = colors[i];
				Console.Write(lines[i]);
			}
			Console.ResetColor();
		}

		public static void WriteColor(ColorText text) => WriteColor(text.GetLines(), text.Colors());

		public static void WriteColor(ColorText[] lines)
		{
			foreach (ColorText line in lines)
				WriteColor(line.GetLines(), line.Colors());
		}

		public static void WriteColorLine(string[] lines, ConsoleColor[] colors)
		{
			WriteColor(lines, colors);
			Console.WriteLine();
		}

		public static ColorText[] Help(string cmd)
		{
			ColorText[] retCT;
			switch (cmd) {
				case "add":
					retCT = new ColorText[] {
						new ColorText(new string[] { "\nSyntax", " is: ", "add ", "item ", "[", "arg", "]\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White }),
						new ColorText(new string[] { "\titem", " - must be an ", "Item", " type, ", "House", ", ", "Floor", ", or ", "Room\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Green }),
						new ColorText(new string[] { "\t arg", " - causes you to be prompted for the required info to create a new\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White }),
						new ColorText(new string[] { "\t                Item", " of this type (without ", "arg", ", a default ", "Item", " is created)\n\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }),
						new ColorText(new string[] { "Adds", " Item ", "to the current floor\n\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.Yellow, ConsoleColor.White })
					};
					break;
				case "attach":
					retCT = new ColorText[] {
						new ColorText(new string[] { "\nSyntax", " is: ", "attach ", "src dst ", "[", "-d", "]\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White }),
						new ColorText(new string[] { "\tsrc", " - ", "integer", " of an ", "Item", " on the current floor (when used with ", "-d", ", ", "src\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Red }),
						new ColorText(new string[] { "\t      must be the ", "integer", " of the ", "Item", " that is attached)\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }),
						new ColorText(new string[] { "\tdst", " - ", "integer", " of an ", "Item", " on the current floor\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }),
						new ColorText(new string[] { "\t -d", " - ", "detaches", " source", " from ", "destination\n\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.DarkRed, ConsoleColor.White, ConsoleColor.DarkRed }),
						new ColorText(new string[] { "[De/A]ttaches ", "src", " [from/to] ", "dst", ".\n\n" }, new ConsoleColor[] { ConsoleColor.DarkBlue, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White })
					};
					break;
				case "clear":
				case "cls":
					retCT = new ColorText[] {
						new ColorText(new string[] { "\nSyntax", " is: ", $"{cmd.ToLower()}\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue }),
						new ColorText(new string[] { "Clears", " the console, and places cursor at home position\n\n" }, new ConsoleColor[] { ConsoleColor.DarkBlue, ConsoleColor.White })
					};
					break;
				case "down":
					retCT = new ColorText[] {
						new ColorText(new string[] { "\nSyntax", " is: ", "down", " [", "floors", "]\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White }),
						new ColorText(new string[] { "\tfloors", " - ", "integer", " of how many floors you want to go down\n\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White }),
						new ColorText(new string[] { "Moves to the next floor ", "down", ", unless you are at the bottom\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White })
					};
					break;
				case "exit":
				case "quit":
					retCT = new ColorText[] {
						new ColorText(new string[] { "\nSyntax", " is: ", $"{cmd.ToLower()}\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue }),
						new ColorText(new string[] { "Stops the program, and returns to your command line/operating environment\n\n" }, new ConsoleColor[] { ConsoleColor.White })
					};
					break;
				case "export":
				case "save":
					retCT = new ColorText[] {
						new ColorText(new string[] { "\nSyntax", " is: ", $"{cmd.ToLower()}", " [(", "-h", " / ", "-f", " / ", "-r", ") ", "src", "]\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White }),
						new ColorText(new string[] { "\t -h", " - specifies that you want to ", $"{cmd.ToLower()}", " House number ", "src\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Red }),
						new ColorText(new string[] { "\t -f", " - specifies that you want to ", $"{cmd.ToLower()}", " Floor number ", "src", " of the current House\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White }),
						new ColorText(new string[] { "\t -r", " - specifies that you want to ", $"{cmd.ToLower()}", " Room number ", "src", " of the current Floor of the current House\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White }),
						new ColorText(new string[] { "\tsrc", " - ", "integer", " of House, or Floor to ", $"{cmd.ToLower()}\n\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Blue }),
						new ColorText(new string[] { $"{cmd.ToLower()}s", " data from all Houses to a text file, or just specific Houses/Floors/Rooms\n\n" }, new ConsoleColor[] { ConsoleColor.DarkBlue, ConsoleColor.White })
					};
					break;
				case "find":
				case "search":
					retCT = new ColorText[] {
						new ColorText(new string[] { "\nSyntax", " is: ", $"{cmd.ToLower()}", " [", "-t", " item", "] [", "-f", " floor", "] [", "-r", " room", "]\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.Red, ConsoleColor.White }),
						new ColorText(new string[] { "\t   -t", " - specifies that you want to ", "search", " for a specific ", "Item", " type\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }),
						new ColorText(new string[] { "\t item", " - must be an ", "Item", " type\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }),
						new ColorText(new string[] { "\t   -f", " - specifies that you want to ", "search", " a specific floor\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White }),
						new ColorText(new string[] { "\tfloor", " - ", "integer", " of floor to ", "search\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Blue }),
						new ColorText(new string[] { "\t   -r", " - specifies that you want to ", "search", " a specific room\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White }),
						new ColorText(new string[] { "\t room", " - ", "integer", " of room to ", "search\n\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Blue }),
						new ColorText(new string[] { $"{(cmd.ToLower().Equals("find") ? "finds" : "searches for")}", " Items ", "matching the provided keywords, based on search parameters\n\n" }, new ConsoleColor[] { ConsoleColor.DarkBlue, ConsoleColor.Yellow, ConsoleColor.White })
					};
					break;
				case "grab":
				case "select":
					retCT = new ColorText[] {
						new ColorText(new string[] { "\nSyntax", " is: ", cmd.ToLower(), " item\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.Red }),
						new ColorText(new string[] { "\titem", " - ", "integer", " of ", "Item", " (see ", "list", ")\n\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White }),
						new ColorText(new string[] { "Changes the \"Viewer\"'s current ", "Item\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow })
					};
					break;
				case "goto":
					retCT = new ColorText[] {
						new ColorText(new string[] { "\nSyntax", " is: ", "goto", " [", "room", "]\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.Magenta, ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.Red }),
						new ColorText(new string[] { "\troom", " - ", "integer", " of room (run ", "goto", " without arguments to see rooms) [", "-1\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Green }),
						new ColorText("\t       exits all rooms (think of it like a hallway)]\n\n"),
						new ColorText("Changes the current room of the \"Viewer\"\n\n")
					};
					break;
				case "help":
					retCT = new ColorText[] {
						new ColorText(new string[] { "\nSyntax", " is: ", "help ", "[ ", "command", " [ ", "sub-topic", " ] ]\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White }),
						new ColorText(new string[] { "\t  command", " - a valid ", "command\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Blue }),
						new ColorText(new string[] { "\tsub-topic", " - some ", "commands", " can give you more info about their arguments\nColors:\n\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White }),
						new ColorText(new string[] { "Colors:\n" }, new ConsoleColor[] { ConsoleColor.White }),
						new ColorText(new string[] { "\t   R", "e", "d", " - warning -or- argument name (usually an integer)\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.DarkRed, ConsoleColor.Red, ConsoleColor.White }),
						new ColorText(new string[] { "\t         dark: usually expanded name of a commands argument (to show\n" }, new ConsoleColor[] { ConsoleColor.White }),
						new ColorText(new string[] { "\t         meaning)\n" }, new ConsoleColor[] { ConsoleColor.White }),
						new ColorText(new string[] { "\tY", "e", "l", "l", "o", "w", " - Item\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.DarkYellow, ConsoleColor.Yellow, ConsoleColor.DarkYellow, ConsoleColor.Yellow, ConsoleColor.White }),
						new ColorText(new string[] { "\t         dark: when talking about an Item but not using the exact term\n" }, new ConsoleColor[] { ConsoleColor.White }),
						new ColorText(new string[] { "\t         \"Item\" (or the exact name of an Item)\n" }, new ConsoleColor[] { ConsoleColor.White }),
						new ColorText(new string[] { "\t G", "r", "e", "e", "n", " - string argument (type it as it appears [without any brackets])\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.DarkGreen, ConsoleColor.Green, ConsoleColor.DarkGreen, ConsoleColor.Green, ConsoleColor.White }),
						new ColorText(new string[] { "\t         dark: (no use yet)\n" }, new ConsoleColor[] { ConsoleColor.White }),
						new ColorText(new string[] { "\t  C", "y", "a", "n", " - an integer for use when a command requires an item number\n" }, new ConsoleColor[] { ConsoleColor.Cyan, ConsoleColor.DarkCyan, ConsoleColor.Cyan, ConsoleColor.DarkCyan, ConsoleColor.White }),
						new ColorText(new string[] { "\t         dark: Item integer for sub-items (ie: a book in a bookshelf)\n" }, new ConsoleColor[] { ConsoleColor.White }),
						new ColorText(new string[] { "\t  B", "l", "u", "e", " - command\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.DarkBlue, ConsoleColor.Blue, ConsoleColor.DarkBlue, ConsoleColor.White }),
						new ColorText(new string[] { "\t         dark: when referencing the command without using its exact name\n\n" }, new ConsoleColor[] { ConsoleColor.White }),
					};
					break;
				case "info":
				case "status":
					retCT = new ColorText[] {
						new ColorText(new string[] { "\nSyntax", " is: ", $"{cmd.ToLower()}\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue }),
						new ColorText(new string[] { "Returns ", "info", " about the current 'Viewer'\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White })
					};
					break;
				case "list":
				case "look":
					retCT = new ColorText[] {
						new ColorText(new string[] { "\nSyntax", " is: ", cmd.ToLower(), " [", "item", "]\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White }),
						new ColorText(new string[] { $"           {cmd.ToLower()}", " [(", "-h", " / ", "-f", ")] [", "-r ", "start end", "] [", "-p", "] [", "-i ", "Item", "] [", "-x", " room", "]\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.Red, ConsoleColor.White }),
						new ColorText(new string[] { $"           {cmd.ToLower()}", " [(", "-h", ") [((", "-a", ") / ", "house", ")]]\n\n" }, new ConsoleColor[] { ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White }),
						new ColorText(new string[] { "\t   item", " - ", "integer", " of ", "Item", " (see ", "list", ")\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White }),
						new ColorText(new string[] { "\t-h / -f", " - will show the \"Viewer\"'s current ", "Item\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Yellow }),
						new ColorText(new string[] { "\t          Long version is ", "--hand", " or ", "--focus\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Green }),
						new ColorText(new string[] { "\t     -r", " - will ", "list", " Items", " between [", "start", "] and [", "end", "]\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White }),
						new ColorText(new string[] { "\t          Long version is ", "--range\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green }),
						new ColorText(new string[] { "\t     -p", " - ", "lists", " all ", "Items", " on the floor one page at a time (page is\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }),
						new ColorText(new string[] { "\t          defined as 20 lines)\n" }, new ConsoleColor[] { ConsoleColor.White }),
						new ColorText(new string[] { "\t          Long version is ", "--page\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green }),
						new ColorText(new string[] { "\t-i ", "Item", " - ", "lists", " all ", "Items", " of type ", "Item", " (", "Item", " string)\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }),
						new ColorText(new string[] { "\t          Long version is ", "--item\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green }),
						new ColorText(new string[] { "\t     -x", " - ", "lists", " Items ", "on the floor, that are in ", "room\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.DarkYellow, ConsoleColor.White, ConsoleColor.Red }),
						new ColorText(new string[] { "\t          Long version is ", "--room\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green }),
						new ColorText(new string[] { "\t     -a", " - ", "lists", " info about House ", "house", " / House at ", "-a\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Green }),
						new ColorText(new string[] { "\t          Long version is ", "--house", " and ", "--address\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Green }),
						new ColorText(new string[] { "Used for getting info about an ", "Item", ", or multiple ", "Items", ".\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White }),
					};
					break;
				case "move":
					retCT = new ColorText[] {
						new ColorText(new string[] { "\nSyntax", " is: ", "move ", "item floor", "[", ":", "room", "]\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.Red, ConsoleColor.White }),
						new ColorText(new string[] { "\t item", " - ", "integer", " of ", "Item", " (see ", "list", ")\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White }),
						new ColorText(new string[] { "\tfloor", " - ", "integer", " of floor (or: ", "<", " for next floor down or ", ">", " for next floor\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White }),
						new ColorText(new string[] { "\t        up)\n" }, new ConsoleColor[] { ConsoleColor.White }),
						new ColorText(new string[] { "\t:", "room", " - ", "integer", " of Room\n\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White }),
						new ColorText(new string[] { "Moves", " an ", "Item", " from your current floor to the specified floor.\n\n" }, new ConsoleColor[] { ConsoleColor.DarkBlue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White })
					};
					break;
				case "remove":
					retCT = new ColorText[] {
						new ColorText(new string[] { "\nSyntax", " is: ", "remove ", "item", " [", "sub-item", " / ", "-h", " / ", "-f", " / ", "-r", "]\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White }),
						new ColorText(new string[] { "\t    item", " - ", "integer", " of ", "Item", " (see ", "list", ")\n\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White }),
						new ColorText(new string[] { "\tsub-item", " - ", "integer", " of ", "subItem\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.DarkYellow }),
						new ColorText(new string[] { "\t      -h", " - ", "removes", " house number ", "item\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White, ConsoleColor.Red }),
						new ColorText(new string[] { "\t           Long version is ", "--house\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green }),
						new ColorText(new string[] { "\t      -f", " - ", "removes", " floor number ", "item\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White, ConsoleColor.Red }),
						new ColorText(new string[] { "\t           Long version is ", "--floor\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green }),
						new ColorText(new string[] { "\t      -r", " - ", "removes", " room number ", "item\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White, ConsoleColor.Red }),
						new ColorText(new string[] { "\t           Long version is ", "--room\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Green }),
						new ColorText(new string[] { "Removes", " specified ", "Item", " from current floor.\n\n" }, new ConsoleColor[] { ConsoleColor.DarkBlue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White })
					};
					break;
				case "ren":
				case "rename":
					retCT = new ColorText[] {
						new ColorText(new string[] { "\nSyntax", " is: ", cmd.ToLower(), " item ", "name...\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.Green }),
						new ColorText(new string[] { "\t   item", " - ", "integer", " of ", "Item", " to ", "rename\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Blue }),
						new ColorText(new string[] { "\tname...", " - all text after ", "item", ", will be used as the ", "Item's", " name\n\n" }, new ConsoleColor[] { ConsoleColor.Green, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White }),
						new ColorText(new string[] { "Renames", " specified ", "Item.\n\n" }, new ConsoleColor[] { ConsoleColor.DarkBlue, ConsoleColor.White, ConsoleColor.Yellow })
					};
					break;
				case "set":
					retCT = new ColorText[] {
						new ColorText(new string[] { "\nSyntax", " is: ", "set", " [", "variable", " [", "value", "]]\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White }),
						new ColorText(new string[] { "\tvariable", " - displays what said ", "variable", " is currently set to\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White }),
						new ColorText(new string[] { "\t   value", " - sets ", "variable", " to ", "value\n\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Red }),
						new ColorText(new string[] { "Sets", " and views variables.\n\n" }, new ConsoleColor[] { ConsoleColor.DarkBlue, ConsoleColor.White })
					};
					break;
				case "up":
					retCT = new ColorText[] {
						new ColorText(new string[] { "\nSyntax", " is: ", "up", " [", "floors", "]\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White }),
						new ColorText(new string[] { "\tfloors", " - ", "integer", " of how many floors you want to go up\n\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White }),
						new ColorText(new string[] { "Moves to the next floor ", "up", ", unless you are at the top\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White })
					};
					break;
				case "use":
					retCT = new ColorText[] {
						new ColorText(new string[] { "\nSyntax", " is: ", "use", " object\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.Red }),
						new ColorText(new string[] { "\tobject", " - an object to interact with\n\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White }),
						new ColorText(new string[] { "Uses", " an interactable object.\n\n" }, new ConsoleColor[] { ConsoleColor.DarkBlue, ConsoleColor.White })
					};
					break;
				case "ver":
				case "version":
					retCT = new ColorText[] {
						new ColorText(new string[] { "\nSyntax", " is: ", $"{cmd.ToLower()}\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue }),
						new ColorText(new string[] { "Tells you the current ", "version", " of the Check Command Interpretter\n\n" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White }),
					};
					break;
				case "visit":
					retCT = new ColorText[] {
						new ColorText(new string[] { "\nSyntax", " is: ", "visit", " house\n\n" }, new ConsoleColor[] { ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.Red }),
						new ColorText(new string[] { "\thouse", " - ", "integer", " of house to go to\n\n" }, new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White }),
						new ColorText(new string[] { "Visits", " a specified house.\n\n" }, new ConsoleColor[] { ConsoleColor.DarkBlue, ConsoleColor.White })
					};
					break;
				//start sub-help
				case "top-item":
					List<string> retStr = new List<string>() { "\nItem", " must be one of these types:\n" };
					List<ConsoleColor> retClr = new List<ConsoleColor>() { ConsoleColor.Yellow, ConsoleColor.White };
					foreach (string type in topTypes) {
						retStr.Add($"\n\t* {type}");
						retClr.Add(ConsoleColor.Yellow);
					}
					retStr.Add("\n\n");
					retClr.Add(ConsoleColor.White);
					retCT = new ColorText[] { new ColorText(retStr.ToArray(), retClr.ToArray()) };
					break;
				case "floor-interact":
					List<string> retStr2 = new List<string>() { "\nObject", " must be one of these types:\n" };
					List<ConsoleColor> retClr2 = new List<ConsoleColor>() { ConsoleColor.Red, ConsoleColor.White };
					foreach (string obj in floorInteracts) {
						retStr2.Add($"\n\t* {obj}");
						retClr2.Add(ConsoleColor.Yellow);
					}
					retStr2.Add("\n\n");
					retClr2.Add(ConsoleColor.White);
					retCT = new ColorText[] { new ColorText(retStr2.ToArray(), retClr2.ToArray()) };
					break;
				case "bad-sub":
					retCT = new ColorText[] { new ColorText("\nInvalid help sub-topic.\n\n") };
					break;
				default:
					retCT = new ColorText[] {
						new ColorText(new string[] { $"\"{cmd}\" not recognized as a command, or command topic\n" }, new ConsoleColor[]{ ConsoleColor.White })
						//new ColorText(new string[] { "Code error!!! (Please report, as this message shouldn't be possible to see.)" }, new ConsoleColor[] { ConsoleColor.Red })
					};
					break;
			}
			return retCT;
		}

		public static bool EqualsIgnoreCaseOr(string test, string[] strs)
		{
			if (string.IsNullOrEmpty(test))
				throw new ArgumentException("message", nameof(test));
			for (int i = 0; i < strs.Length; i++)
				if (string.Equals(test, strs[i], StringComparison.OrdinalIgnoreCase))
					return true;
			return false;
		}

		public static bool CanGoInside(string src, string dst)
		{
			if (string.IsNullOrEmpty(src) || string.IsNullOrEmpty(dst))
				throw new ArgumentException("message", nameof(src));
			switch (dst.ToLower()) {
				case "bookshelf":
					return src.ToLower().Equals("book");
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
				case "fridge":
					switch (src.ToLower()) {
						case "empty":
						case "fridge":
							return false;
						default:
							return true;
					}
				case "dresser":
					return src.ToLower().Equals("clothing");
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

		public static int GetInput(int min, int max)
		{
			if (min >= max)
				throw new InvalidRange(min, max);
			do {
				WriteColor(new string[] { "\n[", min.ToString(), "-", (max - 1).ToString(), "] > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
				string lineIn = Console.ReadLine();
				if (Regex.IsMatch(lineIn, @"^-?\d+$") && int.Parse(lineIn) >= min && int.Parse(lineIn) < max)
					return int.Parse(lineIn);
			} while (true);
		}

		public static double GetInput(double min, double max)
		{
			if (min >= max)
				throw new InvalidRange(min, max);
			do {
				WriteColor(new string[] { "\n[", min.ToString(), "-", (max - 1.0).ToString(), "] > " }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White });
				string lineIn = Console.ReadLine();
				if (Regex.IsMatch(lineIn, @"^-?\d+([.]{1}\d+)?$") && double.Parse(lineIn) >= min && double.Parse(lineIn) <= max - 1.0)
					return double.Parse(lineIn);
			} while (true);
		}

		public static string GetInput(ColorText message, string[] values, bool ignoreCase)
		{
			if (values is null)
				throw new ArgumentNullException(nameof(values));
			if (values.Length == 0)
				throw new ArrayTooSmall(1, values.Length);
			do {
				WriteColor(new ColorText[] { new ColorText("\n"), message, new ColorText("> ") });
				string lineIn = Console.ReadLine();
				foreach (string test in values)
					if ((ignoreCase && lineIn.Equals(test, StringComparison.OrdinalIgnoreCase)) || lineIn.Equals(test))
						return lineIn;
			} while (true);
		}

		public static string OrdSuf(int num)
		{
			if (num / 10 == 1) return $"{num}th";
			switch (num % 10) {
				case 1:
					return $"{num}st";
				case 2:
					return $"{num}nd";
				case 3:
					return $"{num}rd";
				case 0:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
					return $"{num}th";
				default:
					return $"{num}";
			}
		}
	}
}
