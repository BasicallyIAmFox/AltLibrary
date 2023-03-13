using AltLibrary.Common.Solutions;
using AltLibrary.Content.Solutions;
using AltLibrary.Core;
using AltLibrary.Core.Attributes;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.Conversion;

[FeedEventCore(nameof(PostLoad))]
public sealed class ConversionHandler {
	private static ConversionData.Data[] data;

	public const int Keep = -1;
	public const int Break = -2;

	internal static void PostLoad() {
		EventSystem.PostContentHook += mod => {
			SolutionLoader.Fill((TileLoader.TileCount + WallLoader.WallCount) * SolutionLoader.Count, out data);
		};
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
			_ => -1
		};
	}

	internal static int TileIndex(int solution, int type) => (TileLoader.TileCount * solution) + type;
	internal static int WallIndex(int solution, int type) => (TileLoader.TileCount * SolutionLoader.Count) + (WallLoader.WallCount * solution) + type;

	public static void Convert<T>(int i, int j) where T : ModSolution => Convert(ModContent.GetInstance<T>(), i, j);
	public static void Convert<T>(int i, int j, int size) where T : ModSolution => Convert(ModContent.GetInstance<T>(), i, j, size);
	public static void Convert(int type, int i, int j) => Convert(SolutionLoader.Get(type), i, j);
	public static void Convert(int type, int i, int j, int size) => Convert(SolutionLoader.Get(type), i, j, size);
	public static void Convert(ModSolution solution, int i, int j) => ConvertInternal(solution, i, j, ref MemoryMarshal.GetArrayDataReference(data));
	public static void Convert(ModSolution solution, int i, int j, int size) {
		ref var arrayData = ref MemoryMarshal.GetArrayDataReference(data);

		var startX = i - size;
		var endX = i + size;
		var startY = j - size;
		var endY = j + size;
		for (int l = startX; l <= endX; l++) {
			int k = startY;
			for (; k <= endY - (endY % 4); k += 4) {
				if (WorldGen.InWorld(l, k, 1) && Math.Abs(l - i) + Math.Abs(k - j) < 6) {
					ConvertInternal(solution, i, j, ref arrayData);
				}
				if (WorldGen.InWorld(l, k + 1, 1) && Math.Abs(l - i) + Math.Abs(k - j + 1) < 6) {
					ConvertInternal(solution, i, j, ref arrayData);
				}
				if (WorldGen.InWorld(l, k + 2, 1) && Math.Abs(l - i) + Math.Abs(k - j + 2) < 6) {
					ConvertInternal(solution, i, j, ref arrayData);
				}
				if (WorldGen.InWorld(l, k + 3, 1) && Math.Abs(l - i) + Math.Abs(k - j + 3) < 6) {
					ConvertInternal(solution, i, j, ref arrayData);
				}
			}
			for (; k <= endY; k++) {
				if (WorldGen.InWorld(l, k, 1) && Math.Abs(l - i) + Math.Abs(k - j) < 6) {
					ConvertInternal(solution, i, j, ref arrayData);
				}
			}
		}
	}

	private static void ConvertInternal(ModSolution solution, int i, int j, ref ConversionData.Data data) {
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
		var convertedTile = Unsafe.Add(ref data, TileIndex(solution.Type, oldTile));
		var convertedWall = Unsafe.Add(ref data, WallIndex(solution.Type, oldWall));

		var transformations = new BitsByte();

		var preConvTileVal = convertedTile?.PreConversionDelegate?.Invoke(tile, i, j);
		var preConvWallVal = convertedWall?.PreConversionDelegate?.Invoke(tile, i, j);
		transformations[breakTile] = preConvTileVal == ConversionRunCodeValues.Break;
		transformations[breakTileW] = preConvWallVal == ConversionRunCodeValues.Break;

		if (convertedTile != null && preConvTileVal != ConversionRunCodeValues.DontRun) {
			transformations[wasCalled] = true;
			var conv = convertedTile.ConvertsTo;
			if (conv == Break) {
				transformations[breakTile] = true;
			}
			else if (conv >= 0) {
				tile.TileType = conv.As<ushort>();
				convertedTile.OnConversionDelegate?.Invoke(tile, oldWall, i, j);
				transformations[replacedTile] = true;
			}
		}

		if (convertedWall != null && preConvWallVal != ConversionRunCodeValues.DontRun) {
			transformations[wasCalledW] = true;
			var conv = convertedWall.ConvertsTo;
			if (conv == Break) {
				transformations[breakTileW] = true;
			}
			else if (conv >= 0) {
				tile.WallType = conv.As<ushort>();
				convertedWall.OnConversionDelegate?.Invoke(tile, oldWall, i, j);
				transformations[replacedTileW] = true;
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
}
