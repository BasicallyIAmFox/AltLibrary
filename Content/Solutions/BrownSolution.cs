using AltLibrary.Common.Conversion;
using AltLibrary.Common.Solutions;
using Terraria;
using Terraria.ID;

namespace AltLibrary.Content.Solutions;

// Last update: 1.4.4.9
public sealed class BrownSolution : ModSolution {
	public override void FillTileEntries(int index3, ref int convertedTile, ref bool forceConversionCode) {
		if ((TileID.Sets.Conversion.Stone[index3] || TileID.Sets.Conversion.Ice[index3] || TileID.Sets.Conversion.Sandstone[index3]) && index3 != 1) {
			convertedTile = 1;
		}
		else if (TileID.Sets.Conversion.GolfGrass[index3] && index3 != 477) {
			convertedTile = 477;
		}
		else if (TileID.Sets.Conversion.Grass[index3] && index3 != 2 && index3 != 477) {
			convertedTile = 2;
		}
		else if ((TileID.Sets.Conversion.Sand[index3] || TileID.Sets.Conversion.HardenedSand[index3] || TileID.Sets.Conversion.Snow[index3] || TileID.Sets.Conversion.Dirt[index3]) && index3 != 0) {
			forceConversionCode = true;
		}
		else if (TileID.Sets.Conversion.Thorn[index3] && index3 != 69) {
			convertedTile = ConversionHandler.Break;
		}
	}

	public override void OnTileConversion(int index3, int index1, int index2) {
		if ((TileID.Sets.Conversion.Stone[index3] || TileID.Sets.Conversion.Ice[index3] || TileID.Sets.Conversion.Sandstone[index3]) && index3 != 1
			|| TileID.Sets.Conversion.GolfGrass[index3] && index3 != 477
			|| TileID.Sets.Conversion.Grass[index3] && index3 != 2 && index3 != 477) {
			WorldGen.TryKillingTreesAboveIfTheyWouldBecomeInvalid(index1, index2, Main.tile[index1, index2].TileType);
		}
		else if ((TileID.Sets.Conversion.Sand[index3] || TileID.Sets.Conversion.HardenedSand[index3] || TileID.Sets.Conversion.Snow[index3] || TileID.Sets.Conversion.Dirt[index3]) && index3 != 0) {
			int newFloorType = 0;
			if (WorldGen.TileIsExposedToAir(index1, index2))
				newFloorType = 2;
			WorldGen.TryKillingTreesAboveIfTheyWouldBecomeInvalid(index1, index2, newFloorType);
			Main.tile[index1, index2].TileType = (ushort)newFloorType;
		}
	}

	public override void FillWallEntries(int index4, ref int convertedWall, ref bool forceConversionCode) {
		if ((WallID.Sets.Conversion.Stone[index4] || WallID.Sets.Conversion.Ice[index4] || WallID.Sets.Conversion.Sandstone[index4]) && index4 != 1) {
			convertedWall = 1;
		}
		else if ((WallID.Sets.Conversion.HardenedSand[index4] || WallID.Sets.Conversion.Snow[index4] || WallID.Sets.Conversion.Dirt[index4]) && index4 != 2) {
			convertedWall = 2;
		}
		else if (WallID.Sets.Conversion.NewWall1[index4] && index4 != 196) {
			convertedWall = 196;
		}
		else if (WallID.Sets.Conversion.NewWall2[index4] && index4 != 197) {
			convertedWall = 197;
		}
		else if (WallID.Sets.Conversion.NewWall3[index4] && index4 != 198) {
			convertedWall = 198;
		}
		else if (WallID.Sets.Conversion.NewWall4[index4] && index4 != 199) {
			convertedWall = 199;
		}
	}

	public override void OnWallConversion(int index4, int index1, int index2) {
	}
}
