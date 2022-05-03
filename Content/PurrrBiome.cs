using AltLibrary.Common.AltBiomes;
using Terraria.ID;

namespace AltLibrary.Content
{
    public class PurrrBiome : AltBiome
    {
        public override WallContext WallContext => new WallContext()
                .AddReplacement(1, 10);

        public override void SetStaticDefaults()
        {
            BiomeType = BiomeType.Evil;
            BiomeGrass = TileID.Obsidian;
            BiomeStone = TileID.Mythril;
            BiomeOre = TileID.Palladium;
            BiomeSand = TileID.HallowedIce;
            BiomeSandstone = TileID.SnowBrick;
            BiomeOreItem = ItemID.SillyBalloonGreen;
            SeedType = ItemID.JungleSpores;
        }

        public override string IconLarge => "AltLibrary/Assets/WorldIcons/Pur";
    }
}
