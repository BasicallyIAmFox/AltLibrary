using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using MonoMod.Cil;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AltLibrary.Common.Hooks
{
    [Autoload(Side = ModSide.Both)]
    internal class NearbyAltChloro
    {
        public static void Init()
        {
            IL.Terraria.WorldGen.nearbyChlorophyte += WorldGen_nearbyChlorophyte;
        }

        public static void Unload()
        {
            IL.Terraria.WorldGen.nearbyChlorophyte -= WorldGen_nearbyChlorophyte;
        }

        private static void WorldGen_nearbyChlorophyte(ILContext il)
        {
            ALUtils.ReplaceIDs<int>(il,
                TileID.Chlorophyte,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldJungle).BiomeOre ?? orig,
                (orig) => WorldBiomeManager.WorldJungle != "" && Find<AltBiome>(WorldBiomeManager.WorldJungle).BiomeOre.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                TileID.ChlorophyteBrick,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldJungle).BiomeOreBrick ?? orig,
                (orig) => WorldBiomeManager.WorldJungle != "" && Find<AltBiome>(WorldBiomeManager.WorldJungle).BiomeOreBrick.HasValue);
        }
    }
}
