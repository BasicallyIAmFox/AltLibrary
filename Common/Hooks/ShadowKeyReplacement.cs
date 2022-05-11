using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using MonoMod.Cil;
using System;
using Terraria.ModLoader;

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
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchLdcI4(329)))
                return;

            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldBiomeManager.worldHell != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldHell).ShadowKeyAlt.HasValue)
                {
                    return ModContent.Find<AltBiome>(WorldBiomeManager.worldHell).ShadowKeyAlt.Value;
                }
                return orig;
            });
        }
    }
}
