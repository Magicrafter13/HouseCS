using System;
using HouseCS.ConsoleUtils;

namespace HouseCS.Items.Clothes
{
    public class Clothing : IItem
    {
        private static readonly string typeS = "Clothing";

        public Clothing() : this("Black") { }
        public Clothing(string c) => Color = c;
        public string Color { get; set; }
        public bool HasItem(IItem test) => false;
        public string Type => typeS;
        public string SubType => Type;
        public ColorText ListInfo(bool beforeNotAfter) => new ColorText(beforeNotAfter ? $"{Color} " : " - Generic", ConsoleColor.White);
        public ColorText ToText() => new ColorText($"This is a Generic piece of clothing, it is {Color}", ConsoleColor.White);
    }
}