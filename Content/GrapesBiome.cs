using AltLibrary.Common.AltBiomes;
using System.Collections.Generic;
using Terraria.ID;

namespace AltLibrary.Content
{
    internal class GrapesBiome : AltBiome
    {
        public override void SetStaticDefaults()
        {
            BiomeType = BiomeType.Jungle;
            BiomeGrass = TileID.SandStoneSlab;
        }

        public override string WorldIcon => "";
    }
}
