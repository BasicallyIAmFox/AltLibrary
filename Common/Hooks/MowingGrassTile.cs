using AltLibrary.Common.AltBiomes;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;

namespace AltLibrary.Common.Hooks
{
    internal class MowingGrassTile
    {
        public static void Init()
        {
            IL.Terraria.Player.MowGrassTile += Player_MowGrassTile;
        }

        private static void Player_MowGrassTile(ILContext il)
        {
            ILCursor c = new(il);
            if (!c.TryGotoNext(i => i.MatchLdcI4(492)))
                return;
            if (!c.TryGotoNext(i => i.MatchLdloc(2)))
                return;

            c.Index++;
            c.Emit(OpCodes.Ldloc, 1);
            c.EmitDelegate<Func<int, Tile, int>>((mowedTileType, tile) =>
            {
                foreach (AltBiome biome in AltLibrary.biomes)
                {
                    if (biome.BiomeGrass.HasValue && tile.TileType == biome.BiomeGrass && biome.BiomeMowedGrass.HasValue)
                    {
                        return biome.BiomeMowedGrass.Value;
                    }
                }
                return mowedTileType;
            });
            c.Emit(OpCodes.Stloc, 2);
            c.Emit(OpCodes.Ldloc, 2);
        }
    }
}
