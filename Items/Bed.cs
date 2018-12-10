namespace HouseCS.Items
{
    public class Bed : IItem
    {
        public static readonly string[] types = {"King", "Queen", "Twin", "Single"};
        private bool adjustable;
        private int bedType;
        private static readonly string typeS = "Bed";

        public Bed() : this(false, 2) { }
        public Bed(bool a, int t) {
            adjustable = a;
            bedType = t >= 0 && t < types.Length ? t : 2;
        }
        public bool HasItem(IItem test) => false;
        public string Type => typeS;
        public string ListInfo(bool beforeNotAfter) => beforeNotAfter
            ? $"{types[bedType]} "
            : adjustable
                ? " - Adjustable"
                : "";
        public override string ToString() => $"{(adjustable ? "Adjustable" : "Non adjustable")} {types[bedType]} size bed";
    }
}