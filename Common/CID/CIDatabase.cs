using AltLibrary.Common.AltTypes;
using AltLibrary.Common.Attributes;
using AltLibrary.Content.Groups;
using System.Linq;
using System.Runtime.CompilerServices;
using Terraria.ID;
using Terraria.ModLoader;

namespace AltLibrary.Common.CID;

// TODO: More optimization.

// Conversion Inhertiance Database
public abstract class CIData {
	public const int Keep = -1;
	public const int Break = -2;

	public const int DecleminationId = 0;       // Purity
	public const int YellowSolutionId = 1;      // Desert
	public const int BrownSolutionId = 2;       // Forest
	public const int WhiteSolutionId = 3;       // Snow
	public const int DarkBlueSolutionId = 4;    // Mushroom

	protected readonly int[][] tiles;

	public CIData() {
		int size = 5 + UniteAltBiomes().Length;
		tiles = new int[size][];
		for (int i = 0; i < size; i++) {
			tiles[i] = TileID.Sets.Factory.CreateIntSet();
		}
	}

	public static IAltBiome[] UniteAltBiomes() => ModContent.GetContent<IAltBiome>().Where(x => x is AltBiome<EvilBiomeGroup> or AltBiome<GoodBiomeGroup>).ToArray();

	public abstract void Bake();

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void Set(ushort id, int baseTile, int tile) => tiles[id][baseTile] = tile;

	public void Set<T>(int baseTile, int tile) where T : class, IAltBiome => Set((ushort)(ModContent.GetInstance<T>().Type + 5), baseTile, tile);

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public int GetInfectedTile(int tile, ushort id) => tiles[id][tile];
}
[LoadableContent(ContentOrder.PostContent, nameof(Load))]
public static class CIDatabase {
	private static CIDTile TileData;
	private static CIDWall WallData;

	public static void Load() {
		(TileData = new()).Bake();
		(WallData = new()).Bake();
	}

	public static int GetConvertedTile(ushort conversionType, int baseTile) => TileData.GetInfectedTile(baseTile, conversionType);
	public static int GetConvertedTile(int conversionType, int baseTile) => GetConvertedTile((ushort)conversionType, baseTile);
	public static int GetConvertedTile<T>(int baseTile) where T : class, IAltBiome => GetConvertedTile(ModContent.GetInstance<T>().Type + 5, baseTile);

	public static int GetConvertedWall(ushort conversionType, int baseTile) => WallData.GetInfectedTile(baseTile, conversionType);
	public static int GetConvertedWall(int conversionType, int baseTile) => GetConvertedWall((ushort)conversionType, baseTile);
	public static int GetConvertedWall<T>(int baseTile) where T : class, IAltBiome => GetConvertedWall(ModContent.GetInstance<T>().Type + 5, baseTile);
}
