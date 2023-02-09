using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Attributes;
using AltLibrary.Common.CID;
using AltLibrary.Content.Biomes;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.Hooks;

[LoadableContent(ContentOrder.Content, nameof(Load))]
public static class CIDConvert {
	public static void Load() {
		ILHelper.On<WorldGen>(nameof(WorldGen.Convert), On_WorldGen_Convert);
	}

	private static void On_WorldGen_Convert(On_WorldGen.orig_Convert orig, int i, int j, int conversionType, int size) {
		conversionType = conversionType switch {
			BiomeConversionID.Purity => CIData.DecleminationId,
			BiomeConversionID.Sand => CIData.YellowSolutionId,
			BiomeConversionID.Dirt => CIData.BrownSolutionId,
			BiomeConversionID.Snow => CIData.WhiteSolutionId,
			BiomeConversionID.GlowingMushroom => CIData.DarkBlueSolutionId,

			BiomeConversionID.Corruption => 5 + ModContent.GetInstance<CorruptBiome>().Type,
			BiomeConversionID.Crimson => 5 + ModContent.GetInstance<CrimsonBiome>().Type,
			BiomeConversionID.Hallow => 5 + ModContent.GetInstance<HallowBiome>().Type,
			_ => conversionType
		};

		for (ushort k = (ushort)(i - size); k <= i + size; k++) {
			for (ushort l = (ushort)(j - size); l <= j + size; l++) {
				if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < 6) {
					Tile tile = Main.tile[k, l];
					int newTile = CIDatabase.GetConvertedTile(conversionType, tile.TileType);
					if (newTile == CIData.Break) {
						WorldGen.KillTile(k, l, false, false, false);
						if (Main.netMode == NetmodeID.MultiplayerClient) {
							NetMessage.SendData(MessageID.TileManipulation, number: k, number2: j);
						}
					}
					else if (newTile != CIData.Keep && newTile != tile.TileType) {
						tile.TileType = (ushort)newTile;

						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
					}

					int newWall = CIDatabase.GetConvertedWall(conversionType, tile.WallFrameNumber);
					if (newWall == CIData.Break) {
						WorldGen.KillWall(k, l, false);
						if (Main.netMode == NetmodeID.MultiplayerClient) {
							NetMessage.SendData(MessageID.TileManipulation, number: k, number2: j);
						}
					}
					else if (newWall != CIData.Keep && newWall != tile.WallType) {
						tile.WallType = (ushort)newWall;

						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
					}
				}
			}
		}
	}

	public static void Convert(IAltBiome biome, int i, int j, int size = 4) {
		if (biome is null)
			throw new ArgumentNullException(nameof(biome), "Can't be null!");
		for (ushort k = (ushort)(i - size); k <= i + size; k++) {
			for (ushort l = (ushort)(j - size); l <= j + size; l++) {
				if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < 6) {
					Tile tile = Main.tile[k, l];
					int newTile = CIDatabase.GetConvertedTile(5 + biome.Type, tile.TileType);
					if (newTile == CIData.Break) {
						WorldGen.KillTile(k, l, false, false, false);
						if (Main.netMode == NetmodeID.MultiplayerClient) {
							NetMessage.SendData(MessageID.TileManipulation, number: k, number2: j);
						}
					}
					else if (newTile != CIData.Keep && newTile != tile.TileType) {
						tile.TileType = (ushort)newTile;

						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
					}

					int newWall = CIDatabase.GetConvertedWall(5 + biome.Type, tile.WallType);
					if (newWall == CIData.Break) {
						WorldGen.KillWall(k, l, false);
						if (Main.netMode == NetmodeID.MultiplayerClient) {
							NetMessage.SendData(MessageID.TileManipulation, number: k, number2: j);
						}
					}
					else if (newWall != CIData.Keep && newWall != tile.WallType) {
						tile.WallType = (ushort)newWall;

						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
					}
				}
			}
		}
		return;
	}
}
