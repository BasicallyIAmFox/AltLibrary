using Terraria.ID;

namespace AltLibrary.Common.Solutions;

public sealed class GreenSolution : Solution {
	public override void FillTileEntries(int currentTileId, ref int tileEntry) {
		tileEntry = currentTileId switch {
			TileID.Stone => TileID.Stone,

			TileID.Grass => TileID.Grass,
			TileID.GolfGrass => TileID.GolfGrass,
			TileID.JungleGrass => TileID.JungleGrass,

			TileID.IceBlock => TileID.IceBlock,

			TileID.Sand => TileID.Sand,
			TileID.HardenedSand => TileID.HardenedSand,
			TileID.Sandstone => TileID.Sandstone,

			TileID.JungleThorns => TileID.JungleThorns,
			TileID.CorruptThorns => TileID.JungleThorns,
			TileID.CrimsonThorns => TileID.JungleThorns,
			TileID.MushroomGrass => TileID.JungleGrass,
			_ => tileEntry
		};
	}

	public override void FillWallEntries(int currentWallId, ref int wallEntry) {
	}
}
