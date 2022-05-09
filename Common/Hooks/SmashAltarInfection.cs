using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
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
                if (!AltLibraryConfig.Config.SmashingAltarsSpreadsRandom)
                {
                    if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.HasValue)
                    {
                        return ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeStone.Value;
                    }
                }
                else
                {
                    List<int> indexToUse = new();
                    indexToUse.Add(orig);
                    AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Evil && x.BiomeStone.HasValue)
                                        .ToList()
                                        .ForEach(x => indexToUse.Add(x.BiomeStone.Value));
                    return indexToUse[Main.rand.Next(indexToUse.Count)];
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(117)))
                return;

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (!AltLibraryConfig.Config.SmashingAltarsSpreadsRandom)
                {
                    if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.HasValue)
                    {
                        return ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).BiomeStone.Value;
                    }
                }
                else
                {
                    List<int> indexToUse = new();
                    indexToUse.Add(orig);
                    AltLibrary.Biomes.Where(x => x.BiomeType == BiomeType.Hallow && x.BiomeStone.HasValue)
                                        .ToList()
                                        .ForEach(x => indexToUse.Add(x.BiomeStone.Value));
                    return indexToUse[Main.rand.Next(indexToUse.Count)];
                }
                return orig;
            });
        }
    }
}
