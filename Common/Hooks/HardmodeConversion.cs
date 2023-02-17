using AltLibrary.Common.Attributes;
using AltLibrary.Common.CID;
using AltLibrary.Common.IL;
using AltLibrary.Common.IO;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.Utilities;
using System;
using Terraria;
using Terraria.Utilities;

namespace AltLibrary.Common.Hooks;

[LoadableContent(ContentOrder.Content, nameof(Load))]
public static class HardmodeConversion {
	private static void Load() {
		ILHelper.IL<WorldGen>(nameof(WorldGen.GERunner), (ILContext il) => {
			var c = new ILCursor(il);

			var startIndex = c.Index;
			var endIndex = startIndex;

			var biomeConvIdIndex = c.AddVariable<int>();

			var xIndex = 0;
			var yIndex = 0;
			var goodIndex = 0;

			/*
			// if (Math.Abs((double)m - vector2D.X) + Math.Abs((double)l - vector2D.Y) < (double)num15 * 0.5 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015))
			IL_01de: ldloc.s 15
			IL_01e0: conv.r8
			IL_01e1: ldloc.s 5
			IL_01e3: ldfld float64 [ReLogic]ReLogic.Utilities.Vector2D::X
			IL_01e8: sub
			IL_01e9: call float64 [System.Runtime]System.Math::Abs(float64)
			IL_01ee: ldloc.s 16
			IL_01f0: conv.r8
			IL_01f1: ldloc.s 5
			IL_01f3: ldfld float64 [ReLogic]ReLogic.Utilities.Vector2D::Y
			IL_01f8: sub
			IL_01f9: call float64 [System.Runtime]System.Math::Abs(float64)
			IL_01fe: add
			IL_01ff: ldloc.2
			IL_0200: conv.r8
			IL_0201: ldc.r8 0.5
			IL_020a: mul
			IL_020b: ldc.r8 1
			IL_0214: call class Terraria.Utilities.UnifiedRandom Terraria.WorldGen::get_genRand()
			IL_0219: ldc.i4.s -10
			IL_021b: ldc.i4.s 11
			IL_021d: callvirt instance int32 Terraria.Utilities.UnifiedRandom::Next(int32, int32)
			IL_0222: conv.r8
			IL_0223: ldc.r8 0.015
			IL_022c: mul
			IL_022d: add
			IL_022e: mul
			IL_022f: bge.un IL_1051

			// if (good)
			IL_0234: ldarg.s good
			// (no C# code)
			IL_0236: brfalse IL_07ba
			*/
			ILLabel incLoopLabel = null;
			c.GotoNext(MoveType.After,
				i => i.MatchLdloc(out xIndex),
				i => i.MatchConvR8(),
				i => i.MatchLdloc(out _),
				i => i.MatchLdfld<Vector2D>(nameof(Vector2D.X)),
				i => i.MatchSub(),
				i => i.MatchCall(typeof(Math), nameof(Math.Abs)),
				i => i.MatchLdloc(out yIndex),
				i => i.MatchConvR8(),
				i => i.MatchLdloc(out _),
				i => i.MatchLdfld<Vector2D>(nameof(Vector2D.Y)),
				i => i.MatchSub(),
				i => i.MatchCall(typeof(Math), nameof(Math.Abs)),
				i => i.MatchAdd(),
				i => i.MatchLdloc(out _),
				i => i.MatchConvR8(),
				i => i.MatchLdcR8(out _),
				i => i.MatchMul(),
				i => i.MatchLdcR8(out _),
				i => i.MatchCall<WorldGen>("get_genRand"),
				i => i.MatchLdcI4(out _),
				i => i.MatchLdcI4(out _),
				i => i.MatchCallvirt<UnifiedRandom>(nameof(UnifiedRandom.Next)),
				i => i.MatchConvR8(),
				i => i.MatchLdcR8(out _),
				i => i.MatchMul(),
				i => i.MatchAdd(),
				i => i.MatchMul(),
				i => i.MatchBgeUn(out incLoopLabel),
				
				i => i.MatchLdarg(out goodIndex),
				i => i.MatchBrfalse(out _));

			endIndex = c.Index;
			c.Index = startIndex;

			c.Emit(OpCodes.Ldarg, goodIndex);
			c.EmitDelegate(static (bool good) => ConversionInheritanceData.GetConversionIdOf(good ? WorldDataManager.GetHallow() : WorldDataManager.GetEvil()));
			c.Emit(OpCodes.Stloc, biomeConvIdIndex);

			c.Index = endIndex;

			c.Emit(OpCodes.Ldloc, xIndex);
			c.Emit(OpCodes.Ldloc, yIndex);
			c.Emit(OpCodes.Ldloc, biomeConvIdIndex);
			c.EmitDelegate(static (int x, int y, int conversionType) => {
				Tile tile = Main.tile[x, y];

				var convType = ConversionInheritanceDatabase.GetConvertedTile(conversionType, tile.TileType);
				var convWall = ConversionInheritanceDatabase.GetConvertedWall(conversionType, tile.WallType);

				var transformedAny = false;
				if (convType >= 0 && convType != tile.TileType) {
					tile.TileType = (ushort)convType;
					transformedAny = true;
				}
				if (convWall >= 0 && convWall != tile.WallType) {
					tile.WallType = (ushort)convWall;
					transformedAny = true;
				}
				if (transformedAny) {
					WorldGen.SquareTileFrame(x, y);
				}
			});

			c.Emit(OpCodes.Ldc_I4, 1);
			c.Emit(OpCodes.Ldc_I4, 0);
			c.Emit(OpCodes.Bne_Un, incLoopLabel);
		});
	}
}
