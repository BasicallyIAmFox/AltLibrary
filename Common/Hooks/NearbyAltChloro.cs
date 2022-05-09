using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using MonoMod.Cil;
using System;
using Terraria.ModLoader;

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
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchLdcI4(211)))
                return;
            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldBiomeManager.worldJungle != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldJungle).BiomeOre.HasValue)
                {
                    return ModContent.Find<AltBiome>(WorldBiomeManager.worldJungle).BiomeOre.Value;
                }
                return orig;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(346)))
                return;
            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldBiomeManager.worldJungle != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldJungle).BiomeOreBrick.HasValue)
                {
                    return ModContent.Find<AltBiome>(WorldBiomeManager.worldJungle).BiomeOreBrick.Value;
                }
                return orig;
            });
        }
    }
}
