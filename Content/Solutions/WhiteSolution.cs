﻿using AltLibrary.Common.Conversion;
using AltLibrary.Common.Solutions;
using Terraria;
using Terraria.ID;

namespace AltLibrary.Content.Solutions;

// Last update: 1.4.4.9
public sealed class WhiteSolution : ModSolution {
	public override void FillTileEntries(int index3, ref int convertedTile, ref bool forceConversionCode) {
		if ((TileID.Sets.Conversion.Grass[index3] || TileID.Sets.Conversion.Sand[index3] || TileID.Sets.Conversion.HardenedSand[index3] || TileID.Sets.Conversion.Snow[index3] || TileID.Sets.Conversion.Dirt[index3]) && index3 != 147) {
			convertedTile = 147;
		}
		else if ((Main.tileMoss[index3] || TileID.Sets.Conversion.Stone[index3] || TileID.Sets.Conversion.Ice[index3] || TileID.Sets.Conversion.Sandstone[index3]) && index3 != 161) {
			convertedTile = 161;
		}
		else if (TileID.Sets.Conversion.Thorn[index3] && index3 != 69) {
			convertedTile = ConversionHandler.Break;
		}
	}

	public override void OnTileConversion(int index3, int index1, int index2) {
		if ((TileID.Sets.Conversion.Grass[index3] || TileID.Sets.Conversion.Sand[index3] || TileID.Sets.Conversion.HardenedSand[index3] || TileID.Sets.Conversion.Snow[index3] || TileID.Sets.Conversion.Dirt[index3]) && index3 != 147
			|| (Main.tileMoss[index3] || TileID.Sets.Conversion.Stone[index3] || TileID.Sets.Conversion.Ice[index3] || TileID.Sets.Conversion.Sandstone[index3]) && index3 != 161) {
			WorldGen.TryKillingTreesAboveIfTheyWouldBecomeInvalid(index1, index2, Main.tile[index1, index2].TileType);
		}
	}

	public override void FillWallEntries(int index4, ref int convertedWall, ref bool forceConversionCode) {
		if ((WallID.Sets.Conversion.Stone[index4] || WallID.Sets.Conversion.NewWall1[index4] || WallID.Sets.Conversion.NewWall2[index4] || WallID.Sets.Conversion.NewWall3[index4] || WallID.Sets.Conversion.NewWall4[index4] || WallID.Sets.Conversion.Ice[index4] || WallID.Sets.Conversion.Sandstone[index4]) && index4 != 71) {
			convertedWall = 71;
		}
		else if ((WallID.Sets.Conversion.HardenedSand[index4] || WallID.Sets.Conversion.Dirt[index4] || WallID.Sets.Conversion.Snow[index4]) && index4 != 40) {
			convertedWall = 40;
		}
	}

	public override void OnWallConversion(int index4, int index1, int index2) {
	}
}
