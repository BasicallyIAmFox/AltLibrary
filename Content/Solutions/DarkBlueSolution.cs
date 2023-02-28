using AltLibrary.Common.Conversion;
using AltLibrary.Common.Solutions;
using Terraria;
using Terraria.ID;

namespace AltLibrary.Content.Solutions;

// Last update: 1.4.4.9
public sealed class DarkBlueSolution : ModSolution {
	public override void FillTileEntries(int index3, ref int convertedTile, ref bool forceConversionCode) {
		if (index3 == 60) {
			convertedTile = 70;
		}
		else if (TileID.Sets.Conversion.Thorn[index3]) {
			convertedTile = ConversionHandler.Break;
		}
	}

	public override void OnTileConversion(int index3, int index1, int index2) {
		if (index3 == 60) {
			WorldGen.TryKillingTreesAboveIfTheyWouldBecomeInvalid(index1, index2, Main.tile[index1, index2].TileType);
		}
	}

	public override void FillWallEntries(int index4, ref int convertedWall, ref bool forceConversionCode) {
		if (WallID.Sets.CanBeConvertedToGlowingMushroom[index4]) {
			convertedWall = 80;
		}
	}

	public override void OnWallConversion(int index4, int index1, int index2) {
	}
}
