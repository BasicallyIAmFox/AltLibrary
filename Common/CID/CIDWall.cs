using AltLibrary.Common.AltTypes;
using System;

namespace AltLibrary.Common.CID;

public sealed class CIDWall : CIData {
	public override void Bake() {
	}

	[Obsolete]
	public override int GetConverted_Vanilla(in int baseTile, in byte conversionType, in ushort x, in ushort y) {
		return -1;
	}

	[Obsolete]
	public override int GetConverted_Modded(in int baseTile, in IAltBiome biome, in ushort x, in ushort y) {
		return -1;
	}
}