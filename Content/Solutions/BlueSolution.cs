using AltLibrary.Common.Conversion;
using AltLibrary.Common.Solutions;
using Terraria;
using Terraria.ID;

namespace AltLibrary.Content.Solutions;

// Last update: 1.4.4.9
public sealed class BlueSolution : ModSolution {
	public override void FillTileEntries(int index3, ref int convertedTile, ref bool forceConversionCode) {
		if ((Main.tileMoss[index3] || TileID.Sets.Conversion.Stone[index3]) && index3 != 117) {
			convertedTile = 117;
		}
		else if (TileID.Sets.Conversion.GolfGrass[index3] && index3 != 492) {
			convertedTile = 492;
		}
		else if (TileID.Sets.Conversion.Grass[index3] && index3 != 109 && index3 != 492) {
			convertedTile = 109;
		}
		else if (TileID.Sets.Conversion.Ice[index3] && index3 != 164) {
			convertedTile = 164;
		}
		else if (TileID.Sets.Conversion.Sand[index3] && index3 != 116) {
			convertedTile = 116;
		}
		else if (TileID.Sets.Conversion.HardenedSand[index3] && index3 != 402) {
			convertedTile = 402;
		}
		else if (TileID.Sets.Conversion.Sandstone[index3] && index3 != 403) {
			convertedTile = 403;
		}
		else if (TileID.Sets.Conversion.Thorn[index3]) {
			convertedTile = ConversionHandler.Break;
		}
		if (index3 == 59) {
			forceConversionCode = true;
		}
	}

	public override void OnTileConversion(int index3, int index1, int index2) {
		if ((Main.tileMoss[index3] || TileID.Sets.Conversion.Stone[index3]) && index3 != 117
			|| TileID.Sets.Conversion.GolfGrass[index3] && index3 != 492
			|| TileID.Sets.Conversion.Grass[index3] && index3 != 109 && index3 != 492
			|| TileID.Sets.Conversion.Ice[index3] && index3 != 164
			|| TileID.Sets.Conversion.Sand[index3] && index3 != 116) {
			WorldGen.TryKillingTreesAboveIfTheyWouldBecomeInvalid(index1, index2, Main.tile[index1, index2].TileType);
		}
		if (index3 == 59 && (Main.tile[index1 - 1, index2].TileType == 109 || Main.tile[index1 + 1, index2].TileType == 109 || Main.tile[index1, index2 - 1].TileType == 109 || Main.tile[index1, index2 + 1].TileType == 109)) {
			Main.tile[index1, index2].TileType = 0;
		}
	}

	public override void FillWallEntries(int index4, ref int convertedWall, ref bool forceConversionCode) {
		if (WallID.Sets.Conversion.Grass[index4] && index4 != 70) {
			convertedWall = 70;
		}
		else if (WallID.Sets.Conversion.Stone[index4] && index4 != 28) {
			convertedWall = 28;
		}
		else if (WallID.Sets.Conversion.HardenedSand[index4] && index4 != 219) {
			convertedWall = 219;
		}
		else if (WallID.Sets.Conversion.Sandstone[index4] && index4 != 222) {
			convertedWall = 222;
		}
		else if (WallID.Sets.Conversion.NewWall1[index4] && index4 != 200) {
			convertedWall = 200;
		}
		else if (WallID.Sets.Conversion.NewWall2[index4] && index4 != 201) {
			convertedWall = 201;
		}
		else if (WallID.Sets.Conversion.NewWall3[index4] && index4 != 202) {
			convertedWall = 202;
		}
		else if (WallID.Sets.Conversion.NewWall4[index4] && index4 != 203) {
			convertedWall = 203;
		}
	}

	public override void OnWallConversion(int index4, int index1, int index2) {
	}
}
