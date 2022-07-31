using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks
{
	/// <summary>
	/// TODO:
	/// make hell chest alts actually gen...
	/// </summary>
	internal class DungeonChests
	{
		internal static int hellChestIndex;

		public static void Init()
		{
			hellChestIndex = -1;
			IL.Terraria.WorldGen.MakeDungeon += WorldGen_MakeDungeon;
		}

		public static void Unload()
		{
			IL.Terraria.WorldGen.MakeDungeon -= WorldGen_MakeDungeon;
			hellChestIndex = 0;
		}

		private static void WorldGen_MakeDungeon(ILContext il)
		{
			ILCursor c = new(il);
			if (!c.TryGotoNext(i => i.MatchCall<WorldGen>(nameof(WorldGen.AddBuriedChest))))
			{
				AltLibrary.Instance.Logger.Info("b $ 1");
				return;
			}
			if (!c.TryGotoPrev(i => i.MatchStloc(15)))
			{
				AltLibrary.Instance.Logger.Info("b $ 2");
				return;
			}

			c.Index++;
			c.Emit(OpCodes.Ldloc, 15);
			c.EmitDelegate<Func<int, int>>((orig) =>
			{
				hellChestIndex = -1;
				if (WorldBiomeManager.WorldHell != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).BiomeChestTile.HasValue)
				{
					hellChestIndex = orig + 1;
					return orig + 1;
				}
				return orig;
			});
			c.Emit(OpCodes.Stloc, 15);

			if (!c.TryGotoNext(i => i.MatchCall<WorldGen>(nameof(WorldGen.AddBuriedChest))))
			{
				AltLibrary.Instance.Logger.Info("b $ 3");
				return;
			}
			if (!c.TryGotoPrev(i => i.MatchLdloc(98)))
			{
				AltLibrary.Instance.Logger.Info("b $ 4");
				return;
			}

			c.Index++;
			c.Emit(OpCodes.Ldloc, 93);
			c.Emit(OpCodes.Ldc_I4, hellChestIndex);
			c.EmitDelegate<Func<int, int, int, int>>((contain, chests, hellChestIndex) =>
			{
				if (chests == 0 && WorldBiomeManager.WorldJungle != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldJungle).BiomeChestItem.HasValue)
				{
					return ModContent.Find<AltBiome>(WorldBiomeManager.WorldJungle).BiomeChestItem.Value;
				}
				if (chests == 2 && WorldBiomeManager.WorldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeChestItem.HasValue)
				{
					return ModContent.Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeChestItem.Value;
				}
				if ((chests == 1 || chests == 5) && WorldBiomeManager.WorldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeChestItem.HasValue)
				{
					return ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeChestItem.Value;
				}
				if (chests == hellChestIndex && WorldBiomeManager.WorldHell != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).BiomeChestItem.HasValue)
				{
					return ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).BiomeChestItem.Value;
				}
				return contain;
			});

			if (!c.TryGotoNext(i => i.MatchLdloc(99)))
			{
				AltLibrary.Instance.Logger.Info("b $ 5");
				return;
			}

			c.Index++;
			c.Emit(OpCodes.Ldloc, 93);
			c.Emit(OpCodes.Ldc_I4, hellChestIndex);
			c.EmitDelegate<Func<int, int, int, int>>((style, chests, hellChestIndex) =>
			{
				if (chests == 0 && WorldBiomeManager.WorldJungle != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldJungle).BiomeChestTileStyle.HasValue)
				{
					style = ModContent.Find<AltBiome>(WorldBiomeManager.WorldJungle).BiomeChestTileStyle.Value;
				}
				if (chests == 2 && WorldBiomeManager.WorldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeChestTileStyle.HasValue)
				{
					style = ModContent.Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeChestTileStyle.Value;
				}
				if ((chests == 1 || chests == 5) && WorldBiomeManager.WorldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeChestTileStyle.HasValue)
				{
					style = ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeChestTileStyle.Value;
				}
				if (chests == hellChestIndex && WorldBiomeManager.WorldHell != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).BiomeChestTileStyle.HasValue)
				{
					return ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).BiomeChestTileStyle.Value;
				}
				return style;
			});

			if (!c.TryGotoNext(i => i.MatchLdloc(97)))
			{
				AltLibrary.Instance.Logger.Info("b $ 6");
				return;
			}

			c.Index++;
			c.Emit(OpCodes.Ldloc, 93);
			c.Emit(OpCodes.Ldc_I4, hellChestIndex);
			c.EmitDelegate<Func<int, int, int, int>>((chestTileType, chests, hellChestIndex) =>
			{
				if (chests == 0 && WorldBiomeManager.WorldJungle != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldJungle).BiomeChestTile.HasValue)
				{
					return ModContent.Find<AltBiome>(WorldBiomeManager.WorldJungle).BiomeChestTile.Value;
				}
				if (chests == 2 && WorldBiomeManager.WorldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeChestTile.HasValue)
				{
					return ModContent.Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeChestTile.Value;
				}
				if ((chests == 1 || chests == 5) && WorldBiomeManager.WorldEvil != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeChestTile.HasValue)
				{
					return ModContent.Find<AltBiome>(WorldBiomeManager.WorldEvil).BiomeChestTile.Value;
				}
				if (chests == hellChestIndex && WorldBiomeManager.WorldHell != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).BiomeChestTile.HasValue)
				{
					return ModContent.Find<AltBiome>(WorldBiomeManager.WorldHell).BiomeChestTile.Value;
				}
				return chestTileType;
			});
		}
	}
}
