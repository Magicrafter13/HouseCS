using System.Collections.Generic;
using System;
using HouseCS.ConsoleUtils;

namespace HouseCS.Items.Containers
{
    public class Fridge : Container, IItem
    {
        private static readonly string typeS = "Fridge";
        private double temperature = 35.0;
        private bool celsius = false;
        private double freezerTemp = 0.0;
        private bool freezerCelsius = false;

        public Fridge() : this(false) { }
        public Fridge(List<IItem> iS) : base(iS) { }
        public Fridge(bool f) : base() => HasFreezer = f;
        public Fridge(List<IItem> iS, bool f) : base(iS) => HasFreezer = f;
        public bool HasFreezer { get; private set; }
        public void ToCelsius(bool freezer) {
            if (freezer) {
                if (!freezerCelsius) {
                    freezerCelsius = true;
                    freezerTemp = (freezerTemp - 32.0) * 5.0/9.0;
                }
            } else {
                if (!celsius) {
                    celsius = true;
                    temperature = (temperature - 32.0) * 5.0/9.0;
                }
            }
        }
        public void ToFarenheit(bool freezer) {
            if (freezer) {
                if (freezerCelsius) {
                    freezerCelsius = false;
                    freezerTemp = (freezerTemp * 9.0/5.0) + 32.0;
                }
            } else {
                if (celsius) {
                    celsius = false;
                    temperature = (temperature * 9.0/5.0) + 32.0;
                }
            }
        }
        public void tempInc() => temperature += 1.0;
        public void tempDec() => temperature -= 1.0;
        public void tempChange(double n) => temperature += n;
        public void tempReset() {
            celsius = false;
            temperature = 35.0;
        }
        public new string SubType => typeS;
        public new ColorText ListInfo(bool beforeNotAfter) => beforeNotAfter
            ? new ColorText($"{temperature}° ", ConsoleColor.White)
            : Size > 0
                ? new ColorText(new string[] { " - [", Size.ToString(), " Items", $"]{(HasFreezer ? $", with {freezerTemp}° Freezer - " : "")}" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.White })
                : new ColorText(new string[] { " - ", "Empty", $"]{(HasFreezer ? $", with {freezerTemp}° Freezer - " : "")}" }, new ConsoleColor[] { ConsoleColor.White, ConsoleColor.DarkYellow, ConsoleColor.White });
        public new ColorText ToText() {
            List<string> retStr = new List<string>() { "Items", " in this ", "Fridge", ":" };
            List<ConsoleColor> retClr = new List<ConsoleColor>() { ConsoleColor.DarkYellow, ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.White };
            for (int i = 0; i < Size; i++) {
                retStr.Add($"\n\t{i.ToString()}");
                retClr.Add(ConsoleColor.Cyan);
                retStr.Add(": ");
                retClr.Add(ConsoleColor.White);
                ColorText left = GetItem(i).ListInfo(true);
                foreach (string str in left.Lines)
                    retStr.Add(str);
                foreach (ConsoleColor clr in left.Colors)
                    retClr.Add(clr);
                retStr.Add(GetItem(i).SubType);
                retClr.Add(ConsoleColor.Yellow);
                ColorText right = GetItem(i).ListInfo(false);
                foreach (string str in right.Lines)
                    retStr.Add(str);
                foreach (ConsoleColor clr in right.Colors)
                    retClr.Add(clr);
            }
            retStr.Add("\nEnd of ");
            retClr.Add(ConsoleColor.White);
            retStr.Add("Fridge");
            retClr.Add(ConsoleColor.Yellow);
            retStr.Add(" contents.");
            retClr.Add(ConsoleColor.White);
            return new ColorText(retStr.ToArray(), retClr.ToArray());
        }
    }
}