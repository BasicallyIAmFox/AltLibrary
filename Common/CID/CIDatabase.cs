using AltLibrary.Common.AltTypes;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace AltLibrary.Common.CID;

// Conversion Inhertiance Database
public abstract class CIData {
	protected Dictionary<int, int> Parent = new();

	protected Dictionary<int, int> PurityConversion = new();
	protected Dictionary<int, int> HallowConversion = new();
	protected Dictionary<int, int> CorruptionConversion = new();
	protected Dictionary<int, int> CrimsonConversion = new();
	protected Dictionary<int, int> MushroomConversion = new();
	protected Dictionary<int, int> ForestConversion = new();
	protected Dictionary<int, int> DesertConversion = new();
	protected Dictionary<int, int> SnowConversion = new();

	protected Dictionary<int, BitsByte> ForceDeconversion = new();
	protected Dictionary<int, BitsByte> BreakIfConversionFail = new();

	public const int KEEP = -1;
	public const int KILL = -2;

	public int GetConverted(int baseTile, int conversionType, int x, int y) {
		int forcedConvertedTile = KEEP;
		while (true) {
			int test = GetAltBlock(baseTile, conversionType, x, y);
			if (test != KEEP) {
				return test;
			}

			if (BreakIfConversionFail.TryGetValue(baseTile, out BitsByte bits)) {
				if (bits[conversionType])
					forcedConvertedTile = KILL;
			}

			if (!Parent.TryGetValue(baseTile, out test)) {
				return forcedConvertedTile;
			}

			if (ForceDeconversion.TryGetValue(baseTile, out bits)) {
				if (bits[conversionType])
					forcedConvertedTile = test;
			}
			baseTile = test;
		}
	}

	public virtual int GetAltBlock(int baseTile, int conversionType, int x, int y) {
		int newTile;
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

	public abstract int GetConverted_Vanilla(int baseTile, int conversionType, int x, int y);
	public abstract int GetConverted_Modded(int baseTile, IAltBiome biome, int x, int y);
}
public class CIDatabase {
	internal static readonly CIDTile TileData = new();
	internal static readonly CIDWall WallData = new();

	public static int GetConvertedTile_Vanilla(int baseTile, int conversionType, int x, int y) {
		return TileData.GetConverted_Vanilla(baseTile, conversionType, x, y);
	}
	public static int GetConvertedWall_Vanilla(int baseTile, int conversionType, int x, int y) {
		return WallData.GetConverted_Vanilla(baseTile, conversionType, x, y);
	}
	public static int GetConvertedTile_Modded(int baseTile, IAltBiome conversionType, int x, int y) {
		return TileData.GetConverted_Modded(baseTile, conversionType, x, y);
	}
	public static int GetConvertedWall_Modded(int baseTile, IAltBiome conversionType, int x, int y) {
		return WallData.GetConverted_Modded(baseTile, conversionType, x, y);
	}
}
