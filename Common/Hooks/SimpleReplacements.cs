using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using MonoMod.Cil;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AltLibrary.Common.Hooks
{
    internal class SimpleReplacements
    {
        internal static void Load()
        {
            IL.Terraria.NPC.AttemptToConvertNPCToEvil += NPC_AttemptToConvertNPCToEvil;
            IL.Terraria.NPC.CreateBrickBoxForWallOfFlesh += NPC_CreateBrickBoxForWallOfFlesh;
            IL.Terraria.WorldGen.nearbyChlorophyte += WorldGen_nearbyChlorophyte;
            IL.Terraria.WorldGen.AddBuriedChest_int_int_int_bool_int_bool_ushort += WorldGen_AddBuriedChest_int_int_int_bool_int_bool_ushort;
        }

        internal static void Unload()
        {
            IL.Terraria.NPC.AttemptToConvertNPCToEvil -= NPC_AttemptToConvertNPCToEvil;
            IL.Terraria.NPC.CreateBrickBoxForWallOfFlesh -= NPC_CreateBrickBoxForWallOfFlesh;
            IL.Terraria.WorldGen.nearbyChlorophyte -= WorldGen_nearbyChlorophyte;
            IL.Terraria.WorldGen.AddBuriedChest_int_int_int_bool_int_bool_ushort -= WorldGen_AddBuriedChest_int_int_int_bool_int_bool_ushort;
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

        private static void NPC_CreateBrickBoxForWallOfFlesh(ILContext il)
        {
            ALUtils.ReplaceIDs(il,
                TileID.DemoniteBrick,
                (orig) => (ushort)(Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeOreBrick ?? orig),
                (orig) => WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeOreBrick.HasValue);
        }

        private static void WorldGen_nearbyChlorophyte(ILContext il)
        {
            ALUtils.ReplaceIDs<int>(il,
                TileID.Chlorophyte,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldJungle).BiomeOre ?? orig,
                (orig) => WorldBiomeManager.WorldJungle != "" && Find<AltBiome>(WorldBiomeManager.WorldJungle).BiomeOre.HasValue);
            ALUtils.ReplaceIDs<int>(il,
                TileID.ChlorophyteBrick,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldJungle).BiomeOreBrick ?? orig,
                (orig) => WorldBiomeManager.WorldJungle != "" && Find<AltBiome>(WorldBiomeManager.WorldJungle).BiomeOreBrick.HasValue);
        }

        private static void WorldGen_AddBuriedChest_int_int_int_bool_int_bool_ushort(ILContext il)
        {
            ALUtils.ReplaceIDs<int>(il,
                ItemID.ShadowKey,
                (orig) => Find<AltBiome>(WorldBiomeManager.WorldHell).ShadowKeyAlt ?? orig,
                (orig) => WorldBiomeManager.WorldHell != "" && Find<AltBiome>(WorldBiomeManager.WorldHell).ShadowKeyAlt.HasValue);
        }
    }
}
