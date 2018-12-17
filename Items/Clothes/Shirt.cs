using System;
using HouseCS.ConsoleUtils;

namespace HouseCS.Items.Clothes
{
    public class Shirt : Clothing, IItem
    {
        private static readonly string typeS = "Shirt";

        public Shirt() : base() { }
        public Shirt(string c) : base(c) { }
        public new string SubType => typeS;
        public new ColorText ListInfo(bool beforeNotAfter) => new ColorText(beforeNotAfter ? $"{Color} " : "", ConsoleColor.White);
        public new ColorText ToText() => new ColorText($"This is a {Color} {SubType}", ConsoleColor.White);
    }
}