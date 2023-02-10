using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Attributes;
using AltLibrary.Common.CID;
using AltLibrary.Content.Biomes;
using System;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.ID;

namespace AltLibrary.Common.Hooks;

[LoadableContent(ContentOrder.Content, nameof(Load))]
public static class CIDConvert {
	public static void Load() {
		// TODO: replace with IL.
		ILHelper.On<WorldGen>(nameof(WorldGen.Convert), On_WorldGen_Convert);
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
