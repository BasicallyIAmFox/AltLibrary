using AltLibrary.Common.AltBiomes;
using System.Collections.Generic;
using Terraria.ID;

namespace AltLibrary
{
    internal class EpicFurryBiome : AltBiome
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
        }

        public override Dictionary<int, int> SpecialConversion => new()
        {
            [TileID.Dirt] = TileID.AdamantiteBeam,
            [TileID.SnowBlock] = TileID.Asphalt
        };
    }
}
