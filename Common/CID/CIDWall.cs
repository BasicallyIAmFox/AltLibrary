using AltLibrary.Common.AltTypes;
using System.Runtime.CompilerServices;

namespace AltLibrary.Common.CID;

public sealed class CIDWall : CIData {
	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	public override void Bake() {
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	public override int GetConverted_Vanilla(in int baseTile, in byte conversionType, in ushort x, in ushort y) {
		return -1;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	public override int GetConverted_Modded(in int baseTile, in IAltBiome biome, in ushort x, in ushort y) {
		return -1;
	}
}