using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using MonoMod.Cil;
using System;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks
{
    internal class BloodMoonCritterTransformations
    {
        public static void Init()
        {
            IL.Terraria.NPC.AttemptToConvertNPCToEvil += NPC_AttemptToConvertNPCToEvil;
        }

        private static void NPC_AttemptToConvertNPCToEvil(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchLdcI4(47)))
                return;

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BloodBunny.HasValue)
                {
                    return ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BloodBunny.Value;
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(57)))
                return;

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BloodGoldfish.HasValue)
                {
                    return ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BloodGoldfish.Value;
                }
                return orig;
            });

            if (!c.TryGotoNext(i => i.MatchLdcI4(168)))
                return;

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BloodPenguin.HasValue)
                {
                    return ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BloodPenguin.Value;
                }
                return orig;
            });
        }
    }
}
