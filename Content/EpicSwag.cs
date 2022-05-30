using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.AltOres;
using Terraria.ID;

namespace AltLibrary.Content
{
    internal class EpicSwag : AltBiome
    {
        public override void SetStaticDefaults()
        {
            BiomeType = BiomeType.Evil;
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

    internal class BaddaassssOre : AltOre
    {
        public override void SetStaticDefaults()
        {
            OreType = OreType.Cobalt;
            ore = TileID.LunarOre;
        }
    }
}
