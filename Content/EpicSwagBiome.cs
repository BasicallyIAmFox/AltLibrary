using AltLibrary.Common.AltBiomes;
using System.Collections.Generic;
using Terraria.ID;

namespace AltLibrary.Content
{
    internal class EpicSwagBiome : AltBiome
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
        public override List<int> SpreadingTiles => new List<int> { TileID.GoldBrick, TileID.Gold };

        public override string IconLarge => "AltLibrary/Assets/WorldIcons/EpicSwag";
    }
}
