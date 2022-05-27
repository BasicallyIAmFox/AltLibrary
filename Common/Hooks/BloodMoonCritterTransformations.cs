using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using MonoMod.Cil;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AltLibrary.Common.Hooks
{
    [Autoload(Side = ModSide.Both)]
    internal class BloodMoonCritterTransformations
    {
        public static void Init()
        {
            IL.Terraria.NPC.AttemptToConvertNPCToEvil += NPC_AttemptToConvertNPCToEvil;
        }

        public static void Unload()
        {
            IL.Terraria.NPC.AttemptToConvertNPCToEvil -= NPC_AttemptToConvertNPCToEvil;
        }

        private static void NPC_AttemptToConvertNPCToEvil(ILContext il)
        {
            ALUtils.ReplaceIDs(il,
                NPCID.CorruptBunny,
                (orig) => (short)(Find<AltBiome>(WorldBiomeManager.WorldEvil).BloodBunny ?? orig),
                (orig) => WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).BloodBunny.HasValue);
            ALUtils.ReplaceIDs(il,
                NPCID.CorruptGoldfish,
                (orig) => (short)(Find<AltBiome>(WorldBiomeManager.WorldEvil).BloodGoldfish ?? orig),
                (orig) => WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).BloodGoldfish.HasValue);
            ALUtils.ReplaceIDs(il,
                NPCID.CorruptPenguin,
                (orig) => (short)(Find<AltBiome>(WorldBiomeManager.WorldEvil).BloodPenguin ?? orig),
                (orig) => WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).BloodPenguin.HasValue);
        }
    }
}
