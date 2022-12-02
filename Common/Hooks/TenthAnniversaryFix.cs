using AltLibrary.Common.AltBiomes;
using AltLibrary.Common.Systems;
using AltLibrary.Core;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Net.Mime;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks
{
	internal class TenthAnniversaryFix
	{
		public static void Init()
		{
			IL_WorldGen.ConvertSkyIslands += WorldGen_ConvertSkyIslands;
			IL_WorldGen.IslandHouse += WorldGen_IslandHouse;
		}

		public static void Unload()
		{
			IL_WorldGen.ConvertSkyIslands -= WorldGen_ConvertSkyIslands;
			IL_WorldGen.IslandHouse -= WorldGen_IslandHouse;
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
			ILLabel label = c.DefineLabel();

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
			
			try
			{
				int k = 0;
				int j = 0;
				int size = 0;

				c.GotoNext(i => i.MatchLdloc(out k),
					i => i.MatchLdloc(out j),
					i => i.MatchLdarg(0),
					i => i.MatchLdcI4(out size),
					i => i.MatchCall<WorldGen>(nameof(WorldGen.Convert)));

				var skip = c.DefineLabel();
				var def = c.DefineLabel();

				c.Emit(OpCodes.Ldarg, 0);
				c.EmitDelegate((int convertType) => convertType == 2 && WorldBiomeManager.WorldHallow == string.Empty);
				c.Emit(OpCodes.Brtrue_S, def);

				c.Emit(OpCodes.Ldarg, 0);
				c.EmitDelegate((int convertType) => convertType == 2 ? WorldBiomeManager.WorldHallow : string.Empty);
				c.Emit(OpCodes.Ldloc, k);
				c.Emit(OpCodes.Ldloc, j);
				c.Emit(OpCodes.Ldc_I4, size);
				c.EmitDelegate<Action<string, int, int, int>>(ALConvert.Convert);

				c.Emit(OpCodes.Nop);
				c.Emit(OpCodes.Br_S, skip);

				c.MarkLabel(def);
				c.Index += 5;
				c.MarkLabel(skip);
			}
			catch
			{
			}
		}
	}
}
