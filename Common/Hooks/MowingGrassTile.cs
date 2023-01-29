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
			EditsHelper.IL<Player>(nameof(Player.MowGrassTile), Player_MowGrassTile);
		}

		public static void Unload()
		{
		}

		private static void Player_MowGrassTile(ILContext il)
		{
			ILCursor c = new(il);
			c.GotoNext(i => i.MatchLdcI4(492));
			c.GotoNext(MoveType.After, i => i.MatchLdloc(2));

			c.Emit(OpCodes.Ldloc, 1);
			c.EmitDelegate<Func<int, Tile, int>>((mowedTileType, tile) =>
			{
				foreach (AltBiome biome in AltLibrary.Biomes)
				{
					if (biome.BiomeGrass.HasValue && tile.TileType == biome.BiomeGrass.Value && biome.BiomeMowedGrass.HasValue)
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
