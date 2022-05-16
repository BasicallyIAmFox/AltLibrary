using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using MonoMod.Cil;
using System;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace AltLibrary.Common.Hooks
{
    internal class ShadowKeyReplacement
    {
        public static void Init()
        {
            IL.Terraria.WorldGen.AddBuriedChest_int_int_int_bool_int_bool_ushort += WorldGen_AddBuriedChest_int_int_int_bool_int_bool_ushort;
        }

        private static void WorldGen_AddBuriedChest_int_int_int_bool_int_bool_ushort(ILContext il)
        {
            ALUtils.ReplaceIDs<int>(il,
                ItemID.ShadowKey,
                (orig) => Find<AltBiome>(WorldBiomeManager.worldHell).ShadowKeyAlt ?? orig,
                (orig) => WorldBiomeManager.worldHell != "" && Find<AltBiome>(WorldBiomeManager.worldHell).ShadowKeyAlt.HasValue);
        }
    }
}
