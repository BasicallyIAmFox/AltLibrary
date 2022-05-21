using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

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

            ALUtils.ReplaceIDs<int>(il,
                ItemID.LightKey,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldHallow).MimicKeyType ?? orig,
                (orig) => WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).MimicKeyType.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                ItemID.NightKey,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldEvil).MimicKeyType ?? orig,
                (orig) => WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).MimicKeyType.HasValue);

            if (!c.TryGotoNext(i => i.MatchLdcI4(475)))
            {
                AltLibrary.Instance.Logger.Info("k $ 1");
                return;
            }

            c.Index++;
            c.Emit(OpCodes.Ldloc, 4);
            c.EmitDelegate<Func<int, int, int>>((value, isGood) =>
            {
                if (WorldBiomeManager.worldHallow != "" && Find<AltBiome>(WorldBiomeManager.worldHallow).MimicType.HasValue)
                {
                    return Find<AltBiome>(WorldBiomeManager.worldHallow).MimicType.Value;
                }
                return value;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(473)))
            {
                AltLibrary.Instance.Logger.Info("k $ 2");
                return;
            }

            c.Index++;
            c.Emit(OpCodes.Ldloc, 2);
            c.EmitDelegate<Func<int, int, int>>((value, isEvil) =>
            {
                if (WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).MimicType.HasValue)
                {
                    return Find<AltBiome>(WorldBiomeManager.worldEvil).MimicType.Value;
                }
                return value;
            });
        }
    }
}
