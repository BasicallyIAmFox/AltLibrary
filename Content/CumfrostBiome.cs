using AltLibrary.Common.AltBiomes;
using Terraria.ID;

namespace AltLibrary.Content
{
    public class CumfrostBiome : AltBiome
    {
        public override void SetStaticDefaults()
        {
            BiomeType = BiomeType.Hell;
            BiomeStone = TileID.Mythril;
        }
    }
}
