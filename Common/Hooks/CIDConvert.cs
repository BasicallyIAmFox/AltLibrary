using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Attributes;
using AltLibrary.Common.CID;
using AltLibrary.Common.IL;
using AltLibrary.Content.Biomes;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks;

[LoadableContent(ContentOrder.Content, nameof(Load))]
public static class CIDConvert {
	public static void Load() {
		ILHelper.IL<WorldGen>(nameof(WorldGen.Convert), (ILContext il) => {
			var c = new ILCursor(il);

			c.EmitDelegateBody((int conversionType) => conversionType = conversionType switch {
				BiomeConversionID.Purity => ConversionInheritanceData.DecleminationId,
				BiomeConversionID.Sand => ConversionInheritanceData.YellowSolutionId,
				BiomeConversionID.Dirt => ConversionInheritanceData.BrownSolutionId,
				BiomeConversionID.Snow => ConversionInheritanceData.WhiteSolutionId,
				BiomeConversionID.GlowingMushroom => ConversionInheritanceData.DarkBlueSolutionId,

				BiomeConversionID.Corruption => ConversionInheritanceData.GetConversionIdOf<CorruptBiome>(),
				BiomeConversionID.Crimson => ConversionInheritanceData.GetConversionIdOf<CrimsonBiome>(),
				BiomeConversionID.Hallow => ConversionInheritanceData.GetConversionIdOf<HallowBiome>(),
				_ => conversionType
			}, new() {
				ParameterTypes = new ParameterReferenceType[] {
					new(ParamRef.TargetParameter, 2) // conversionType
				}
			});

			var conversionTypeIndex = 0;
			var xIndex = 0;
			var yIndex = 0;
			var tileIndex = 0;
			var typeIndex = 0;
			var wallIndex = 0;

			/*
			// Tile tile = Main.tile[k, l];
			IL_0044: ldsflda valuetype Terraria.Tilemap Terraria.Main::tile
			IL_0049: ldloc.0
			IL_004a: ldloc.1
			IL_004b: call instance valuetype Terraria.Tile Terraria.Tilemap::get_Item(int32, int32)
			IL_0050: stloc.2
			// int type = *(ushort*)tile.type;
			IL_0051: ldloca.s 2
			IL_0053: call instance uint16& Terraria.Tile::get_type()
			IL_0058: ldind.u2
			IL_0059: stloc.3
			// int wall = *(ushort*)tile.wall;
			IL_005a: ldloca.s 2
			IL_005c: call instance uint16& Terraria.Tile::get_wall()
			IL_0061: ldind.u2
			IL_0062: stloc.s 4
			// switch (conversionType)
			IL_0056: ldarg.2
			// (no C# code)
			IL_0057: ldc.i4.1
			IL_0058: sub
			IL_0059: switch (IL_080a, IL_0408, IL_0b63, IL_007f, IL_0bfb, IL_0e12, IL_0fe6)
			 */
			c.GotoNext(
				i => i.MatchLdsflda(out _),
				i => i.MatchLdloc(out xIndex),
				i => i.MatchLdloc(out yIndex),
				i => i.MatchCall<Tilemap>("get_Item"),
				i => i.MatchStloc(out tileIndex));
			c.GotoNext(
				i => i.MatchLdloca(tileIndex),
				i => i.MatchCall<Tile>("get_type"),
				i => i.MatchLdindU2(),
				i => i.MatchStloc(out typeIndex));
			c.GotoNext(
				i => i.MatchLdloca(tileIndex),
				i => i.MatchCall<Tile>("get_wall"),
				i => i.MatchLdindU2(),
				i => i.MatchStloc(out wallIndex));
			c.GotoNext(
				i => i.MatchLdarg(out conversionTypeIndex),
				i => i.MatchLdcI4(out _),
				i => i.MatchSub(),
				i => i.MatchSwitch(out _));

			var skipVanilla = c.DefineLabel();
			c.Emit(OpCodes.Br_S, skipVanilla);

			var tempIndex2 = 0;
			/*
			// NetMessage.SendData(17, -1, -1, null, 0, (float)l, (float)k, 0f, 0, 0, 0);
			IL_17f6: ldc.i4.s 17
			IL_17f8: ldc.i4.m1
			IL_17f9: ldc.i4.m1
			IL_17fa: ldnull
			IL_17fb: ldc.i4.0
			IL_17fc: ldloc.0
			IL_17fd: conv.r4
			IL_17fe: ldloc.1
			IL_17ff: conv.r4
			IL_1800: ldc.r4 0.0
			IL_1805: ldc.i4.0
			IL_1806: ldc.i4.0
			IL_1807: ldc.i4.0
			IL_1808: call void Terraria.NetMessage::SendData(int32, int32, int32, class Terraria.Localization.NetworkText, int32, float32, float32, float32, int32, int32, int32)

			// for (int k = j - size; k <= j + size; k++)
			IL_180d: ldloc.1
			IL_180e: ldc.i4.1
			IL_180f: add
			IL_1810: stloc.1
			 */
			c.GotoNext(MoveType.After,
				i => i.MatchLdcI4(out _),
				i => i.MatchLdcI4(out _),
				i => i.MatchLdcI4(out _),
				i => i.MatchLdnull(),
				i => i.MatchLdcI4(out _),
				i => i.MatchLdloc(out _),
				i => i.MatchConvR4(),
				i => i.MatchLdloc(out tempIndex2),
				i => i.MatchConvR4(),
				i => i.MatchLdcR4(out _),
				i => i.MatchLdcI4(out _),
				i => i.MatchLdcI4(out _),
				i => i.MatchLdcI4(out _),
				i => i.MatchCall<NetMessage>(nameof(NetMessage.SendData)),

				i => i.MatchLdloc(tempIndex2),
				i => i.MatchLdcI4(out _),
				i => i.MatchAdd(),
				i => i.MatchStloc(tempIndex2)
				);

			c.Index -= 4;
			c.EmitDelegateBody(static (Tile tile, int conversionType, int tileType, int wallType, int k, int l) => {
				int newTile = ConversionInheritanceDatabase.GetConvertedTile(conversionType, tileType);
				int newWall = ConversionInheritanceDatabase.GetConvertedWall(conversionType, wallType);

				bool transformedAny = false;
				bool breakNewTile = newTile == ConversionInheritanceData.Break;
				bool breakNewWall = newWall == ConversionInheritanceData.Break;
				if (breakNewTile || breakNewWall) {
					if (breakNewTile) {
						WorldGen.KillTile(k, l);
					}
					if (breakNewWall) {
						WorldGen.KillWall(k, l);
					}

					if (Main.netMode == NetmodeID.MultiplayerClient) {
						NetMessage.SendData(MessageID.TileManipulation, number: k, number2: l);
					}
				}

				if (newTile >= 0 && newTile != tile.TileType) {
					tile.TileType = (ushort)newTile;
					transformedAny = true;
				}
				if (newWall >= 0 && newWall != tile.WallType) {
					tile.WallType = (ushort)newWall;
					transformedAny = true;
				}
				if (transformedAny) {
					WorldGen.SquareTileFrame(k, l);
					NetMessage.SendTileSquare(-1, k, l);
				}
			}, new() {
				ParameterTypes = new ParameterReferenceType[] {
					new(ParamRef.TargetLocal, tileIndex),
					new(ParamRef.TargetParameter, conversionTypeIndex),
					new(ParamRef.TargetLocal, typeIndex),
					new(ParamRef.TargetLocal, wallIndex),
					new(ParamRef.TargetLocal, xIndex),
					new(ParamRef.TargetLocal, yIndex),
				}
			});
		});
	}

	public static void Convert<T>(int i, int j, int size = 4) where T : class, IAltBiome {
		Convert(ModContent.GetInstance<T>(), i, j, size);
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public static void Convert(IAltBiome biome, int i, int j, int size = 4) {
		if (biome is null)
			throw new ArgumentNullException(nameof(biome), "Can't be null!");

		for (int k = i - size; k <= i + size; k++) {
			for (int l = j - size; l <= j + size; l++) {
				if (!WorldGen.InWorld(k, l, 1) || Math.Abs(k - i) + Math.Abs(l - j) >= 6) {
					continue;
				}

				Tile tile = Main.tile[k, l];
				int newTile = ConversionInheritanceDatabase.GetConvertedTile(biome, tile.TileType);
				int newWall = ConversionInheritanceDatabase.GetConvertedWall(biome, tile.WallType);

				bool transformedAny = false;
				bool breakNewTile = newTile == ConversionInheritanceData.Break;
				bool breakNewWall = newWall == ConversionInheritanceData.Break;
				if (breakNewTile || breakNewWall) {
					if (breakNewTile) {
						WorldGen.KillTile(k, l);
					}
					if (breakNewWall) {
						WorldGen.KillWall(k, l);
					}

					if (Main.netMode == NetmodeID.MultiplayerClient) {
						NetMessage.SendData(MessageID.TileManipulation, number: k, number2: l);
					}
				}

				if (newTile >= 0 && newTile != tile.TileType) {
					tile.TileType = (ushort)newTile;
					transformedAny = true;
				}
				if (newWall >= 0 && newWall != tile.WallType) {
					tile.WallType = (ushort)newWall;
					transformedAny = true;
				}
				if (transformedAny) {
					WorldGen.SquareTileFrame(k, l);
					NetMessage.SendTileSquare(-1, k, l);
				}
			}
		}
	}
}
