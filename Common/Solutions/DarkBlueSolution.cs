using Terraria.ID;

namespace AltLibrary.Common.Solutions;

public sealed class DarkBlueSolution : Solution {
	public override void FillTileEntries(int currentTileId, ref int tileEntry) {
		tileEntry = currentTileId switch {
			TileID.JungleGrass => TileID.MushroomGrass,
			_ => tileEntry
		};
	}

	public override void FillWallEntries(int currentWallId, ref int wallEntry) {
	}
}
