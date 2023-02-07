using AltLibrary.Common.AltTypes;
using System.Runtime.CompilerServices;

namespace AltLibrary.Common.CID;

public sealed class CIDWall : CIData {
	public override void Bake() {
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	public override int GetConverted_Vanilla(int baseTile, byte conversionType, ushort x, ushort y) {
		return KEEP;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	public override int GetConverted_Modded(int baseTile, IAltBiome biome, ushort x, ushort y) {
		return KEEP;
	}
}