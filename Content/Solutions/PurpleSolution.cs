using AltLibrary.Common.Solutions;
using Terraria;
using Terraria.ID;

namespace AltLibrary.Content.Solutions;

// Last update: 1.4.4.9
public sealed class PurpleSolution : ModSolution {
	public override void FillTileEntries(int index3, ref int convertedTile, ref bool forceConversionCode) {
		if (TileID.Sets.Conversion.JungleGrass[index3] && index3 != 661) {
			convertedTile = 661;
		}
		else if ((Main.tileMoss[index3] || TileID.Sets.Conversion.Stone[index3]) && index3 != 25) {
			convertedTile = 25;
		}
		else if (TileID.Sets.Conversion.Grass[index3] && index3 != 23) {
			convertedTile = 23;
		}
		else if (TileID.Sets.Conversion.Ice[index3] && index3 != 163) {
			convertedTile = 163;
		}
		else if (TileID.Sets.Conversion.Sand[index3] && index3 != 112) {
			convertedTile = 112;
		}
		else if (TileID.Sets.Conversion.HardenedSand[index3] && index3 != 398) {
			convertedTile = 398;
		}
		else if (TileID.Sets.Conversion.Sandstone[index3] && index3 != 400) {
			convertedTile = 400;
		}
		else if (TileID.Sets.Conversion.Thorn[index3] && index3 != 32) {
			convertedTile = 32;
		}
	}

	public override void OnTileConversion(int index3, int index1, int index2) {
		if ((Main.tileMoss[index3] || TileID.Sets.Conversion.Stone[index3]) && index3 != 25
			|| TileID.Sets.Conversion.Grass[index3] && index3 != 23
			|| TileID.Sets.Conversion.Ice[index3] && index3 != 163
			|| TileID.Sets.Conversion.Sand[index3] && index3 != 112) {
			WorldGen.TryKillingTreesAboveIfTheyWouldBecomeInvalid(index1, index2, Main.tile[index1, index2].TileType);
		}
	}

	public override void FillWallEntries(int index4, ref int convertedWall, ref bool forceConversionCode) {
		if (WallID.Sets.Conversion.Grass[index4] && index4 != 69) {
			convertedWall = 69;
		}
		else if (WallID.Sets.Conversion.Stone[index4] && index4 != 3) {
			convertedWall = 3;
		}
		else if (WallID.Sets.Conversion.HardenedSand[index4] && index4 != 217) {
			convertedWall = 217;
		}
		else if (WallID.Sets.Conversion.Sandstone[index4] && index4 != 220) {
			convertedWall = 220;
		}
		else if (WallID.Sets.Conversion.NewWall1[index4] && index4 != 188) {
			convertedWall = 188;
		}
		else if (WallID.Sets.Conversion.NewWall2[index4] && index4 != 189) {
			convertedWall = 189;
		}
		else if (WallID.Sets.Conversion.NewWall3[index4] && index4 != 190) {
			convertedWall = 190;
		}
		else if (WallID.Sets.Conversion.NewWall4[index4] && index4 != 191) {
			convertedWall = 191;
		}
	}

	public override void OnWallConversion(int index4, int index1, int index2) {
	}
}
