using AltLibrary.Common.Solutions;
using AltLibrary.Content.Solutions;
using AltLibrary.Core.Attributes;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.Conversion;

[LoadableContent(ContentOrder.PostContent, nameof(PostLoad))]
public sealed class ConversionHandler {
	private static int[] tiles;
	private static int[] walls;
	private static bool[] tiles2;
	private static bool[] walls2;

	public const int Keep = -1;
	public const int Break = -2;

	internal static void PostLoad() {
		var tileFactory = new SetFactory(TileLoader.TileCount * SolutionLoader.Count);
		var wallFactory = new SetFactory(WallLoader.WallCount * SolutionLoader.Count);
		tiles = tileFactory.CreateIntSet(defaultState: Keep);
		walls = wallFactory.CreateIntSet(defaultState: Keep);
		tiles2 = tileFactory.CreateBoolSet(defaultState: false);
		walls2 = wallFactory.CreateBoolSet(defaultState: false);

		foreach (var solution in SolutionLoader.solutions) {
			for (int i = 0; i < TileLoader.TileCount; i++) {
				var id = TileIndex(solution, i);
				solution.FillTileEntries(i, ref tiles[id], ref tiles2[id]);
			}
			for (int i = 0; i < WallLoader.WallCount; i++) {
				var id = WallIndex(solution, i);
				solution.FillWallEntries(i, ref walls[id], ref walls2[id]);
			}
		}
	}

	internal static int ChangeVanillaConversionIdToModdedConversionId(int biomeConversionId) {
		return biomeConversionId switch {
			BiomeConversionID.Purity => ModContent.GetInstance<GreenSolution>().Type,
			BiomeConversionID.Corruption => ModContent.GetInstance<PurpleSolution>().Type,
			BiomeConversionID.Hallow => ModContent.GetInstance<BlueSolution>().Type,
			BiomeConversionID.GlowingMushroom => ModContent.GetInstance<DarkBlueSolution>().Type,
			BiomeConversionID.Crimson => ModContent.GetInstance<RedSolution>().Type,
			BiomeConversionID.Sand => ModContent.GetInstance<YellowSolution>().Type,
			BiomeConversionID.Snow => ModContent.GetInstance<WhiteSolution>().Type,
			BiomeConversionID.Dirt => ModContent.GetInstance<BrownSolution>().Type,
			_ => biomeConversionId
		};
	}

	internal static int TileIndex(ModSolution solution, int type) => TileLoader.TileCount * solution.Type + type;
	internal static int WallIndex(ModSolution solution, int type) => WallLoader.WallCount * solution.Type + type;

	public static void Convert<T>(int i, int j) where T : ModSolution => Convert(ModContent.GetInstance<T>(), i, j);
	public static void Convert<T>(int i, int j, int size) where T : ModSolution => Convert(ModContent.GetInstance<T>(), i, j, size);
	public static void Convert(ModSolution solution, int i, int j) {
		var tile = Main.tile[i, j];
		var oldTile = tile.TileType;
		var oldWall = tile.WallType;
		var convertedTile = tiles[TileIndex(solution, tile.TileType)];
		var convertedWall = walls[WallIndex(solution, tile.WallType)];

		var transformations = new BitsByte();
		var breakNewTile = convertedTile == Break;
		var breakNewWall = convertedWall == Break;
		if (breakNewTile || breakNewWall) {
			if (breakNewTile) {
				WorldGen.KillTile(i, j);
			}
			if (breakNewWall) {
				WorldGen.KillWall(i, j);
			}

			if (Main.netMode == NetmodeID.MultiplayerClient) {
				NetMessage.SendData(MessageID.TileManipulation, number: i, number2: j);
			}
			return;
		}

		if (convertedTile >= 0 && tile.TileType != convertedTile) {
			tile.TileType = (ushort)convertedTile;
			transformations[0] = true;
			transformations[1] = true;
		}
		if (convertedWall >= 0 && tile.WallType != convertedWall) {
			tile.WallType = (ushort)convertedWall;
			transformations[0] = true;
			transformations[2] = true;
		}
		if (tiles2[oldTile] || walls2[oldWall] || transformations[0]) {
			if (transformations[1]) {
				solution.OnTileConversion(oldTile, i, j);
			}
			if (transformations[2]) {
				solution.OnWallConversion(oldWall, i, j);
			}
			WorldGen.SquareTileFrame(i, j);
			NetMessage.SendTileSquare(-1, i, j);
		}
	}
	public static void Convert(ModSolution solution, int i, int j, int size) {
		for (int l = i - size; l <= i + size; l++) {
			for (int k = j - size; k <= j + size; k++) {
				if (!WorldGen.InWorld(l, k, 1) || Math.Abs(l - i) + Math.Abs(k - j) >= 6) {
					continue;
				}
				Convert(solution, i, j);
			}
		}
	}
}
