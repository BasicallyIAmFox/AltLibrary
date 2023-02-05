using AltLibrary.Common.AltTypes;
using System;

namespace AltLibrary.Common.CID;

public sealed class CIDWall : CIData {
	public CIDWall() {
		Bake();
	}

	public override void Bake() {
	}

	[Obsolete]
	public override int GetConverted_Vanilla(int baseTile, int conversionType, int x, int y) {
		return -1;
	}

	[Obsolete]
	public override int GetConverted_Modded(int baseTile, IAltBiome biome, int x, int y) {
		return -1;
	}
}