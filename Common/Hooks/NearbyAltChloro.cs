using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using MonoMod.Cil;
using System;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace AltLibrary.Common.Hooks
{
    internal class NearbyAltChloro
    {
        public static void Init()
        {
            IL.Terraria.WorldGen.nearbyChlorophyte += WorldGen_nearbyChlorophyte;
        }

        private static void WorldGen_nearbyChlorophyte(ILContext il)
        {
            ALUtils.ReplaceIDs<int>(il,
                TileID.Chlorophyte,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldJungle).BiomeOre ?? orig,
                (orig) => WorldBiomeManager.worldJungle != "" && Find<AltBiome>(WorldBiomeManager.worldJungle).BiomeOre.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                TileID.ChlorophyteBrick,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldJungle).BiomeOreBrick ?? orig,
                (orig) => WorldBiomeManager.worldJungle != "" && Find<AltBiome>(WorldBiomeManager.worldJungle).BiomeOreBrick.HasValue);
        }
    }
}
