using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Attributes;
using AltLibrary.Common.CID;
using AltLibrary.Common.Data;
using System;
using Terraria;
using Terraria.ID;

namespace AltLibrary.Common.Hooks;

[LoadableContent(ContentOrder.Content, nameof(Load))]
public static class CIDConvert {
	public static void Load() {
		ILHelper.On<WorldGen>(nameof(WorldGen.Convert), On_WorldGen_Convert);
	}

	private static void On_WorldGen_Convert(On_WorldGen.orig_Convert orig, int i, int j, int conversionType, int size) {
		for (ushort k = (ushort)(i - size); k <= i + size; k++) {
			for (ushort l = (ushort)(j - size); l <= j + size; l++) {
				if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < 6) {
					Tile tile = Main.tile[k, l];
					int newTile = CIDatabase.GetConvertedTile_Vanilla(tile.TileType, (byte)conversionType, in k, in l);
					if (newTile == CIData.KILL) {
						WorldGen.KillTile(k, l, false, false, false);
						if (Main.netMode == NetmodeID.MultiplayerClient) {
							NetMessage.SendData(MessageID.TileManipulation, number: k, number2: j);
						}
					}
					else if (newTile != CIData.KEEP && newTile != tile.TileType) {
						tile.TileType = (ushort)newTile;

						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
					}

					int newWall = CIDatabase.GetConvertedWall_Vanilla(tile.TileType, (byte)conversionType, k, l);
					if (newWall == CIData.KILL) {
						WorldGen.KillWall(k, l, false);
						if (Main.netMode == NetmodeID.MultiplayerClient) {
							NetMessage.SendData(MessageID.TileManipulation, number: k, number2: j);
						}
					}
					else if (newWall != CIData.KEEP && newWall != tile.WallType) {
						tile.WallType = (ushort)newWall;

						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
					}

					var corruptGrass = conversionType switch {
						1 => TileID.CorruptGrass,
						2 => TileID.HallowedGrass,
						4 => TileID.CrimsonGrass,
						_ => 0,
					};
					if (corruptGrass != 0) {
						if (tile.TileType == TileID.Mud && (Main.tile[k - 1, l].TileType == corruptGrass || Main.tile[k + 1, l].TileType == corruptGrass || Main.tile[k, l - 1].TileType == corruptGrass || Main.tile[k, l + 1].TileType == corruptGrass)) {
							Main.tile[k, l].TileType = TileID.Dirt;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
						}
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
					int newTile = CIDatabase.GetConvertedTile_Modded(tile.TileType, in biome, in k, in l);
					if (newTile == CIData.KILL) {
						WorldGen.KillTile(k, l, false, false, false);
						if (Main.netMode == NetmodeID.MultiplayerClient) {
							NetMessage.SendData(MessageID.TileManipulation, number: k, number2: j);
						}
					}
					else if (newTile != CIData.KEEP && newTile != tile.TileType) {
						tile.TileType = (ushort)newTile;

						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
					}

					int newWall = CIDatabase.GetConvertedWall_Modded(tile.WallType, in biome, in k, in l);
					if (newWall == CIData.KILL) {
						WorldGen.KillWall(k, l, false);
						if (Main.netMode == NetmodeID.MultiplayerClient) {
							NetMessage.SendData(MessageID.TileManipulation, number: k, number2: j);
						}
					}
					else if (newWall != CIData.KEEP && newWall != tile.WallType) {
						tile.WallType = (ushort)newWall;

						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
					}

					var conversionData = biome.DataHandler.Get<ConversionData>();
					if (conversionData.MudToDirt && tile.TileType == TileID.Mud && conversionData.Grass != 0 && (Main.tile[k - 1, l].TileType == conversionData.Grass || Main.tile[k + 1, l].TileType == conversionData.Grass || Main.tile[k, l - 1].TileType == conversionData.Grass || Main.tile[k, l + 1].TileType == conversionData.Grass)) {
						Main.tile[k, l].TileType = TileID.Dirt;
						WorldGen.SquareTileFrame(k, l, true);
						NetMessage.SendTileSquare(-1, k, l, TileChangeType.None);
					}
				}
			}
		}
		return;
	}
}
