using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using MonoMod.Cil;
using System;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks
{
    internal class TenthAnniversaryFix
    {
        public static void Init()
        {
            IL.Terraria.WorldGen.ConvertSkyIslands += WorldGen_ConvertSkyIslands;
        }

        private static void WorldGen_ConvertSkyIslands(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchLdcI4(109)))
                return;
            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass.HasValue)
                {
                    return ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeGrass.Value;
                }
                return orig;
            });
        }
    }
}
