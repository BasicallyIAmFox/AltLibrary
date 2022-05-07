using AltLibrary.Common.AltBiomes;
using Terraria.ID;
using Microsoft.Xna.Framework;

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
            BiomeDirt = TileID.AdamantiteBeam;
            BiomeSnow = TileID.Asphalt;
        }
        public override Color OuterColor => new(214, 66, 56);
        public override string WorldIcon => "AltLibrary/Assets/WorldIcons/Pur";
    }
}
