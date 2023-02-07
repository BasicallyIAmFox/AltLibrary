using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Attributes;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.CID;

// TODO: More optimization.

// Conversion Inhertiance Database
public interface ICIData {
	IReadOnlyDictionary<int, int> Parent { get; }

	IReadOnlyDictionary<int, int> PurityConversion { get; }
	IReadOnlyDictionary<int, int> HallowConversion { get; }
	IReadOnlyDictionary<int, int> CorruptionConversion { get; }
	IReadOnlyDictionary<int, int> CrimsonConversion { get; }
	IReadOnlyDictionary<int, int> MushroomConversion { get; }
	IReadOnlyDictionary<int, int> ForestConversion { get; }
	IReadOnlyDictionary<int, int> DesertConversion { get; }
	IReadOnlyDictionary<int, int> SnowConversion { get; }

	IReadOnlyDictionary<int, BitsByte> ForceDeconversion { get; }
	IReadOnlyDictionary<int, BitsByte> BreakIfConversionFail { get; }
}
public abstract class CIData : ICIData {
	public delegate int GetAltBlockEvent(ICIData data, in int baseTile, in byte conversionType, in ushort x, in ushort y);

	public static GetAltBlockEvent AltBlockEvent;

	protected Dictionary<int, int> Parent = new();
	IReadOnlyDictionary<int, int> ICIData.Parent => Parent;

	protected Dictionary<int, int> PurityConversion = new();
	protected Dictionary<int, int> HallowConversion = new();
	protected Dictionary<int, int> CorruptionConversion = new();
	protected Dictionary<int, int> CrimsonConversion = new();
	protected Dictionary<int, int> MushroomConversion = new();
	protected Dictionary<int, int> ForestConversion = new();
	protected Dictionary<int, int> DesertConversion = new();
	protected Dictionary<int, int> SnowConversion = new();
	IReadOnlyDictionary<int, int> ICIData.PurityConversion => PurityConversion;
	IReadOnlyDictionary<int, int> ICIData.HallowConversion => HallowConversion;
	IReadOnlyDictionary<int, int> ICIData.CorruptionConversion => CorruptionConversion;
	IReadOnlyDictionary<int, int> ICIData.CrimsonConversion => CrimsonConversion;
	IReadOnlyDictionary<int, int> ICIData.MushroomConversion => MushroomConversion;
	IReadOnlyDictionary<int, int> ICIData.ForestConversion => ForestConversion;
	IReadOnlyDictionary<int, int> ICIData.DesertConversion => DesertConversion;
	IReadOnlyDictionary<int, int> ICIData.SnowConversion => SnowConversion;

	protected Dictionary<int, BitsByte> ForceDeconversion = new();
	protected Dictionary<int, BitsByte> BreakIfConversionFail = new();
	IReadOnlyDictionary<int, BitsByte> ICIData.ForceDeconversion => ForceDeconversion;
	IReadOnlyDictionary<int, BitsByte> ICIData.BreakIfConversionFail => BreakIfConversionFail;

	public const int KEEP = -1;
	public const int KILL = -2;

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	public int GetConverted(in int baseTile, in byte conversionType, in ushort x, in ushort y) {
		int forcedConvertedTile = KEEP;
		int tile = baseTile;

		// a loop here will decrease performance by about 50%, and this is performance-critical case so we have to deal with... this.
		// 0
		if (!Parent.TryGetValue(tile, out int test)) {
			return forcedConvertedTile;
		}
		tile = test;

		if (ForceDeconversion.TryGetValue(tile, out BitsByte bits) && bits[conversionType]) {
			forcedConvertedTile = test;
		}

		// 1
		if (!Parent.TryGetValue(tile, out test)) {
			return forcedConvertedTile;
		}
		tile = test;

		if (ForceDeconversion.TryGetValue(tile, out bits) && bits[conversionType]) {
			forcedConvertedTile = test;
		}

		// 2
		if (!Parent.TryGetValue(tile, out test)) {
			return forcedConvertedTile;
		}
		tile = test;

		if (ForceDeconversion.TryGetValue(tile, out bits) && bits[conversionType]) {
			forcedConvertedTile = test;
		}

		// 3
		if (!Parent.TryGetValue(tile, out test)) {
			return forcedConvertedTile;
		}
		tile = test;

		if (ForceDeconversion.TryGetValue(tile, out bits) && bits[conversionType]) {
			forcedConvertedTile = test;
		}

		// 4
		if (!Parent.TryGetValue(tile, out test)) {
			return forcedConvertedTile;
		}
		tile = test;

		if (ForceDeconversion.TryGetValue(tile, out bits) && bits[conversionType]) {
			forcedConvertedTile = test;
		}

		// 5
		if (!Parent.TryGetValue(tile, out test)) {
			return forcedConvertedTile;
		}
		tile = test;

		if (ForceDeconversion.TryGetValue(tile, out bits) && bits[conversionType]) {
			forcedConvertedTile = test;
		}

		// 6
		if (!Parent.TryGetValue(tile, out test)) {
			return forcedConvertedTile;
		}
		tile = test;

		if (GetAltBlock(in tile, in conversionType, in x, in y) != KEEP) {
			return test;
		}

		if (BreakIfConversionFail.TryGetValue(tile, out bits) && bits[conversionType]) {
			forcedConvertedTile = KILL;
		}

		return forcedConvertedTile;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	public virtual int GetAltBlock(in int baseTile, in byte conversionType, in ushort x, in ushort y) {
		int newTile;
		if (AltBlockEvent != null) {
			return AltBlockEvent(this, in baseTile, in conversionType, in x, in y);
		}
		switch (conversionType) {
			case BiomeConversionID.Purity:
				if (!PurityConversion.TryGetValue(baseTile, out newTile)) {
					newTile = KEEP;
				}
				break;
			case BiomeConversionID.Corruption:
				if (!CorruptionConversion.TryGetValue(baseTile, out newTile)) {
					newTile = KEEP;
				}
				break;
			case BiomeConversionID.Crimson:
				if (!CrimsonConversion.TryGetValue(baseTile, out newTile)) {
					newTile = KEEP;
				}
				break;
			case BiomeConversionID.Hallow:
				if (!HallowConversion.TryGetValue(baseTile, out newTile)) {
					newTile = KEEP;
				}
				break;
			case BiomeConversionID.GlowingMushroom:
				if (!MushroomConversion.TryGetValue(baseTile, out newTile)) {
					newTile = KEEP;
				}
				break;
			case BiomeConversionID.Sand:
				if (!DesertConversion.TryGetValue(baseTile, out newTile)) {
					newTile = KEEP;
				}
				break;
			case BiomeConversionID.Snow:
				if (!SnowConversion.TryGetValue(baseTile, out newTile)) {
					newTile = KEEP;
				}
				break;
			case BiomeConversionID.Dirt:
				if (!ForestConversion.TryGetValue(baseTile, out newTile)) {
					newTile = KEEP;
				}
				break;
			default:
				if (!Parent.TryGetValue(baseTile, out newTile)) {
					newTile = KEEP;
				}
				break;
		}
		return newTile;
	}

	public abstract void Bake();

	public abstract int GetConverted_Vanilla(in int baseTile, in byte conversionType, in ushort x, in ushort y);
	public abstract int GetConverted_Modded(in int baseTile, in IAltBiome biome, in ushort x, in ushort y);

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	public int GetConverted_Modded<T>(in int baseTile, in ushort x, in ushort y) where T : class, IAltBiome
		=> GetConverted_Modded(in baseTile, ModContent.GetInstance<T>(), in x, in y);
}
[LoadableContent(ContentOrder.PostContent, nameof(Load))]
public static class CIDatabase {
	internal static readonly CIDTile TileData = new();
	internal static readonly CIDWall WallData = new();

	public static void Load() {
		CIData.AltBlockEvent += AltBlockEvent_Delegate;

		TileData.Bake();
		WallData.Bake();
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	private static int AltBlockEvent_Delegate(ICIData data, in int baseTile, in byte conversionType, in ushort x, in ushort y) {
		if (conversionType <= BiomeConversionID.Dirt || baseTile == TileID.JungleGrass) {
			return baseTile;
		}
		if (!data.PurityConversion.TryGetValue(baseTile, out int test)) {
			test = CIData.KEEP;
		}
		return test;
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	public static int GetConvertedTile_Vanilla(in int baseTile, in byte conversionType, in ushort x, in ushort y) {
		return TileData.GetConverted_Vanilla(in baseTile, in conversionType, in x, in y);
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	public static int GetConvertedWall_Vanilla(in int baseTile, in byte conversionType, in ushort x, in ushort y) {
		return WallData.GetConverted_Vanilla(in baseTile, in conversionType, in x, in y);
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	public static int GetConvertedTile_Modded(in int baseTile, in IAltBiome conversionType, in ushort x, in ushort y) {
		return TileData.GetConverted_Modded(in baseTile, in conversionType, in x, in y);
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
	public static int GetConvertedWall_Modded(in int baseTile, in IAltBiome conversionType, in ushort x, in ushort y) {
		return WallData.GetConverted_Modded(in baseTile, in conversionType, in x, in y);
	}
}
