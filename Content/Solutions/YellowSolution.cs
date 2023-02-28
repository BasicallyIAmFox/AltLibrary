using AltLibrary.Common.Conversion;
using AltLibrary.Common.Solutions;
using Terraria;
using Terraria.ID;

namespace AltLibrary.Content.Solutions;

// Last update: 1.4.4.9
public sealed class YellowSolution : ModSolution {
	public override void FillTileEntries(int index3, ref int convertedTile, ref bool forceConversionCode) {
		if ((TileID.Sets.Conversion.Grass[index3] || TileID.Sets.Conversion.Sand[index3] || TileID.Sets.Conversion.Snow[index3] || TileID.Sets.Conversion.Dirt[index3]) && index3 != 53) {
			forceConversionCode = true;
		}
		else if (TileID.Sets.Conversion.HardenedSand[index3] && index3 != 397) {
			convertedTile = 397;
		}
		else if ((Main.tileMoss[index3] || TileID.Sets.Conversion.Stone[index3] || TileID.Sets.Conversion.Ice[index3] || TileID.Sets.Conversion.Sandstone[index3]) && index3 != 396) {
			convertedTile = 396;
		}
		else if (TileID.Sets.Conversion.Thorn[index3] && index3 != 69) {
			convertedTile = ConversionHandler.Break;
		}
	}

	public override void OnTileConversion(int index3, int index1, int index2) {
		if ((TileID.Sets.Conversion.Grass[index3] || TileID.Sets.Conversion.Sand[index3] || TileID.Sets.Conversion.Snow[index3] || TileID.Sets.Conversion.Dirt[index3]) && index3 != 53) {
			int newFloorType = 53;
			if (WorldGen.BlockBelowMakesSandConvertIntoHardenedSand(index1, index2))
				newFloorType = 397;
			WorldGen.TryKillingTreesAboveIfTheyWouldBecomeInvalid(index1, index2, newFloorType);
			Main.tile[index1, index2].TileType = (ushort)newFloorType;
		}
		else if ((Main.tileMoss[index3] || TileID.Sets.Conversion.Stone[index3] || TileID.Sets.Conversion.Ice[index3] || TileID.Sets.Conversion.Sandstone[index3]) && index3 != 396) {
			WorldGen.TryKillingTreesAboveIfTheyWouldBecomeInvalid(index1, index2, Main.tile[index1, index2].TileType);
		}
	}

	public override void FillWallEntries(int index4, ref int convertedWall, ref bool forceConversionCode) {
		if ((WallID.Sets.Conversion.Stone[index4] || WallID.Sets.Conversion.NewWall1[index4] || WallID.Sets.Conversion.NewWall2[index4] || WallID.Sets.Conversion.NewWall3[index4] || WallID.Sets.Conversion.NewWall4[index4] || WallID.Sets.Conversion.Ice[index4] || WallID.Sets.Conversion.Sandstone[index4]) && index4 != 187) {
			convertedWall = 187;
		}
		else if ((WallID.Sets.Conversion.HardenedSand[index4] || WallID.Sets.Conversion.Dirt[index4] || WallID.Sets.Conversion.Snow[index4]) && index4 != 216) {
			convertedWall = 216;
		}
	}

	public override void OnWallConversion(int index4, int index1, int index2) {
	}
}
