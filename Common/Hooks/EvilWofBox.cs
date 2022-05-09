using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using MonoMod.Cil;
using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks
{
    internal class EvilWofBox
    {
        public static void Init()
        {
            IL.Terraria.NPC.CreateBrickBoxForWallOfFlesh += NPC_CreateBrickBoxForWallOfFlesh;
        }

        private static void NPC_CreateBrickBoxForWallOfFlesh(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchLdcI4(TileID.DemoniteBrick)))
                return;
            c.Index++;
            c.EmitDelegate<Func<int, int>>((orig) =>
            {
                if (WorldBiomeManager.worldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeOreBrick.HasValue)
                {
                    return ModContent.Find<AltBiome>(WorldBiomeManager.worldEvil).BiomeOreBrick.Value;
                }
                return orig;
            });
        }
    }
}
