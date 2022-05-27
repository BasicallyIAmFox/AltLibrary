using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using MonoMod.Cil;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AltLibrary.Common.Hooks
{
    [Autoload(Side = ModSide.Both)]
    internal class ShadowKeyReplacement
    {
        public static void Init()
        {
            IL.Terraria.WorldGen.AddBuriedChest_int_int_int_bool_int_bool_ushort += WorldGen_AddBuriedChest_int_int_int_bool_int_bool_ushort;
        }

        public static void Unload()
        {
            IL.Terraria.WorldGen.AddBuriedChest_int_int_int_bool_int_bool_ushort -= WorldGen_AddBuriedChest_int_int_int_bool_int_bool_ushort;
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
