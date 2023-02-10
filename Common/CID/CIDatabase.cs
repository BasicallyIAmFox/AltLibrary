using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Attributes;
using AltLibrary.Content.Groups;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Terraria.ID;
using Terraria.ModLoader;
using static AltLibrary.Common.CID.ConversionInheritanceData;

namespace AltLibrary.Common.CID;

// TODO: More optimization.

public abstract class ConversionInheritanceData {
	public const int Keep = -1;
	public const int Break = -2;

	public const int DecleminationId = 0;       // Purity
	public const int YellowSolutionId = 1;      // Desert
	public const int BrownSolutionId = 2;       // Forest
	public const int WhiteSolutionId = 3;       // Snow
	public const int DarkBlueSolutionId = 4;    // Mushroom

	private readonly int[] tiles;

	public ConversionInheritanceData() {
		int size = 5 + UniteAltBiomes().LastOrDefault()?.Type ?? 0;
		tiles = new SetFactory(size * TileLoader.TileCount).CreateIntSet(defaultState: Keep);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static int GetId(int id, int tile) => id * TileLoader.TileCount + tile;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static int GetConversionIdOf<T>() where T : class, IAltBiome => 5 + ModContent.GetInstance<T>().Type;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static IEnumerable<IAltBiome> UniteAltBiomes() => ModContent.GetContent<IAltBiome>().Where(x => x is AltBiome<EvilBiomeGroup> or AltBiome<GoodBiomeGroup>);

	public abstract void Bake();

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public int Get(int id, int tile) => tiles[GetId(id, tile)];

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public int Get<T>(int tile) where T : class, IAltBiome => Get(GetConversionIdOf<T>(), tile);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void Set(int id, int baseTile, int tile) => tiles[GetId(id, baseTile)] = tile;

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void Set<T>(int baseTile, int tile) where T : class, IAltBiome => Set(GetConversionIdOf<T>(), baseTile, tile);
}
[LoadableContent(ContentOrder.PostContent, nameof(Load))]
public static class ConversionInheritanceDatabase {
	private static ConversionInheritanceDataTile TileData;
	private static ConversionInheritanceDataWall WallData;

	public static void Load() {
		(TileData = new()).Bake();
		(WallData = new()).Bake();
	}

	public static int GetConvertedTile(int conversionType, int baseTile) => TileData.Get(baseTile, conversionType);
	public static int GetConvertedTile<T>(int baseTile) where T : class, IAltBiome => GetConvertedTile(GetConversionIdOf<T>(), baseTile);

	public static int GetConvertedWall(int conversionType, int baseTile) => WallData.Get(baseTile, conversionType);
	public static int GetConvertedWall<T>(int baseTile) where T : class, IAltBiome => GetConvertedWall(GetConversionIdOf<T>(), baseTile);
}
