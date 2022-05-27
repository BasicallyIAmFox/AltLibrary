using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using MonoMod.Cil;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace AltLibrary.Common.Hooks
{
    [Autoload(Side = ModSide.Both)]
    internal class EvilWofBox
    {
        public static void Init()
        {
            IL.Terraria.NPC.CreateBrickBoxForWallOfFlesh += NPC_CreateBrickBoxForWallOfFlesh;
        }

        public static void Unload()
        {
            IL.Terraria.NPC.CreateBrickBoxForWallOfFlesh -= NPC_CreateBrickBoxForWallOfFlesh;
        }

        private static void NPC_CreateBrickBoxForWallOfFlesh(ILContext il)
        {
            ALUtils.ReplaceIDs(il,
                TileID.DemoniteBrick,
                (orig) => (ushort)(Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeOreBrick ?? orig),
                (orig) => WorldBiomeManager.WorldEvil != "" && Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeOreBrick.HasValue);
        }
    }
}
