﻿using AltLibrary.Common.AltBiomes;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Linq;
using Terraria.GameContent.ItemDropRules;

namespace AltLibrary.Common.Hooks
{
    internal class TwinsRules
    {
        public static void Init()
        {
            IL.Terraria.GameContent.ItemDropRules.ItemDropDatabase.RegisterBoss_Twins += ItemDropDatabase_RegisterBoss_Twins;
        }

        private static void ItemDropDatabase_RegisterBoss_Twins(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchLdcI4(1225)))
                return;
            if (!c.TryGotoNext(i => i.MatchPop()))
                return;
            c.Index++;
            c.Emit(OpCodes.Ldloc, 1);
            c.EmitDelegate<Func<LeadingConditionRule, LeadingConditionRule>>((leadCond) =>
            {
                if (AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Hallow).Any())
                {
                    foreach (AltBiome biome in AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Hallow))
                    {
                        if (biome.MechDropItemType != null)
                        {
                            leadCond.ChainedRules.RemoveAt(1);
                            break;
                        }
                    }
                }
                foreach (AltBiome biome in AltLibrary.biomes.Where(x => x.BiomeType == BiomeType.Hallow))
                {
                    if (biome.MechDropItemType != null)
                    {
                        leadCond.OnSuccess(ItemDropRule.ByCondition(new HallowedBarAltDropCondition(biome), (int)biome.MechDropItemType, 1, 15, 30));
                    }
                }
                return leadCond;
            });
            c.Emit(OpCodes.Stloc, 1);
        }
    }
}
