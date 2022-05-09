using AltLibrary.Common.AltBiomes;
using Terraria.ID;

namespace AltLibrary
{
    internal class CumfrostStrikeAgain : AltBiome
    {
        public override void SetStaticDefaults()
        {
            BiomeType = BiomeType.Hell;
            BiomeStone = TileID.Mythril;
            BiomeChestTile = TileID.Containers2;
            BiomeChestTileStyle = 13;
            BiomeChestItem = ItemID.ShadowbeamStaff;
            BiomeOre = TileID.Adamantite;
            BiomeOreBrick = TileID.Orichalcum;
            DisplayName.SetDefault("Cumfrosty");
            Description.SetDefault("lion loves this biome, that's truth");
        }
    }
}
