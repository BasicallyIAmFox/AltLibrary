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
	private static ConversionData.Data[] data;

	public const int Keep = -1;
	public const int Break = -2;

	internal static void PostLoad() {
		SolutionLoader.Fill((TileLoader.TileCount + WallLoader.WallCount) * SolutionLoader.Count, out data);
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

	internal static int TileIndex(int solution, int type) => (TileLoader.TileCount * solution) + type;
	internal static int WallIndex(int solution, int type) => (TileLoader.TileCount * SolutionLoader.Count) + (WallLoader.WallCount * solution) + type;

	public static void Convert<T>(int i, int j) where T : ModSolution => Convert(ModContent.GetInstance<T>(), i, j);
	public static void Convert<T>(int i, int j, int size) where T : ModSolution => Convert(ModContent.GetInstance<T>(), i, j, size);
	public static void Convert(ModSolution solution, int i, int j) {
		const int wallOffset = 4;

		const int wasCalled = 0;
		const int breakTile = 1;
		const int replacedTile = 2;
		const int wasCalledW = wasCalled + wallOffset;
		const int breakTileW = breakTile + wallOffset;
		const int replacedTileW = replacedTile + wallOffset;

		var tile = Main.tile[i, j];
		var oldTile = tile.TileType;
		var oldWall = tile.WallType;
		var tIndex = TileIndex(solution.Type, oldTile);
		var wIndex = WallIndex(solution.Type, oldWall);
		var convertedTile = data[tIndex];
		var convertedWall = data[wIndex];

		var transformations = new BitsByte();

		var preConvTileVal = convertedTile?.PreConversionDelegate?.Invoke(tile, i, j);
		var preConvWallVal = convertedWall?.PreConversionDelegate?.Invoke(tile, i, j);
		transformations[breakTile] = preConvTileVal == ConversionRunCodeValues.Break;
		transformations[breakTileW] = preConvWallVal == ConversionRunCodeValues.Break;

		if (convertedTile != null && preConvTileVal != ConversionRunCodeValues.DontRun) {
			transformations[wasCalled] = true;
			if (convertedTile.ConvertsTo == Break) {
				transformations[breakTile] = true;
			}
			else {
				var conv = convertedTile.ConvertsTo;
				if (conv >= 0 && oldWall != conv) {
					tile.TileType = conv.As<ushort>();
					convertedTile.OnConversionDelegate?.Invoke(tile, oldWall, i, j);
					transformations[replacedTile] = true;
				}
			}
		}

		if (convertedWall != null && preConvWallVal != ConversionRunCodeValues.DontRun) {
			transformations[wasCalledW] = true;
			if (convertedWall.ConvertsTo == Break) {
				transformations[breakTileW] = true;
			}
			else {
				var conv = convertedWall.ConvertsTo;
				if (conv >= 0 && oldWall != conv) {
					tile.WallType = conv.As<ushort>();
					convertedWall.OnConversionDelegate?.Invoke(tile, oldWall, i, j);
					transformations[replacedTileW] = true;
				}
			}
		}

		if ((transformations[breakTile] || transformations[breakTileW]) && Main.netMode == NetmodeID.MultiplayerClient) {
			if (transformations[breakTile]) {
				WorldGen.KillTile(i, j);
			}
			if (transformations[breakTileW]) {
				WorldGen.KillWall(i, j);
			}
			NetMessage.SendData(MessageID.TileManipulation, number: i, number2: j);
		}
		if (transformations[replacedTile] || transformations[replacedTileW]) {
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
