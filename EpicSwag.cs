using AltLibrary.Common.AltBiomes;
using Terraria.ID;

namespace AltLibrary
{
    internal class EpicSwag : AltBiome
    {
        public override void SetStaticDefaults()
        {
            BiomeType = BiomeType.Hallow;
            BiomeGrass = TileID.GoldBrick;
            BiomeStone = TileID.Gold;
            BiomeSand = TileID.YellowStucco;
            BiomeIce = TileID.TeamBlockYellow;
            BiomeSandstone = TileID.PlatinumBrick;
            MechDropItemType = ItemID.GoldBar;
            BiomeChestItem = ItemID.ReflectiveGoldDye;
            BiomeChestTile = TileID.Containers2;
            BiomeChestTileStyle = 6;
            BiomeMowedGrass = TileID.Platinum;
            MimicKeyType = ItemID.GoldCrown;
            MimicType = NPCID.GoldenSlime;
            FountainTile = TileID.SeaweedPlanter;
            FountainTileStyle = 0;
            FountainActiveFrameY = 0;
        }
    }
}
