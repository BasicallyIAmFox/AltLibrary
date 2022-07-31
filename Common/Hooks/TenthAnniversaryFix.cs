using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks
{
	internal class TenthAnniversaryFix
	{
		public static void Init()
		{
			IL.Terraria.WorldGen.ConvertSkyIslands += WorldGen_ConvertSkyIslands;
			IL.Terraria.WorldGen.IslandHouse += WorldGen_IslandHouse;
		}

		public static void Unload()
		{
			IL.Terraria.WorldGen.ConvertSkyIslands -= WorldGen_ConvertSkyIslands;
			IL.Terraria.WorldGen.IslandHouse -= WorldGen_IslandHouse;
		}

		private static void WorldGen_IslandHouse(ILContext il)
		{
			ILCursor c = new(il);
			if (!c.TryGotoNext(i => i.MatchLdcI4(207)))
			{
				AltLibrary.Instance.Logger.Info("o $ 1");
				return;
			}
			c.Index++;
			c.EmitDelegate<Func<ushort, ushort>>((orig) =>
			{
				if (WorldGen.tenthAnniversaryWorldGen && WorldBiomeManager.WorldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldHallow).FountainTile.HasValue)
				{
					return (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.WorldHallow).FountainTile.Value;
				}
				return orig;
			});
			if (!c.TryGotoNext(i => i.MatchLdarg(2)))
			{
				AltLibrary.Instance.Logger.Info("o $ 2");
				return;
			}
			c.Index++;
			c.EmitDelegate<Func<int, int>>((orig) =>
			{
				if (WorldGen.tenthAnniversaryWorldGen && WorldBiomeManager.WorldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldHallow).FountainTileStyle.HasValue)
				{
					return ModContent.Find<AltBiome>(WorldBiomeManager.WorldHallow).FountainTileStyle.Value;
				}
				return orig;
			});
			if (!c.TryGotoNext(i => i.MatchCall(out _)))
			{
				AltLibrary.Instance.Logger.Info("o $ 3");
				return;
			}
			c.Remove();
			c.EmitDelegate<Action<int, int, ushort, int>>((x, y, type, style) =>
			{
				short frameX = 0;
				short frameY = 0;
				if (WorldGen.tenthAnniversaryWorldGen && WorldBiomeManager.WorldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldHallow).FountainActiveFrameX.HasValue)
				{
					frameX = (short)ModContent.Find<AltBiome>(WorldBiomeManager.WorldHallow).FountainActiveFrameX.Value;
				}
				if (WorldGen.tenthAnniversaryWorldGen && WorldBiomeManager.WorldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldHallow).FountainActiveFrameY.HasValue)
				{
					frameY = (short)ModContent.Find<AltBiome>(WorldBiomeManager.WorldHallow).FountainActiveFrameY.Value;
				}
				UselessCallThatDoesTechnicallyNothing(x, y, type, style, frameX, frameY);
			});

			if (!c.TryGotoNext(i => i.MatchCall(out _)))
			{
				AltLibrary.Instance.Logger.Info("o $ 4");
				return;
			}
			MethodReference switchFountains = null;
			if (!c.TryGotoNext(i => i.MatchCall(out switchFountains)))
			{
				AltLibrary.Instance.Logger.Info("o $ 5");
				return;
			}
			ILLabel label = il.DefineLabel();

			c.Remove();
			c.EmitDelegate(() => WorldGen.tenthAnniversaryWorldGen ? WorldBiomeManager.WorldHallow : "");
			c.Emit(OpCodes.Ldstr, "");
			c.Emit(OpCodes.Call, typeof(string).GetMethod("op_Equality", new Type[] { typeof(string), typeof(string) }));
			c.Emit(OpCodes.Brfalse_S, label);
			c.Emit(OpCodes.Call, switchFountains);
			c.MarkLabel(label);
		}

		internal static void UselessCallThatDoesTechnicallyNothing(int x, int y, ushort type, int style = 0, short frameX = 0, short frameY = 0)
		{
			if (type != 0)
			{
				WorldGen.Place2xX(x, y, type, style);
				if (type != 207 && Main.tile[x, y].HasTile)
				{
					Main.tile[x, y].TileFrameX = frameX;
					Main.tile[x, y].TileFrameY = frameY;
				}
			}
		}

		private static void WorldGen_ConvertSkyIslands(ILContext il)
		{
			ILCursor c = new(il);
			if (!c.TryGotoNext(i => i.MatchLdcI4(109)))
			{
				AltLibrary.Instance.Logger.Info("p $ 1");
				return;
			}

			ALUtils.ReplaceIDs<int>(il,
				TileID.HallowedGrass,
				(orig) => ModContent.Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeGrass ?? orig,
				(orig) => WorldBiomeManager.WorldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeGrass.HasValue);

			if (!c.TryGotoNext(i => i.MatchCall<WorldGen>(nameof(WorldGen.Convert))))
			{
				AltLibrary.Instance.Logger.Info("p $ 2");
				return;
			}
			c.Index++;
			c.Emit(OpCodes.Ldloc, 4);
			c.Emit(OpCodes.Ldloc, 5);
			c.EmitDelegate<Action<int, int>>((i, j) =>
			{
				if (WorldBiomeManager.WorldHallow != "" && ModContent.Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeGrass.HasValue)
				{
					int size = 1;
					for (int l = i - size; l <= i + size; l++)
					{
						for (int k = j - size; k <= j + size; k++)
						{
							Tile tile = Main.tile[l, k];
							int type = tile.TileType;
							if (TileID.Sets.Conversion.Grass[type] && type == 109)
							{
								tile = Main.tile[l, k];
								tile.TileType = (ushort)ModContent.Find<AltBiome>(WorldBiomeManager.WorldHallow).BiomeGrass.Value;
								WorldGen.SquareTileFrame(l, k, true);
								NetMessage.SendTileSquare(-1, l, k, TileChangeType.None);
							}
						}
					}
				}
			});
		}
	}
}
