using AltLibrary.Common.Conversion;
using AltLibrary.Common.Solutions;
using Terraria;
using Terraria.ID;

namespace AltLibrary.Content.Solutions;

// Last update: 1.4.4.9
public sealed class GreenSolution : ModSolution {
	public override void FillTileEntries(int index3, ref int convertedTile, ref bool forceConversionCode) {
		if (index3 == 492) {
			convertedTile = 477;
		}
		else if (TileID.Sets.Conversion.JungleGrass[index3] && index3 != 60) {
			convertedTile = 60;
		}
		else if (TileID.Sets.Conversion.Grass[index3] && index3 != 2 && index3 != 477) {
			convertedTile = 2;
		}
		else if (TileID.Sets.Conversion.Stone[index3] && index3 != 1) {
			convertedTile = 1;
		}
		else if (TileID.Sets.Conversion.Sand[index3] && index3 != 53) {
			convertedTile = 53;
		}
		else if (TileID.Sets.Conversion.HardenedSand[index3] && index3 != 397) {
			convertedTile = 397;
		}
		else if (TileID.Sets.Conversion.Sandstone[index3] && index3 != 396) {
			convertedTile = 396;
		}
		else if (TileID.Sets.Conversion.Ice[index3] && index3 != 161) {
			convertedTile = 161;
		}
		else if (TileID.Sets.Conversion.MushroomGrass[index3]) {
			convertedTile = 60;
		}
		else if (index3 == 32 || index3 == 352) {
			convertedTile = ConversionHandler.Break;
		}
	}

	public override void OnTileConversion(int index3, int i, int j) {
		if (index3 == 492
			|| TileID.Sets.Conversion.JungleGrass[index3] && index3 != 60
			|| TileID.Sets.Conversion.Grass[index3] && index3 != 2 && index3 != 477
			|| TileID.Sets.Conversion.Stone[index3] && index3 != 1
			|| TileID.Sets.Conversion.Sand[index3] && index3 != 53
			|| TileID.Sets.Conversion.HardenedSand[index3] && index3 != 397
			|| TileID.Sets.Conversion.Sandstone[index3] && index3 != 396
			|| TileID.Sets.Conversion.Ice[index3] && index3 != 161
			|| TileID.Sets.Conversion.MushroomGrass[index3]) {
			WorldGen.TryKillingTreesAboveIfTheyWouldBecomeInvalid(i, j, Main.tile[i, j].TileType);
		}
	}

	public override void FillWallEntries(int index4, ref int convertedWall, ref bool forceConversionCode) {
		if (index4 == 69 || index4 == 70 || index4 == 81) {
			forceConversionCode = true;
		}
		else if (WallID.Sets.Conversion.Stone[index4] && index4 != 1 && index4 != 262 && index4 != 274 && index4 != 61 && index4 != 185) {
			convertedWall = 1;
		}
		else if (WallID.Sets.Conversion.Stone[index4] && index4 == 262) {
			convertedWall = 61;
		}
		else if (WallID.Sets.Conversion.Stone[index4] && index4 == 274) {
			convertedWall = 185;
		}
		if (WallID.Sets.Conversion.NewWall1[index4] && index4 != 212) {
			convertedWall = 212;
		}
		else if (WallID.Sets.Conversion.NewWall2[index4] && index4 != 213) {
			convertedWall = 213;
		}
		else if (WallID.Sets.Conversion.NewWall3[index4] && index4 != 214) {
			convertedWall = 214;
		}
		else if (WallID.Sets.Conversion.NewWall4[index4] && index4 != 215) {
			convertedWall = 215;
		}
		else if (index4 == 80) {
			forceConversionCode = true;
		}
		else if (WallID.Sets.Conversion.HardenedSand[index4] && index4 != 216) {
			convertedWall = 216;
		}
		else if (WallID.Sets.Conversion.Sandstone[index4] && index4 != 187) {
			convertedWall = 187;
		}
	}

	public override void OnWallConversion(int index4, int index1, int index2) {
		if (index4 == 69 || index4 == 70 || index4 == 81) {
			Main.tile[index1, index2].WallType = index2 >= Main.worldSurface ? (ushort)64 : (!WorldGen.genRand.NextBool(10) ? (ushort)63 : (ushort)65);
		}
		else if (index4 == 80) {
			if (index2 < Main.worldSurface + 4.0 + WorldGen.genRand.Next(3) || index2 > (Main.maxTilesY + Main.rockLayer) / 2.0 - 3.0 + WorldGen.genRand.Next(3)) {
				Main.tile[index1, index2].WallType = 15;
			}
			else {
				Main.tile[index1, index2].WallType = 64;
			}
		}
	}
}
