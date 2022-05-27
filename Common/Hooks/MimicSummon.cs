using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AltLibrary.Common.Hooks
{
    [Autoload(Side = ModSide.Both)]
    internal class MimicSummon
    {
        public static void Init()
        {
            IL.Terraria.NPC.BigMimicSummonCheck += NPC_BigMimicSummonCheck;
        }

        public static void Unload()
        {
            IL.Terraria.NPC.BigMimicSummonCheck -= NPC_BigMimicSummonCheck;
        }

        private static void NPC_BigMimicSummonCheck(ILContext il)
        {
            ILCursor c = new(il);

            ALUtils.ReplaceIDs<int>(il,
                ItemID.LightKey,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldHallow).MimicKeyType ?? orig,
                (orig) => WorldBiomeManager.WorldHallow != "" && Find<AltBiome>(WorldBiomeManager.WorldHallow).MimicKeyType.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                ItemID.NightKey,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldEvil).MimicKeyType ?? orig,
                (orig) => WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).MimicKeyType.HasValue);

            if (!c.TryGotoNext(i => i.MatchLdcI4(475)))
            {
                AltLibrary.Instance.Logger.Info("k $ 1");
                return;
            }

            c.Index++;
            c.Emit(OpCodes.Ldloc, 4);
            c.EmitDelegate<Func<int, int, int>>((value, isGood) =>
            {
                if (WorldBiomeManager.WorldHallow != "" && Find<AltBiome>(WorldBiomeManager.WorldHallow).MimicType.HasValue)
                {
                    return Find<AltBiome>(WorldBiomeManager.WorldHallow).MimicType.Value;
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
                if (WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).MimicType.HasValue)
                {
                    return Find<AltBiome>(WorldBiomeManager.WorldEvil).MimicType.Value;
                }
                return value;
            });
        }
    }
}
