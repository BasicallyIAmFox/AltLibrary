using AltLibrary.Common;
using Terraria.ID;

namespace AltLibrary.Content
{
    internal class BadassOre : AltOre
    {
        public override void SetStaticDefaults()
        {
            OreType = OreType.Copper;
            ore = TileID.ArgonMoss;
            bar = ItemID.ArgonMoss;
        }
    }
}
