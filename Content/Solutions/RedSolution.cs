using AltLibrary.Common.Solutions;
using Terraria;
using Terraria.ID;

namespace AltLibrary.Content.Solutions;

// Last update: 1.4.4.9
public sealed class RedSolution : ModSolution {
	public override void FillTileEntries(int index3, ref int convertedTile, ref bool forceConversionCode) {
		if ((Main.tileMoss[index3] || TileID.Sets.Conversion.Stone[index3]) && index3 != 203) {
			convertedTile = 203;
		}
		else if (TileID.Sets.Conversion.JungleGrass[index3] && index3 != 662) {
			convertedTile = 662;
		}
		else if (TileID.Sets.Conversion.Grass[index3] && index3 != 199) {
			convertedTile = 199;
		}
		else if (TileID.Sets.Conversion.Ice[index3] && index3 != 200) {
			convertedTile = 200;
		}
		else if (TileID.Sets.Conversion.Sand[index3] && index3 != 234) {
			convertedTile = 234;
		}
		else if (TileID.Sets.Conversion.HardenedSand[index3] && index3 != 399) {
			convertedTile = 399;
		}
		else if (TileID.Sets.Conversion.Sandstone[index3] && index3 != 401) {
			convertedTile = 401;
		}
		else if (TileID.Sets.Conversion.Thorn[index3] && index3 != 352) {
			convertedTile = 352;
		}
	}

	public override void OnTileConversion(int index3, int index1, int index2) {
		if ((Main.tileMoss[index3] || TileID.Sets.Conversion.Stone[index3]) && index3 != 203
			|| TileID.Sets.Conversion.JungleGrass[index3] && index3 != 662
			|| TileID.Sets.Conversion.Grass[index3] && index3 != 199
			|| TileID.Sets.Conversion.Ice[index3] && index3 != 200
			|| TileID.Sets.Conversion.Sand[index3] && index3 != 234) {
			WorldGen.TryKillingTreesAboveIfTheyWouldBecomeInvalid(index1, index2, Main.tile[index1, index2].TileType);
		}
	}

	public override void FillWallEntries(int index4, ref int convertedWall, ref bool forceConversionCode) {
		if (WallID.Sets.Conversion.Grass[index4] && index4 != 81) {
			convertedWall = 81;
		}
		else if (WallID.Sets.Conversion.Stone[index4] && index4 != 83) {
			convertedWall = 83;
		}
		else if (WallID.Sets.Conversion.HardenedSand[index4] && index4 != 218) {
			convertedWall = 218;
		}
		else if (WallID.Sets.Conversion.Sandstone[index4] && index4 != 221) {
			convertedWall = 221;
		}
		else if (WallID.Sets.Conversion.NewWall1[index4] && index4 != 192) {
			convertedWall = 192;
		}
		else if (WallID.Sets.Conversion.NewWall2[index4] && index4 != 193) {
			convertedWall = 193;
		}
		else if (WallID.Sets.Conversion.NewWall3[index4] && index4 != 194) {
			convertedWall = 194;
		}
		else if (WallID.Sets.Conversion.NewWall4[index4] && index4 != 195) {
			convertedWall = 195;
		}
	}

	public override void OnWallConversion(int index4, int index1, int index2) {
	}
}
