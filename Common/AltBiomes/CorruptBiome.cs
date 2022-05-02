using Terraria.ModLoader;

namespace AltLibrary.Common.AltBiomes
{
    internal sealed class CorruptBiome : AltBiome
    {
        public override string IconSmall => "Terraria/Images/UI/Bestiary/Icon_Tags_Shadow";

        public override void SetStaticDefaults()
        {
            BiomeType = BiomeType.Evil;
        }

        public override bool IsLoadingEnabled(Mod mod) => false;
    }
}
