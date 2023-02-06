using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Attributes;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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

	public int GetConverted(int baseTile, in byte conversionType, in ushort x, in ushort y) {
		int forcedConvertedTile = KEEP;
		while (true) {
			int test = GetAltBlock(in baseTile, in conversionType, in x, in y);
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

	public virtual int GetAltBlock(in int baseTile, in byte conversionType, in ushort x, in ushort y) {
		int newTile;
		if (conversionType == BiomeConversionID.Purity) {
			if (!PurityConversion.TryGetValue(baseTile, out newTile)) {
				newTile = KEEP;
			}
		}
		else if (conversionType == BiomeConversionID.Corruption) {
			if (!CorruptionConversion.TryGetValue(baseTile, out newTile)) {
				newTile = KEEP;
			}
		}
		else if (conversionType == BiomeConversionID.Crimson) {
			if (!CrimsonConversion.TryGetValue(baseTile, out newTile)) {
				newTile = KEEP;
			}
		}
		else if (conversionType == BiomeConversionID.Hallow) {
			if (!HallowConversion.TryGetValue(baseTile, out newTile)) {
				newTile = KEEP;
			}
		}
		else if (conversionType == BiomeConversionID.GlowingMushroom) {
			if (!MushroomConversion.TryGetValue(baseTile, out newTile)) {
				newTile = KEEP;
			}
		}
		else if (conversionType == BiomeConversionID.Sand) {
			if (!DesertConversion.TryGetValue(baseTile, out newTile)) {
				newTile = KEEP;
			}
		}
		else if (conversionType == BiomeConversionID.Snow) {
			if (!SnowConversion.TryGetValue(baseTile, out newTile)) {
				newTile = KEEP;
			}
		}
		else if (conversionType == BiomeConversionID.Dirt) {
			if (!ForestConversion.TryGetValue(baseTile, out newTile)) {
				newTile = KEEP;
			}
		}
		else if (!Parent.TryGetValue(baseTile, out newTile)) {
			newTile = KEEP;
		}
		return newTile;
	}

	public abstract void Bake();

	public abstract int GetConverted_Vanilla(in int baseTile, in byte conversionType, in ushort x, in ushort y);
	public abstract int GetConverted_Modded(in int baseTile, in IAltBiome biome, in ushort x, in ushort y);

	public int GetConverted_Modded<T>(in int baseTile, in ushort x, in ushort y) where T : class, IAltBiome
		=> GetConverted_Modded(in baseTile, ModContent.GetInstance<T>(), in x, in y);
}
[LoadableContent(ContentOrder.Content, nameof(Load))]
public static class CIDatabase {
	private readonly record struct CachedData<T>(in int BaseTile, in T ConversionType, in ushort X, in ushort Y) {
	}

	internal static readonly CIDTile TileData = new();
	internal static readonly CIDWall WallData = new();

	private static readonly Dictionary<CachedData<byte>, int> CacheTile_V = new();
	private static readonly Dictionary<CachedData<IAAltType>, int> CacheTile_M = new();
	private static readonly Dictionary<CachedData<byte>, int> CacheWall_V = new();
	private static readonly Dictionary<CachedData<IAAltType>, int> CacheWall_M = new();

	public static void Load() {
		TileData.Bake();
		WallData.Bake();
	}

	public static int GetConvertedTile_Vanilla(in int baseTile, in byte conversionType, in ushort x, in ushort y) {
		var key = new CachedData<byte>(baseTile, conversionType, x, y);
		if (CacheTile_V.TryGetValue(key, out int newTile)) {
			return newTile;
		}
		newTile = TileData.GetConverted_Vanilla(in baseTile, in conversionType, in x, in y);
		CacheTile_V.Add(key, newTile);
		return newTile;
	}

	public static int GetConvertedWall_Vanilla(in int baseTile, in byte conversionType, in ushort x, in ushort y) {
		var key = new CachedData<byte>(baseTile, conversionType, x, y);
		if (CacheWall_V.TryGetValue(key, out int newTile)) {
			return newTile;
		}
		newTile = WallData.GetConverted_Vanilla(in baseTile, in conversionType, in x, in y);
		CacheWall_V.Add(key, newTile);
		return newTile;
	}

	public static int GetConvertedTile_Modded(in int baseTile, in IAltBiome conversionType, in ushort x, in ushort y) {
		var key = new CachedData<IAAltType>(baseTile, conversionType, x, y);
		if (CacheTile_M.TryGetValue(key, out int newTile)) {
			return newTile;
		}
		newTile = TileData.GetConverted_Modded(in baseTile, in conversionType, in x, in y);
		CacheTile_M.Add(key, newTile);
		return newTile;
	}

	public static int GetConvertedWall_Modded(in int baseTile, in IAltBiome conversionType, in ushort x, in ushort y) {
		var key = new CachedData<IAAltType>(baseTile, conversionType, x, y);
		if (CacheWall_M.TryGetValue(key, out int newTile)) {
			return newTile;
		}
		newTile = WallData.GetConverted_Modded(in baseTile, in conversionType, in x, in y);
		CacheWall_M.Add(key, newTile);
		return newTile;
	}
}
