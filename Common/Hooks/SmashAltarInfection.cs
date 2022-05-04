using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using MonoMod.Cil;
using System;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks
{
    internal class SmashAltarInfection
    {
        public static void Init()
        {
            IL.Terraria.WorldGen.SmashAltar += WorldGen_SmashAltar;
        }

        private static void WorldGen_SmashAltar(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchLdcI4(25)))
                return;

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.HasValue)
                {
                    return ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.Value;
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(117)))
                return;

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.HasValue)
                {
                    return ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.Value;
                }
                return orig;
            });
        }
    }
}
