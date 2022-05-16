using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using MonoMod.Cil;
using System;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

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
            ALUtils.ReplaceIDs(il,
                NPCID.CorruptBunny,
                (orig) => (short)(Find<AltBiome>(WorldBiomeManager.worldEvil).BloodBunny ?? orig),
                (orig) => WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BloodBunny.HasValue);
            ALUtils.ReplaceIDs(il,
                NPCID.CorruptGoldfish,
                (orig) => (short)(Find<AltBiome>(WorldBiomeManager.worldEvil).BloodGoldfish ?? orig),
                (orig) => WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BloodGoldfish.HasValue);
            ALUtils.ReplaceIDs(il,
                NPCID.CorruptPenguin,
                (orig) => (short)(Find<AltBiome>(WorldBiomeManager.worldEvil).BloodPenguin ?? orig),
                (orig) => WorldBiomeManager.worldEvil != "" && Find<AltBiome>(WorldBiomeManager.worldEvil).BloodPenguin.HasValue);
        }
    }
}
