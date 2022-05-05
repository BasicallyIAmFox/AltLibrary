using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks
{
    internal class MimicSummon
    {
        public static void Init()
        {
            IL.Terraria.NPC.BigMimicSummonCheck += NPC_BigMimicSummonCheck;
        }

        private static void NPC_BigMimicSummonCheck(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchLdcI4(3092)))
                return;
            c.Index++;
            c.EmitDelegate<Func<int, int>>((kol) =>
            {
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).MimicKeyType.HasValue)
                {
                    return ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).MimicKeyType.Value;
                }
                return kol;
            });
            if (!c.TryGotoNext(i => i.MatchLdcI4(3091)))
                return;
            c.Index++;
            c.EmitDelegate<Func<int, int>>((kon) =>
            {
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).MimicKeyType.HasValue)
                {
                    return ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).MimicKeyType.Value;
                }
                return kon;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(475)))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldloc, 4);
            c.EmitDelegate<Func<int, int, int>>((value, isGood) =>
            {
                if (WorldBiomeManager.worldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).MimicType.HasValue)
                {
                    return ModContent.Find<AltBiome>(WorldBiomeManager.worldHallow).MimicType.Value;
                }
                return value;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(473)))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldloc, 2);
            c.EmitDelegate<Func<int, int, int>>((value, isEvil) =>
            {
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).MimicType.HasValue)
                {
                    return ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).MimicType.Value;
                }
                return value;
            });
        }
    }
}
