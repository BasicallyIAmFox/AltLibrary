using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Attributes;
using AltLibrary.Common.CID;
using AltLibrary.Common.IL;
using AltLibrary.Content.Biomes;
using MonoMod.Cil;
using System;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.ID;

namespace AltLibrary.Common.Hooks;

[LoadableContent(ContentOrder.Content, nameof(Load))]
public static class CIDConvert {
	public static void Load() {
		ILHelper.IL<WorldGen>(nameof(WorldGen.Convert), (ILContext il) => {
			var c = new ILCursor(il);

			const int conversionTypeIndex = 2;

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
					new(ParamRef.TargetParameter, conversionTypeIndex)
				}
			});

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
			c.GotoNext(MoveType.After,
				i => i.MatchLdloca(tileIndex),
				i => i.MatchCall<Tile>("get_wall"),
				i => i.MatchLdindU2(),
				i => i.MatchStloc(out wallIndex));

			c.EmitDelegateBody(static (ref Tile tile, int conversionType, int tileType, int wallType, int k, int l) => {
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

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	private static void On_WorldGen_Convert(On_WorldGen.orig_Convert orig, int i, int j, int conversionType, int size) {
		conversionType = conversionType switch {
			BiomeConversionID.Purity => ConversionInheritanceData.DecleminationId,
			BiomeConversionID.Sand => ConversionInheritanceData.YellowSolutionId,
			BiomeConversionID.Dirt => ConversionInheritanceData.BrownSolutionId,
			BiomeConversionID.Snow => ConversionInheritanceData.WhiteSolutionId,
			BiomeConversionID.GlowingMushroom => ConversionInheritanceData.DarkBlueSolutionId,

			BiomeConversionID.Corruption => ConversionInheritanceData.GetConversionIdOf<CorruptBiome>(),
			BiomeConversionID.Crimson => ConversionInheritanceData.GetConversionIdOf<CrimsonBiome>(),
			BiomeConversionID.Hallow => ConversionInheritanceData.GetConversionIdOf<HallowBiome>(),
			_ => conversionType
		};

		for (int k = i - size; k <= i + size; k++) {
			for (int l = j - size; l <= j + size; l++) {
				if (!WorldGen.InWorld(k, l, 1) || Math.Abs(k - i) + Math.Abs(l - j) >= 6) {
					continue;
				}

				Tile tile = Main.tile[k, l];
				int newTile = ConversionInheritanceDatabase.GetConvertedTile(conversionType, tile.TileType);
				int newWall = ConversionInheritanceDatabase.GetConvertedWall(conversionType, tile.WallType);

				bool transformedAny = false;
				bool breakNewTile = newTile == ConversionInheritanceData.Break;
				bool breakNewWall = newWall == ConversionInheritanceData.Break;
				if (breakNewTile || breakNewWall) {
					if (breakNewTile) {
						WorldGen.KillTile(k, l, false, false, false);
					}
					if (breakNewWall) {
						WorldGen.KillWall(k, l, false);
					}

					if (Main.netMode == NetmodeID.MultiplayerClient) {
						NetMessage.SendData(MessageID.TileManipulation, number: k, number2: j);
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
					WorldGen.SquareTileFrame(k, l, true);
					NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
				}
			}
		}
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
				int newTile = ConversionInheritanceDatabase.GetConvertedTile(5 + biome.Type, tile.TileType);
				int newWall = ConversionInheritanceDatabase.GetConvertedWall(5 + biome.Type, tile.WallType);

				bool breakNewTile = newTile == ConversionInheritanceData.Break;
				bool breakNewWall = newWall == ConversionInheritanceData.Break;
				if (breakNewTile || breakNewWall) {
					if (breakNewTile) {
						WorldGen.KillTile(k, l, false, false, false);
					}
					if (breakNewWall) {
						WorldGen.KillWall(k, l, false);
					}

					if (Main.netMode == NetmodeID.MultiplayerClient) {
						NetMessage.SendData(MessageID.TileManipulation, number: k, number2: j);
					}
				}
				else {
					bool transformedAny = false;

					if (newTile != ConversionInheritanceData.Keep && newTile != tile.TileType) {
						tile.TileType = (ushort)newTile;
						transformedAny = true;
					}
					if (newWall != ConversionInheritanceData.Keep && newWall != tile.WallType) {
						tile.WallType = (ushort)newWall;
						transformedAny = true;
					}

					if (transformedAny) {
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
					}
				}
			}
		}
		return;
	}
}
