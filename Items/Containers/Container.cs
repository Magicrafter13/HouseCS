using System.Collections.Generic;
using HouseCS.Items;
using HouseCS.ConsoleUtils;
using System;

namespace HouseCS.Items.Containers
{
    public class Container : IItem {
        public List<IItem> items { get; private set; }
        private static readonly string typeS = "Container";

        public Container() : this(new List<IItem>()) { }
        public Container(List<IItem> iS) => items = iS;
        public IItem GetItem(int i) => i < 0 || i >= items.Count ? new Empty() : items[i];
        public ColorText AddItem(IItem i) {
            if (i == this) return new ColorText("You can't put something inside itself!", ConsoleColor.White);
            if (HasItem(i)) return new ColorText(new string[] { "That ", "Item", $" is already in this {typeS}! (I don't think this message should be able to be seen.)" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
            items.Add(i);
            return new ColorText(new string[] { "\nItem", " added ", "to this ", typeS, ".\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.DarkBlue, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });
        }
        public ColorText RemoveItem(int i) {
            if (i < 0 || i >= items.Count) {
                List<string> retStr = new List<string>();
                List<ConsoleColor> retClr = new List<ConsoleColor>();
                retStr.Add("This ");
                retClr.Add(ConsoleColor.White);
                retStr.Add(typeS);
                retClr.Add(ConsoleColor.Yellow);
                if (items.Count > 0) {
                    retStr.Add(" only has ");
                    retClr.Add(ConsoleColor.White);
                    retStr.Add(items.Count.ToString());
                    retClr.Add(ConsoleColor.Cyan);
                    retStr.Add(" items in it.");
                    retClr.Add(ConsoleColor.White);
                } else {
                    retStr.Add(" is ");
                    retClr.Add(ConsoleColor.White);
                    retStr.Add("Empty");
                    retClr.Add(ConsoleColor.Yellow);
                }
                return new ColorText(retStr.ToArray(), retClr.ToArray());
            }
            items.RemoveAt(i);
            return new ColorText(new string[] { "\nItem ", "removed", ".\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.DarkBlue, ConsoleColor.White });
        }
        public ColorText RemoveItem(IItem i) => items.Remove(i) ? new ColorText(new string[] { "\nItem ", "removed", ".\n" }, new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.DarkBlue, ConsoleColor.White }) : new ColorText(new string[] { "No matching ", "Item", " found" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White });

        public int Size => items.Count;

        public bool HasItem(IItem test) {
            foreach (IItem i in items) if (i == test) return true;
            return false;
        }
        public String Type => typeS;
        public String SubType => Type;
        public ColorText ListInfo(bool beforeNotAfter) => beforeNotAfter ? ColorText.Empty : items.Count > 0 ? new ColorText(new string[] { ", ", items.Count.ToString(), " Items" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.Yellow }) : new ColorText(new string[] { ", ", "Empty" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Yellow });
        public ColorText ToText() {
            List<string> retStr = new List<string>() { typeS };
            List<ConsoleColor> retClr = new List<ConsoleColor>() { ConsoleColor.Yellow };
            for (int i = 0; i < items.Count; i++) {
                retStr.Add($"\n\t{i.ToString()}");
                retClr.Add(ConsoleColor.Cyan);
                retStr.Add(": ");
                retClr.Add(ConsoleColor.White);
                ColorText left = items[i].ListInfo(true);
                foreach (string str in left.Lines)
                    retStr.Add(str);
                foreach (ConsoleColor clr in left.Colors)
                    retClr.Add(clr);
                retStr.Add(items[i].SubType);
                retClr.Add(ConsoleColor.Yellow);
                ColorText right = items[i].ListInfo(false);
                foreach (string str in right.Lines)
                    retStr.Add(str);
                foreach (ConsoleColor clr in right.Colors)
                    retClr.Add(clr);
            }
            retStr.Add("\nEnd of ");
            retClr.Add(ConsoleColor.White);
            retStr.Add("Container");
            retClr.Add(ConsoleColor.Yellow);
            retStr.Add(" contents.");
            retClr.Add(ConsoleColor.White);
            return new ColorText(retStr.ToArray(), retClr.ToArray());
        }
    }
}