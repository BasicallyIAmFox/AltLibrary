﻿namespace AltLibrary.Common.Solutions;

public sealed class YellowSolution : Solution {
	public override void FillTileEntries(int currentTileId, ref int tileEntry) {
		tileEntry = currentTileId switch {
			_ => tileEntry
		};
	}

	public override void FillWallEntries(int currentWallId, ref int wallEntry) {
	}
}